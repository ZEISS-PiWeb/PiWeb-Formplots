#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Text;
	using System.Xml;
	using System.Xml.Linq;
	using System.Xml.Xsl;
	using Zeiss.IMT.PiWeb.Formplot.Common;

	#endregion

	internal static class FormplotHelpers
	{
		#region constants

		internal const string PropertiesRootTag = "Properties";
		internal const string FormplotRootTag = "Formplot";

		#endregion

		#region members

		private static readonly Lazy<HashSet<string>> UpgradeResourceNamesLazy = new Lazy<HashSet<string>>( () => new HashSet<string>( typeof( Formplot ).Assembly.GetManifestResourceNames().Where( s => s.StartsWith( typeof( Formplot ).Namespace + ".Compatibility.", StringComparison.OrdinalIgnoreCase ) && s.EndsWith( ".xsl", StringComparison.OrdinalIgnoreCase ) ) ) );

		#endregion

		#region properties

		private static HashSet<string> UpgradeResourceNames => UpgradeResourceNamesLazy.Value;

		#endregion

		#region methods

		/// <summary>
		/// Gets the current file format version of a specific <see cref="FormplotTypes"/>.
		/// </summary>
		internal static Version GetFileFormatVersion( FormplotTypes formplotType )
		{
			switch( formplotType )
			{
				case FormplotTypes.None:
					return new Version( 1, 0 );
				default:
					return new Version( 2, 0 );
			}
		}

		internal static Formplot ReadFrom( Stream stream )
		{
			if( stream == null )
			{
				throw new ArgumentNullException( nameof( stream ) );
			}

			using( var zipFile = new ZipArchive( stream, ZipArchiveMode.Read ) )
			using( var metadataStream = new MemoryStream() )
			{
				var fileVersionEntry = zipFile.GetEntry( "fileversion.txt" );

				Version version;
				if( fileVersionEntry != null )
				{
					using( var inputStream = fileVersionEntry.Open() )
					using( var reader = new StreamReader( inputStream, Encoding.UTF8 ) )
					{
						version = new Version( reader.ReadToEnd() );
					}
				}
				else
				{
					throw new NotSupportedException( "\"fileversion.txt\" is missing. data file isn't supported." );
				}

				var headerEntry = zipFile.GetEntry( "header.xml" );

				if( headerEntry != null )
				{
					if( headerEntry.Length > Int32.MaxValue )
						throw new NotSupportedException( "Zip file entry header.xml is larger than 2GB." );

					using( var inputStream = headerEntry.Open() )
					{
						// Ok, so here is the problem: Some applications writes broken XML files which end on ascii zero bytes. The XML loader used in GetFormplotType() does not like this.
						// We need to cut of trailling zeros. However, we cannot do this in little endian UTF-16 encoded files (the last zero would be part of the '>' character).
						// So we compare the first few bytes of the XML header to what Calypso would write to make sure it is not UTF-16.
						var content = ReadFully( inputStream );
						var contentLength = IsTruncationSafe( content ) ? FindEndOfContent( content ) + 1 : content.Length;
						metadataStream.Write( content, 0, contentLength );
					}
					metadataStream.Seek( 0, SeekOrigin.Begin );

					var plotType = GetFormplotType( metadataStream );
					var targetVersion = GetFileFormatVersion( plotType );

					if( version > targetVersion )
						throw new NotSupportedException( $"Version {version} isn't supported" );

					string upgradeResourceName = typeof( Formplot ).Namespace + ".Compatibility." + plotType + "_" + version.ToString( 2 ) + ".xsl";

					if( version != targetVersion && !UpgradeResourceNames.Contains( upgradeResourceName ) )
					{
						throw new NotSupportedException( "formplot upgrade resource is missing" );
					}

					if( UpgradeResourceNames.Contains( upgradeResourceName ) )
					{
						var settings = new XmlReaderSettings
						{
							IgnoreComments = true,
							IgnoreWhitespace = true,
							IgnoreProcessingInstructions = true,
							CloseInput = false,
							NameTable = new NameTable(),
						};
						var transform = new XslCompiledTransform();

						using( var transformInputStream = typeof( Formplot ).Assembly.GetManifestResourceStream( upgradeResourceName ) )
						{
							if (transformInputStream == null)
								throw new NotSupportedException( "couldn't load upgrade resource" );

							using( var reader = XmlReader.Create( transformInputStream, settings ) )
							{
								transform.Load( reader );
							}
						}
						

						using( var reader = XmlReader.Create( metadataStream, settings ) )
						using( var newMetadataStream = new MemoryStream { Capacity = metadataStream.Capacity } )
						{
							using( var writer = XmlWriter.Create( newMetadataStream, transform.OutputSettings ) )
							{
								transform.Transform( reader, writer );
							}

							metadataStream.SetLength( 0 );
							newMetadataStream.Seek( 0, SeekOrigin.Begin );
							newMetadataStream.CopyTo( metadataStream );
						}

						metadataStream.Seek( 0, SeekOrigin.Begin );
					}
				}
				else
				{
					throw new NotSupportedException( "\"header.xml\" is missing. data file isn't supported." );
				}


				var plotpointEntry = zipFile.GetEntry( "plotpoints.dat" );

				using( var inputStream = plotpointEntry?.Open() )
				{
					var settings = new XmlReaderSettings
					{
						IgnoreComments = true,
						IgnoreWhitespace = true,
						IgnoreProcessingInstructions = true,
						CloseInput = false,
						NameTable = new NameTable()
					};
					using( var reader = XmlReader.Create( metadataStream, settings ) )
					{
						reader.MoveToElement();
						while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
						{
							switch( reader.Name )
							{
								case FormplotRootTag:
								case PropertiesRootTag:
									return Deserialize( reader, inputStream );
							}
						}
					}
				}
			}

			throw new NotSupportedException( "\"header.xml\" format is wrong. data file isn't supported." );
		}

		/// <summary>
		/// Reads the points from the specified <see cref="Stream"/> and collects their specifications, such as type, tolerance, segment and metadata.
		/// </summary>
		/// <param name="formplotType">Type of the formplot.</param>
		/// <param name="metadataReader">The metadata reader.</param>
		/// <param name="pointdataStream">The pointdata stream.</param>
		/// <returns></returns>
		private static Point[] ReadPoints( FormplotTypes formplotType, XmlReader metadataReader, Stream pointdataStream )
		{
			if( metadataReader == null )
			{
				throw new ArgumentNullException( nameof( metadataReader ) );
			}

			if( pointdataStream == null )
			{
				throw new ArgumentNullException( nameof( pointdataStream ) );
			}

			var count = 0;
			var propertyLists = new Dictionary<Property, RangeList>();
			var statelists = new Dictionary<PointState, RangeList>();
			var segmentlists = new Dictionary<Segment, RangeList>();
			var tolerancelists = new Dictionary<Tolerance, RangeList>();

			while( metadataReader.Read() && metadataReader.NodeType != XmlNodeType.EndElement )
			{
				if( metadataReader.IsEmptyElement ) continue;

				switch( metadataReader.Name )
				{
					case "Count":
						count = XmlConvert.ToInt32( metadataReader.ReadString() );
						break;
					case "Properties":
						while( metadataReader.Read() && metadataReader.NodeType != XmlNodeType.EndElement )
						{
							switch( metadataReader.Name )
							{
								case "Property":
									var points = RangeList.TryParseOrEmpty( metadataReader.GetAttribute( "Points" ) );
									var property = Property.Deserialize( metadataReader );
									propertyLists[ property ] = points;
									break;
							}
						}
						break;
					case "States":
						while( metadataReader.Read() && metadataReader.NodeType != XmlNodeType.EndElement )
						{
							switch( metadataReader.Name )
							{
								case "State":
									var points = RangeList.TryParseOrEmpty( metadataReader.GetAttribute( "Points" ) );
									var text = metadataReader.ReadString();
									var value = ( PointState ) Enum.Parse( typeof( PointState ), text );
									statelists[ value ] = points;
									break;
							}
						}
						break;
					case "Segments":
						while( metadataReader.Read() && metadataReader.NodeType != XmlNodeType.EndElement )
						{
							switch( metadataReader.Name )
							{
								case "Segment":
									var points = RangeList.TryParseOrEmpty( metadataReader.GetAttribute( "Points" ) );
									var typeString = metadataReader.GetAttribute( "Type" );
									SegmentTypes type;
									Enum.TryParse( typeString, out type );
									var name = metadataReader.ReadString();
									var segment = new Segment( name, type );
									segmentlists[ segment ] = points;
									break;
							}
						}
						break;
					case "Tolerances":
						var isTolerancesElement = false;
						while( metadataReader.NodeType != XmlNodeType.EndElement || !isTolerancesElement )
						{
							if( metadataReader.NodeType == XmlNodeType.Element && metadataReader.Name.Equals( "Tolerance" ) )
							{
								var points = RangeList.TryParseOrEmpty( metadataReader.GetAttribute( "Points" ) );
								var tolerance = Tolerance.Deserialize( metadataReader );
								tolerancelists[ tolerance ] = points;
								metadataReader.Read();
							}
							else
								metadataReader.Read();

							isTolerancesElement = metadataReader.Name.Equals( "Tolerances" );
						}
						break;
				}
			}

			var result = new List<Point>();

			for( var index = 0; index < count; index++ )
			{
				var point = Point.Create( formplotType );
				point.ReadFromStream( pointdataStream, index );

				point.PropertyList = RangeList.GetKeysInRange( propertyLists, index );

				var pointStates = RangeList.GetKeysInRange( statelists, index );

				foreach( var pointState in pointStates )
				{
					point.State |= pointState;
				}

				point.Segment = RangeList.GetKeysInRange( segmentlists, index ).FirstOrDefault();
				point.Tolerance = RangeList.GetKeysInRange( tolerancelists, index ).FirstOrDefault();

				result.Add( point );
			}

			return result.ToArray();
		}
		
		private static bool IsTruncationSafe( byte[] content )
		{
			// check if first few bytes correspond to ascii "<?xml"
			return ( content[ 0 ] == 0x3C ) && ( content[ 1 ] == 0x3F ) && ( content[ 2 ] == 0x78 ) && ( content[ 3 ] == 0x6D ) && ( content[ 4 ] == 0x6C );
		}

		private static int FindEndOfContent( byte[] content )
		{
			int i = content.Length - 1;
			while( i > -1 && content[ i ] == 0 )
				--i;

			return i;
		}

		private static FormplotTypes GetFormplotType( Stream stream )
		{
			var result = FormplotTypes.None;
			var pos = stream.Position;

			var xdoc = XDocument.Load( stream );
			if( xdoc.Root != null && xdoc.Root.Name.LocalName == FormplotRootTag && xdoc.Root.HasAttributes )
			{
				var attr = xdoc.Root.Attribute( XName.Get( "Type" ) );
				if( attr != null )
				{
					if( !Enum.TryParse( attr.Value, out result ) )
						result = FormplotTypes.None;
				}
			}

			// requires CanSeek on stream
			stream.Position = pos;

			return result;
		}

		private static byte[] ReadFully( Stream input )
		{
			var buffer = new byte[16 * 1024];
			using( var ms = new MemoryStream() )
			{
				int read;
				while( ( read = input.Read( buffer, 0, buffer.Length ) ) > 0 )
				{
					ms.Write( buffer, 0, read );
				}
				return ms.ToArray();
			}
		}

		/// <summary>
		/// Deserializes formplot data.
		/// </summary>
		/// <param name="metaDataReader">An XML reader to the header xml.</param>
		/// <param name="pointDataStream">A stream of binary formplot point data.</param>
		/// <returns></returns>
		private static Formplot Deserialize( XmlReader metaDataReader, Stream pointDataStream )
		{
			if( metaDataReader == null )
			{
				throw new ArgumentNullException( nameof( metaDataReader ) );
			}

			var creatorSoftware = "unknown";
			var creatorSoftwareVersion = new Version( 0, 0 );
			var properties = new List<Property>();
			var tolerance = default ( Tolerance );
			var defaultErrorScaling = default ( double? );
			var projectionAxis = ProjectionAxis.None;
			var nominal = default ( Geometry );
			var actual = default ( Geometry );
			var points = default ( Point[] );

			FormplotTypes formplotType;
			Enum.TryParse( metaDataReader.GetAttribute( "Type" ), out formplotType );

			while( metaDataReader.Read() && metaDataReader.NodeType != XmlNodeType.EndElement )
			{
				switch( metaDataReader.Name )
				{
					case "CreatorSoftware":
						creatorSoftware = metaDataReader.ReadString();
						break;
					case "CreatorSoftwareVersion":
						creatorSoftwareVersion = new Version( metaDataReader.ReadString() );
						break;
					case "Property":
						properties.Add( Property.Deserialize( metaDataReader ) );
						break;
					case "Tolerance":
						tolerance = Tolerance.Deserialize( metaDataReader );
						break;
					case "ErrorScaling":
						defaultErrorScaling = XmlConvert.ToDouble( metaDataReader.ReadString() );
						break;
					case "ProjectionAxis":
						if( formplotType == FormplotTypes.Straightness )
							projectionAxis = ( ProjectionAxis ) Enum.Parse( typeof( ProjectionAxis ), metaDataReader.ReadString() );
						else
							throw new NotSupportedException( "only straightness formplot data supports a projection axis" );
						break;
					case "Geometry":
						var geometryString = metaDataReader.GetAttribute( "Type" );
						GeometryTypes geometryType;
						if( EnumParser<GeometryTypes>.TryParse( geometryString, out geometryType ) )
						{
							if( geometryType != Geometry.GetGeometryTypeFromFormplotType( formplotType ) )
							{
								throw new NotSupportedException( $"geometry type \"{geometryType}\" for plot type \"{formplotType}\" is not supported" );
							}

							while( metaDataReader.Read() && metaDataReader.NodeType != XmlNodeType.EndElement )
							{
								switch( metaDataReader.Name )
								{
									case "Nominal":
										nominal = Geometry.DeserializeGeometry( metaDataReader, geometryType );
										break;
									case "Actual":
										actual = Geometry.DeserializeGeometry( metaDataReader, geometryType );
										break;
								}
							}
						}
						else
						{
							throw new NotSupportedException( $"geometry type \"{geometryString}\" is not supported" );
						}

						break;
					case "Points":
						points = ReadPoints( formplotType, metaDataReader, pointDataStream );
						break;
				}
			}

			var plot = CreateFormplot( formplotType );

			plot.CreatorSoftware = creatorSoftware;
			plot.CreatorSoftwareVersion = creatorSoftwareVersion;
			plot.Properties = properties.ToArray();
			plot.Tolerance = tolerance;
			plot.DefaultErrorScaling = defaultErrorScaling;
			plot.ProjectionAxis = projectionAxis;
			plot.Nominal = nominal;
			plot.Actual = actual;
			plot.Points = points;

			return plot;
		}

		/// <summary>
		/// Creates a formplot with the specified type.
		/// </summary>
		/// <param name="type">The formplot type.</param>
		/// <returns></returns>
		private static Formplot CreateFormplot( FormplotTypes type )
		{
			switch( type )
			{
				case FormplotTypes.None:
					return new EmptyPlot();
				case FormplotTypes.Roundness:
					return new RoundnessPlot();
				case FormplotTypes.Flatness:
					return new FlatnessPlot();
				case FormplotTypes.CurveProfile:
					return new CurveProfilePlot();
				case FormplotTypes.Straightness:
					return new StraightnessPlot();
				case FormplotTypes.Cylindricity:
					return new CylindricityPlot();
				case FormplotTypes.Pitch:
					return new PitchPlot();
				case FormplotTypes.BorePattern:
					return new BorePatternPlot();
				case FormplotTypes.CircleInProfile:
					return new CircleInProfilePlot();
				case FormplotTypes.Fourier:
					return new FourierPlot();
				default:
					throw new ArgumentOutOfRangeException( nameof( type ), type, null );
			}
		}

		#endregion
	}
}