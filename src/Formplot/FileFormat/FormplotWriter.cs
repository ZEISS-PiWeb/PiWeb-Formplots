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

	#endregion

	/// <summary>
	/// Extension class to provide serialization of form plots.
	/// </summary>
	internal static class FormplotWriter
	{
		#region members

		private static readonly XmlWriterSettings XmlWriterSettings = new XmlWriterSettings
		{
			Indent = true,
			Encoding = Encoding.UTF8,
			CloseOutput = false
		};

		#endregion

		#region methods

		/// <summary>
		/// Writes the data of the plot into the specified <paramref name="stream"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		internal static void WriteTo<TPoint, TGeometry>( this Formplot<TPoint, TGeometry> plot, Stream stream ) where TPoint : Point<TPoint, TGeometry>, new() where TGeometry : Geometry, new()
		{
			if( stream == null )
				throw new ArgumentNullException( nameof( stream ) );

			using( var zipOutput = new ZipArchive( stream, ZipArchiveMode.Create, true, Encoding.UTF8 ) )
			{
				WriteFileVersion( plot, zipOutput );

				using( var metadataStream = new MemoryStream() )
				{
					using( var writer = XmlWriter.Create( metadataStream, XmlWriterSettings ) )
					{
						Stream? pointDataStream;

						if( plot.FormplotType != FormplotTypes.None )
						{
							var plotPointsEntry = zipOutput.CreateEntry( "plotpoints.dat", CompressionLevel.Fastest );
							plotPointsEntry.LastWriteTime = new DateTime( 1980, 1, 1 );
							pointDataStream = plotPointsEntry.Open();
						}
						else
						{
							pointDataStream = null;
						}

						writer.WriteStartDocument( true );
						writer.WriteStartElement( plot.FormplotType == FormplotTypes.None ? FormplotReader.PropertiesRootTag : FormplotReader.FormplotRootTag );

						plot.Serialize( writer, pointDataStream );

						writer.WriteEndElement();
						writer.WriteEndDocument();

						pointDataStream?.Close();
					}

					var metaDataEntry = zipOutput.CreateEntry( "header.xml", CompressionLevel.Optimal );
					metaDataEntry.LastWriteTime = new DateTime( 1980, 1, 1 );

					using( var metaDataZipStream = metaDataEntry.Open() )
						metadataStream.WriteTo( metaDataZipStream );
				}
			}
		}

		private static void WriteFileVersion<TPoint, TGeometry>( Formplot<TPoint, TGeometry> plot, ZipArchive zipOutput ) where TPoint : Point<TPoint, TGeometry>, new() where TGeometry : Geometry, new()
		{
			var versionInfoEntry = zipOutput.CreateEntry( "fileversion.txt", CompressionLevel.NoCompression );
			versionInfoEntry.LastWriteTime = new DateTime( 1980, 1, 1 );

			using var versionInfoEntryStream = versionInfoEntry.Open();
			using var textWriter = new StreamWriter( versionInfoEntryStream, Encoding.UTF8 );

			textWriter.Write( FormplotHelper.GetFileFormatVersion( plot.FormplotType ).ToString( 2 ) );
		}

		/// <summary>
		/// Serializes the formplot. The header information is written to the <paramref name="metaDataWriter" />, whereas the points are stored as a
		/// blob in the <paramref name="pointDataStream" /></summary>
		/// <param name="plot">The plot.</param>
		/// <param name="metaDataWriter">An XML writer to store the header data, such as tolerances and segments.</param>
		/// <param name="pointDataStream">A stream to store the binary point data.</param>
		private static void Serialize<TPoint, TGeometry>( this Formplot<TPoint, TGeometry> plot, XmlWriter metaDataWriter, Stream? pointDataStream )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			if( metaDataWriter == null )
				throw new ArgumentNullException( nameof( metaDataWriter ) );

			if( plot.FormplotType != FormplotTypes.None )
			{
				metaDataWriter.WriteAttributeString( "Type", plot.FormplotType.ToString() );
			}

			metaDataWriter.WriteStartElement( "CreatorSoftware" );
			metaDataWriter.WriteValue( plot.CreatorSoftware );
			metaDataWriter.WriteEndElement();

			metaDataWriter.WriteStartElement( "CreatorSoftwareVersion" );
			metaDataWriter.WriteValue( plot.CreatorSoftwareVersion.ToString() );
			metaDataWriter.WriteEndElement();

			if( plot.Properties.Count > 0 )
			{
				foreach( var property in plot.Properties )
				{
					metaDataWriter.WriteStartElement( "Property" );
					property.Serialize( metaDataWriter );
					metaDataWriter.WriteEndElement();
				}
			}

			if( plot.FormplotType != FormplotTypes.None && pointDataStream != null )
				WriteFormplotSpecificProperties( plot, metaDataWriter, pointDataStream );
		}

		private static void WriteFormplotSpecificProperties<TPoint, TGeometry>( Formplot<TPoint, TGeometry> plot, XmlWriter metaDataWriter, Stream pointDataStream ) where TPoint : Point<TPoint, TGeometry>, new() where TGeometry : Geometry, new()
		{
			if( !plot.Tolerance.IsEmpty )
			{
				metaDataWriter.WriteStartElement( "Tolerance" );
				plot.Tolerance.Serialize( metaDataWriter );
				metaDataWriter.WriteEndElement();
			}

			if( plot.DefaultErrorScaling.HasValue )
			{
				metaDataWriter.WriteStartElement( "ErrorScaling" );
				metaDataWriter.WriteValue( XmlConvert.ToString( plot.DefaultErrorScaling.Value ) );
				metaDataWriter.WriteEndElement();
			}

			if( plot.PreserveAspectRatio.HasValue )
			{
				metaDataWriter.WriteStartElement( "PreserveAspectRatio" );
				metaDataWriter.WriteValue( XmlConvert.ToString( plot.PreserveAspectRatio.Value ) );
				metaDataWriter.WriteEndElement();
			}

			if( plot.FormplotType == FormplotTypes.Straightness && plot.ProjectionAxis != ProjectionAxis.None )
			{
				metaDataWriter.WriteStartElement( "ProjectionAxis" );
				metaDataWriter.WriteValue( plot.ProjectionAxis.ToString() );
				metaDataWriter.WriteEndElement();
			}

			if( plot.Actual.GeometryType != GeometryTypes.None )
			{
				metaDataWriter.WriteStartElement( "Geometry" );
				metaDataWriter.WriteAttributeString( "Type", plot.GeometryType.ToString() );

				metaDataWriter.WriteStartElement( "Nominal" );
				plot.Nominal.Serialize( metaDataWriter );
				metaDataWriter.WriteEndElement();

				metaDataWriter.WriteStartElement( "Actual" );
				plot.Actual.Serialize( metaDataWriter );
				metaDataWriter.WriteEndElement();

				metaDataWriter.WriteEndElement();
			}

			metaDataWriter.WriteStartElement( "Points" );
			plot.WritePoints( metaDataWriter, pointDataStream );
			metaDataWriter.WriteEndElement();
		}

		/// <summary>
		/// Writes the points into the specified <see cref="Stream" /> and writes their metadata with the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="plot">The plot.</param>
		/// <param name="writer">The xml writer.</param>
		/// <param name="pointDataStream">The point data stream.</param>
		private static void WritePoints<TPoint, TGeometry>( this Formplot<TPoint, TGeometry> plot, XmlWriter writer, Stream pointDataStream )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			if( writer == null )
				throw new ArgumentNullException( nameof( writer ) );

			if( pointDataStream == null )
				throw new ArgumentNullException( nameof( pointDataStream ) );

			writer.WriteStartElement( "Count" );
			writer.WriteValue( XmlConvert.ToString( plot.Points.Count() ) );
			writer.WriteEndElement();

			var propertyLists = new Dictionary<Property, RangeList>();
			var statelists = new Dictionary<PointState, RangeList>();
			var segmentlists = new Dictionary<Segment<TPoint, TGeometry>, RangeList>();
			var tolerancelists = new Dictionary<Tolerance, RangeList>();

			var lastPoint = default( Point<TPoint, TGeometry> );
			var index = 0;

			using( var pointDataWriter = new BinaryWriter( pointDataStream ) )
			{
				foreach( var point in plot.Points )
				{
					CollectPointProperties( point, propertyLists, index, lastPoint );
					CollectStates( point, statelists, index, lastPoint );
					CollectSegments( point, segmentlists, index, lastPoint );
					CollectTolerances( point, tolerancelists, index, lastPoint );
					point.WriteToStream( pointDataWriter );

					lastPoint = point;
					index++;
				}
			}

			WritePropertyLists( writer, propertyLists );
			WriteStates( writer, statelists );
			WriteSegments( writer, segmentlists );
			WriteTolerances( writer, tolerancelists );
		}

		private static void CollectTolerances( Point point, IDictionary<Tolerance, RangeList> tolerancelists, int index, Point? lastPoint )
		{
			if( point.Tolerance.IsEmpty )
				return;

			if( !tolerancelists.ContainsKey( point.Tolerance ) )
				tolerancelists.Add( point.Tolerance, new RangeList { new Range( index ) } );
			else
			{
				if( lastPoint == null )
					return;

				if( Tolerance.Equals( point.Tolerance, lastPoint.Tolerance ) )
					tolerancelists[ point.Tolerance ].Last().End = index;
				else
					tolerancelists[ point.Tolerance ].Add( new Range( index ) );
			}
		}

		private static void CollectSegments<TPoint, TGeometry>( Point<TPoint, TGeometry> point, IDictionary<Segment<TPoint, TGeometry>, RangeList> segmentlists, int index, Point<TPoint, TGeometry>? lastPoint )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			if( point.Segment == null )
				return;

			if( !segmentlists.ContainsKey( point.Segment ) )
				segmentlists.Add( point.Segment, new RangeList { new Range( index ) } );
			else
			{
				if( lastPoint == null )
					return;

				if( Equals( point.Segment, lastPoint.Segment ) )
					segmentlists[ point.Segment ].Last().End = index;
				else
					segmentlists[ point.Segment ].Add( new Range( index ) );
			}
		}

		private static void CollectStates( Point point, IDictionary<PointState, RangeList> statelists, int index, Point? lastPoint )
		{
			if( point.State == PointState.None )
				return;

			if( !statelists.ContainsKey( point.State ) )
				statelists.Add( point.State, new RangeList { new Range( index ) } );
			else
			{
				if( lastPoint == null )
					return;

				if( point.State == lastPoint.State )
					statelists[ point.State ].Last().End = index;
				else
					statelists[ point.State ].Add( new Range( index ) );
			}
		}

		private static void CollectPointProperties( Point point, IDictionary<Property, RangeList> propertyLists, int index, Point? lastPoint )
		{
			foreach( var property in point.Properties )
			{
				if( !propertyLists.ContainsKey( property ) )
				{
					propertyLists.Add( property, new RangeList { new Range( index ) } );
					continue;
				}

				if( lastPoint == null )
					continue;

				if( lastPoint.Properties.Contains( property ) )
					propertyLists[ property ].Last().End = index;
				else
					propertyLists[ property ].Add( new Range( index ) );
			}
		}

		private static void WriteTolerances( XmlWriter writer, IReadOnlyCollection<KeyValuePair<Tolerance, RangeList>> tolerancelists )
		{
			if( tolerancelists.Count <= 0 )
				return;

			writer.WriteStartElement( "Tolerances" );

			foreach( var tolerancelist in tolerancelists )
			{
				writer.WriteStartElement( "Tolerance" );
				writer.WriteAttributeString( "Points", tolerancelist.Value.ToString() );
				tolerancelist.Key.Serialize( writer );
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		private static void WriteSegments<TPoint, TGeometry>( XmlWriter writer, IReadOnlyCollection<KeyValuePair<Segment<TPoint, TGeometry>, RangeList>> segmentlists )
			where TPoint : Point<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			if( segmentlists.Count <= 0 )
				return;

			writer.WriteStartElement( "Segments" );

			foreach( var segment in segmentlists )
			{
				writer.WriteStartElement( "Segment" );
				writer.WriteAttributeString( "Points", segment.Value.ToString() );

				if( segment.Key.SegmentType != SegmentTypes.None )
					writer.WriteAttributeString( "Type", segment.Key.SegmentType.ToString() );

				writer.WriteValue( segment.Key.Name );
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		private static void WriteStates( XmlWriter writer, IReadOnlyCollection<KeyValuePair<PointState, RangeList>> statelists )
		{
			if( statelists.Count <= 0 )
				return;

			writer.WriteStartElement( "States" );

			foreach( var statelist in statelists )
			{
				writer.WriteStartElement( "State" );
				writer.WriteAttributeString( "Points", statelist.Value.ToString() );
				writer.WriteValue( statelist.Key.ToString() );
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		private static void WritePropertyLists( XmlWriter writer, IReadOnlyCollection<KeyValuePair<Property, RangeList>> propertyLists )
		{
			if( propertyLists.Count <= 0 )
				return;

			writer.WriteStartElement( "Properties" );

			foreach( var propertyList in propertyLists )
			{
				writer.WriteStartElement( "Property" );
				writer.WriteAttributeString( "Points", propertyList.Value.ToString() );
				propertyList.Key.Serialize( writer );
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		#endregion
	}
}