#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Text;
	using System.Xml;
	using Zeiss.PiWeb.Formplot.Common;

	#endregion

	internal static class FormplotReader
	{
		#region constants

		internal const string PropertiesRootTag = "Properties";
		internal const string FormplotRootTag = "Formplot";

		#endregion

		#region members

		// ReSharper disable once InconsistentNaming
		[ThreadStatic] private static NameTable? _NameTable;

		#endregion

		#region properties

		private static NameTable NameTable => _NameTable ??= new NameTable();

		#endregion

		#region methods

		internal static Formplot? ReadFrom( Stream stream, FormplotTypes[]? acceptedTypes = null )
		{
			if( stream == null ) throw new ArgumentNullException( nameof( stream ) );

			using var zipFile = new ZipArchive( stream, ZipArchiveMode.Read, true, Encoding.UTF8 );

			var version = ReadFileVersion( zipFile );
			var metadataStream = ReadMetadataStream( zipFile );

			using var pointDataStream = zipFile.GetEntry( "plotpoints.dat" )?.Open();

			return ReadFormplot( metadataStream, pointDataStream, version, acceptedTypes );
		}

		private static Formplot? ReadFormplot( Stream metadataStream, Stream? pointDataStream, Version version, FormplotTypes[]? acceptedTypes )
		{
			using var reader = XmlReader.Create( metadataStream, CreateXmlReaderSettings() );

			reader.MoveToElement();
			while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
			{
				if( reader.Name != FormplotRootTag && reader.Name != PropertiesRootTag )
					continue;

				if( !EnumParser<FormplotTypes>.TryParse( reader.GetAttribute( "Type" ) ?? "", out var plotType ) )
					plotType = FormplotTypes.None; // This code path is for Property Files (they don't have a form plot type)

				if( acceptedTypes != null && !acceptedTypes.Contains( plotType ) )
					return null;

				var targetVersion = FormplotHelper.GetFileFormatVersion( plotType );
				if( version > targetVersion )
					throw new NotSupportedException( $"Invalid form plot file. Version {version} is not supported." );

				return CreateFormplot( pointDataStream, version, plotType, reader );
			}

			return null;
		}

		private static Formplot CreateFormplot( Stream? pointDataStream, Version version, FormplotTypes formplotType, XmlReader reader )
		{
			return formplotType switch
			{
				FormplotTypes.None            => Deserialize<EmptyPlot, EmptyPoint, EmptyGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.Roundness       => Deserialize<RoundnessPlot, CirclePoint, CircleGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.Flatness        => Deserialize<FlatnessPlot, PlanePoint, PlaneGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.CurveProfile    => Deserialize<CurveProfilePlot, CurvePoint, CurveGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.Straightness    => Deserialize<StraightnessPlot, LinePoint, LineGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.Cylindricity    => Deserialize<CylindricityPlot, CylinderPoint, CylinderGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.Pitch           => Deserialize<PitchPlot, PitchPoint, PitchGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.BorePattern     => Deserialize<BorePatternPlot, CurvePoint, CurveGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.CircleInProfile => Deserialize<CircleInProfilePlot, CircleInProfilePoint, CircleInProfileGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.Fourier         => Deserialize<FourierPlot, FourierPoint, EmptyGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.FlushGap        => Deserialize<FlushGapPlot, FlushGapPoint, FlushGapGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.Defect          => Deserialize<DefectPlot, Defect, DefectGeometry>( reader, pointDataStream, formplotType, version ),
				FormplotTypes.Fillet          => Deserialize<FilletPlot, FilletPoint, FilletGeometry>( reader, pointDataStream, formplotType, version ),
				_                             => throw new NotSupportedException( $"Invalid form plot file. The plot with type {formplotType} is not supported." )
			};
		}

		private static TPlot Deserialize<TPlot, TPoint, TGeometry>( XmlReader metaDataReader, Stream? pointDataStream, FormplotTypes formplotType, Version version )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TPlot : Formplot<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			var plot = new TPlot();
			while( metaDataReader.Read() && metaDataReader.NodeType != XmlNodeType.EndElement )
			{
				switch( metaDataReader.Name )
				{
					case "CreatorSoftware":
						plot.CreatorSoftware = metaDataReader.ReadString();
						break;
					case "CreatorSoftwareVersion":
						plot.CreatorSoftwareVersion = new Version( metaDataReader.ReadString() );
						break;
					case "Property":
						plot.Properties.Add( Property.Deserialize( metaDataReader ) );
						break;
					case "Tolerance":
						plot.Tolerance = Tolerance.Deserialize( metaDataReader );
						break;
					case "ErrorScaling":
						plot.DefaultErrorScaling = XmlConvert.ToDouble( metaDataReader.ReadString() );
						break;
					case "PreserveAspectRatio":
						plot.PreserveAspectRatio = XmlConvert.ToBoolean( metaDataReader.ReadString() );
						break;
					case "ProjectionAxis":
						if( formplotType != FormplotTypes.Straightness )
							throw new NotSupportedException( "only straightness formplot data supports a projection axis" );
						plot.ProjectionAxis = EnumParser<ProjectionAxis>.Parse( metaDataReader.ReadString() );
						break;
					case "Geometry":
						ReadGeometry<TPlot, TPoint, TGeometry>( metaDataReader, formplotType, version, plot );
						break;
					case "Points":
						ReadPoints( plot, metaDataReader, pointDataStream );
						break;
				}
			}

			return plot;
		}

		private static void ReadGeometry<TPlot, TPoint, TGeometry>( XmlReader metaDataReader, FormplotTypes formplotType, Version version, TPlot plot ) where TPoint : Point<TPoint, TGeometry>, new() where TPlot : Formplot<TPoint, TGeometry>, new() where TGeometry : Geometry, new()
		{
			var geometryString = metaDataReader.GetAttribute( "Type" );
			if( !EnumParser<GeometryTypes>.TryParse( metaDataReader.GetAttribute( "Type" ), out var geometryType ) )
				throw new NotSupportedException( $"Invalid form plot file. The geometry type \"{geometryString}\" is not supported." );

			if( geometryType != plot.Actual.GeometryType )
				throw new NotSupportedException( $"Invalid form plot file. The geometry type \"{geometryType}\" for plot type \"{formplotType}\" is not supported." );

			while( metaDataReader.Read() && metaDataReader.NodeType != XmlNodeType.EndElement )
			{
				switch( metaDataReader.Name )
				{
					case "Nominal":
						plot.Nominal.Deserialize( metaDataReader, version );
						break;
					case "Actual":
						plot.Actual.Deserialize( metaDataReader, version );
						break;
				}
			}
		}

		private static void ReadPoints<TPoint, TGeometry>( Formplot<TPoint, TGeometry> plot, XmlReader metadataReader, Stream? pointdataStream )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			var pointCount = 0;
			var properties = new Dictionary<Property, RangeList>();
			var states = new Dictionary<PointState, RangeList>();
			var segments = new Dictionary<Segment<TPoint, TGeometry>, RangeList>();
			var tolerances = new Dictionary<Tolerance, RangeList>();

			while( metadataReader.Read() && metadataReader.NodeType != XmlNodeType.EndElement )
			{
				if( metadataReader.IsEmptyElement ) continue;

				switch( metadataReader.Name )
				{
					case "Count":
						pointCount = XmlConvert.ToInt32( metadataReader.ReadString() );
						break;
					case "Properties":
						ReadProperties( metadataReader, properties );
						break;
					case "States":
						ReadStates( metadataReader, states );
						break;
					case "Segments":
						ReadSegments( metadataReader, segments );
						break;
					case "Tolerances":
						ReadTolerances( metadataReader, tolerances );
						break;
				}
			}

			foreach( var segment in segments.Keys )
				plot.Segments.Add( segment );

			var plotPoints = DeserializePoints<TPoint, TGeometry>( pointdataStream, pointCount );

			ApplyProperties<TPoint, TGeometry>( properties, plotPoints );
			ApplyPointStates<TPoint, TGeometry>( states, plotPoints );
			ApplySegments( segments, plotPoints );

			var defaultSegment = new Segment<TPoint, TGeometry>( "", SegmentTypes.None );
			if( SetDefaultSegment( defaultSegment, plotPoints ) )
			{
				// Set name to FormplotType as fallback
				defaultSegment.Name = plot.FormplotType.ToString();
				plot.Segments.Add( defaultSegment );
			}

			ApplyTolerances<TPoint, TGeometry>( tolerances, plotPoints );
		}

		private static void ReadTolerances( XmlReader metadataReader, IDictionary<Tolerance, RangeList> tolerances )
		{
			var isTolerancesElement = false;
			while( metadataReader.NodeType != XmlNodeType.EndElement || !isTolerancesElement )
			{
				if( metadataReader.NodeType == XmlNodeType.Element && metadataReader.Name.Equals( "Tolerance" ) )
				{
					var range = RangeList.TryParseOrEmpty( metadataReader.GetAttribute( "Points" ) );
					var tolerance = Tolerance.Deserialize( metadataReader );

					tolerances[ tolerance ] = range;
				}

				metadataReader.Read();
				isTolerancesElement = metadataReader.Name.Equals( "Tolerances" );
			}
		}

		private static void ReadSegments<TPoint, TGeometry>( XmlReader metadataReader, IDictionary<Segment<TPoint, TGeometry>, RangeList> segments ) where TPoint : Point<TPoint, TGeometry>, new() where TGeometry : Geometry, new()
		{
			while( metadataReader.Read() && metadataReader.NodeType != XmlNodeType.EndElement )
			{
				if( metadataReader.Name != "Segment" )
					continue;

				var range = RangeList.TryParseOrEmpty( metadataReader.GetAttribute( "Points" ) );
				var typeString = metadataReader.GetAttribute( "Type" );

				var type = SegmentTypes.None;
				if( !string.IsNullOrEmpty( typeString ) )
					type = EnumParser<SegmentTypes>.Parse( typeString );

				var name = metadataReader.ReadString();

				var segment = new Segment<TPoint, TGeometry>( name, type );

				segments[ segment ] = range;
			}
		}

		private static void ReadStates( XmlReader metadataReader, IDictionary<PointState, RangeList> states )
		{
			while( metadataReader.Read() && metadataReader.NodeType != XmlNodeType.EndElement )
			{
				if( metadataReader.Name != "State" )
					continue;

				var range = RangeList.TryParseOrEmpty( metadataReader.GetAttribute( "Points" ) );
				var state = EnumParser<PointState>.Parse( metadataReader.ReadString() );

				states[ state ] = range;
			}
		}

		private static void ReadProperties( XmlReader metadataReader, IDictionary<Property, RangeList> properties )
		{
			while( metadataReader.Read() && metadataReader.NodeType != XmlNodeType.EndElement )
			{
				if( metadataReader.Name != "Property" )
					continue;

				var range = RangeList.TryParseOrEmpty( metadataReader.GetAttribute( "Points" ) );
				var property = Property.Deserialize( metadataReader );

				properties[ property ] = range;
			}
		}

		private static List<TPoint> DeserializePoints<TPoint, TGeometry>( Stream? pointDataStream, int pointCount )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			var result = new List<TPoint>( pointCount );
			if( pointDataStream is null )
				return result;

			using var binaryReader = new BinaryReader( pointDataStream );
			for( var index = 0; index < pointCount; index++ )
			{
				var point = new TPoint { Index = index };
				point.ReadFromStream( binaryReader );

				result.Add( point );
			}

			return result;
		}

		private static void ApplyProperties<TPoint, TGeometry>( Dictionary<Property, RangeList> propertyRangeLists, List<TPoint> points )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			foreach( var propertyRange in propertyRangeLists )
			{
				var property = propertyRange.Key;
				foreach( var range in propertyRange.Value )
				{
					FormplotHelper.VerifyValidRange( points.Count, range, property );
					for( var i = range.Start; i <= range.End; i++ )
					{
						points[ i ].Properties.Add( property );
					}
				}
			}
		}

		private static void ApplyPointStates<TPoint, TGeometry>( Dictionary<PointState, RangeList> stateRangeLists, IReadOnlyList<TPoint> points )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			foreach( var stateRange in stateRangeLists )
			{
				var state = stateRange.Key;
				if( state == PointState.None )
					continue;

				foreach( var range in stateRange.Value )
				{
					FormplotHelper.VerifyValidRange( points.Count, range, state );
					for( var i = range.Start; i <= range.End; i++ )
					{
						points[ i ].State |= state;
					}
				}
			}
		}

		private static void ApplySegments<TPoint, TGeometry>( Dictionary<Segment<TPoint, TGeometry>, RangeList> segmentRangeLists, IReadOnlyList<TPoint> points )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			foreach( var segmentRange in segmentRangeLists )
			{
				var segment = segmentRange.Key;
				foreach( var range in segmentRange.Value )
				{
					FormplotHelper.VerifyValidRange( points.Count, range, segment );
					for( var i = range.Start; i <= range.End; i++ )
					{
						segment.Points.Add( points[ i ] );
					}
				}
			}
		}

		private static bool SetDefaultSegment<TPoint, TGeometry>( Segment<TPoint, TGeometry> defaultSegment, IEnumerable<TPoint> points )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			var result = false;

			foreach( var point in points )
			{
				if( point.Segment != null )
					continue;

				defaultSegment.Points.Add( point );
				result = true;
			}

			return result;
		}

		private static void ApplyTolerances<TPoint, TGeometry>( Dictionary<Tolerance, RangeList> toleranceRangeLists, IReadOnlyList<TPoint> points )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			foreach( var toleranceRange in toleranceRangeLists )
			{
				var tolerance = toleranceRange.Key;
				foreach( var range in toleranceRange.Value )
				{
					FormplotHelper.VerifyValidRange( points.Count, range, tolerance );
					for( var i = range.Start; i <= range.End; i++ )
					{
						points[ i ].Tolerance = tolerance;
					}
				}
			}
		}

		private static Stream ReadMetadataStream( ZipArchive zipFile )
		{
			var headerEntry = zipFile.GetEntry( "header.xml" );
			if( headerEntry == null )
				throw new NotSupportedException( "Invalid form plot file. The entry \"header.xml\" is missing." );

			return FormplotHelper.ReadAndSanitizeHeaderEntry( headerEntry );
		}

		private static Version ReadFileVersion( ZipArchive zipFile )
		{
			var fileVersionEntry = zipFile.GetEntry( "fileversion.txt" );
			if( fileVersionEntry == null )
				throw new NotSupportedException( "Invalid form plot file. The entry \"fileversion.txt\" is missing." );

			using var inputStream = fileVersionEntry.Open();
			using var reader = new StreamReader( inputStream, Encoding.UTF8 );

			return new Version( reader.ReadToEnd() );
		}

		private static XmlReaderSettings CreateXmlReaderSettings()
		{
			return new XmlReaderSettings
			{
				IgnoreComments = true,
				IgnoreWhitespace = true,
				IgnoreProcessingInstructions = true,
				CloseInput = false,
				NameTable = NameTable
			};
		}

		#endregion
	}
}