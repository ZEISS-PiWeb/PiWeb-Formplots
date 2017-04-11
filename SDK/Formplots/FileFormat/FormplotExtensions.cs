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

	#endregion

	/// <summary>
	/// Extension class to provide serialization of formplots.
	/// </summary>
	internal static class FormplotExtensions
	{
		#region methods

		/// <summary>
		/// Writes the data of the plot into the specified <paramref name="stream"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		internal static void WriteTo( this Formplot plot, Stream stream )
		{
			if( stream == null )
				throw new ArgumentNullException( nameof( stream ) );

			using( var zipOutput = new ZipArchive( stream, ZipArchiveMode.Create ) )
			{
				var versionInfoEntry = zipOutput.CreateEntry( "fileversion.txt", CompressionLevel.NoCompression );
				versionInfoEntry.LastWriteTime = new DateTime( 1980, 1, 1 );

				using( var textWriter = new StreamWriter( versionInfoEntry.Open(), Encoding.UTF8 ) )
				{
					textWriter.Write( FormplotHelpers.GetFileFormatVersion( plot.FormplotType ).ToString( 2 ) );
				}

				using( var metadataStream = new MemoryStream() )
				{
					var settings = new XmlWriterSettings
					{
						Indent = true,
						Encoding = Encoding.UTF8,
						CloseOutput = false
					};

					using( var writer = XmlWriter.Create( metadataStream, settings ) )
					{
						Stream pointDataStream;

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
						writer.WriteStartElement( plot.FormplotType == FormplotTypes.None ? FormplotHelpers.PropertiesRootTag : FormplotHelpers.FormplotRootTag );

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

		/// <summary>
		/// Serializes the formplot. The header information is written to the <paramref name="metaDataWriter" />, whereas the points are stored as a
		/// blob in the <paramref name="pointDataStream" /></summary>
		/// <param name="plot">The plot.</param>
		/// <param name="metaDataWriter">An XML writer to store the header data, such as tolerances and segments.</param>
		/// <param name="pointDataStream">A stream to store the binary point data.</param>
		private static void Serialize( this Formplot plot, XmlWriter metaDataWriter, Stream pointDataStream )
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

			if( plot.FormplotType != FormplotTypes.None )
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

				if( plot.FormplotType == FormplotTypes.Straightness && plot.ProjectionAxis != ProjectionAxis.None )
				{
					metaDataWriter.WriteStartElement( "ProjectionAxis" );
					metaDataWriter.WriteValue( plot.ProjectionAxis.ToString() );
					metaDataWriter.WriteEndElement();
				}

				if( plot.GeometryType != GeometryTypes.None )
				{
					metaDataWriter.WriteStartElement( "Geometry" );

					{
						metaDataWriter.WriteAttributeString( "Type", plot.GeometryType.ToString() );

						metaDataWriter.WriteStartElement( "Nominal" );
						plot.Nominal.Serialize( metaDataWriter );
						metaDataWriter.WriteEndElement();

						metaDataWriter.WriteStartElement( "Actual" );
						plot.Actual.Serialize( metaDataWriter );
						metaDataWriter.WriteEndElement();
					}

					metaDataWriter.WriteEndElement();
				}

				metaDataWriter.WriteStartElement( "Points" );
				plot.WritePoints( metaDataWriter, pointDataStream );
				metaDataWriter.WriteEndElement();
			}
		}

		/// <summary>
		/// Writes the points into the specified <see cref="Stream" /> and writes their metadata with the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="plot">The plot.</param>
		/// <param name="writer">The xml writer.</param>
		/// <param name="pointdataStream">The point data stream.</param>
		private static void WritePoints( this Formplot plot, XmlWriter writer, Stream pointdataStream )
		{
			if( writer == null )
				throw new ArgumentNullException( nameof( writer ) );

			if( pointdataStream == null )
				throw new ArgumentNullException( nameof( pointdataStream ) );

			writer.WriteStartElement( "Count" );
			writer.WriteValue( XmlConvert.ToString( plot.Points.Count() ) );
			writer.WriteEndElement();

			var propertyLists = new Dictionary<Property, RangeList>();
			var statelists = new Dictionary<PointState, RangeList>();
			var segmentlists = new Dictionary<Segment, RangeList>();
			var tolerancelists = new Dictionary<Tolerance, RangeList>();

			var lastPoint = default( Point );
			var index = 0;

			foreach( var point in plot.Points )
			{
				CollectMetaData( point, propertyLists, index, lastPoint );
				CollectStates( point, statelists, index, lastPoint );
				CollectSegments( point, segmentlists, index, lastPoint );
				CollectTolerances( point, tolerancelists, index, lastPoint );
				point.WriteToStream( pointdataStream );

				lastPoint = point;
				index++;
			}

			WriteMetaData( writer, propertyLists );
			WriteStates( writer, statelists );
			WriteSegments( writer, segmentlists );
			WriteTolerances( writer, tolerancelists );
		}

		/// <summary>
		/// Collects the tolerances.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="tolerancelists">The tolerancelists.</param>
		/// <param name="index">The index.</param>
		/// <param name="lastPoint">The last point.</param>
		private static void CollectTolerances( Point point, IDictionary<Tolerance, RangeList> tolerancelists, int index, Point lastPoint )
		{
			if( !point.Tolerance.IsEmpty )
			{
				if( !tolerancelists.ContainsKey( point.Tolerance ) )
					tolerancelists.Add( point.Tolerance, new RangeList { new Range( index ) } );
				else
				{
					if( lastPoint != null )
					{
						if( Tolerance.Equals( point.Tolerance, lastPoint.Tolerance ) )
							tolerancelists[ point.Tolerance ].Last().End = index;
						else
							tolerancelists[ point.Tolerance ].Add( new Range( index ) );
					}
				}
			}
		}

		/// <summary>
		/// Collects the segments.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="segmentlists">The segmentlists.</param>
		/// <param name="index">The index.</param>
		/// <param name="lastPoint">The last point.</param>
		private static void CollectSegments( Point point, IDictionary<Segment, RangeList> segmentlists, int index, Point lastPoint )
		{
			if( point.Segment != null )
			{
				if( !segmentlists.ContainsKey( point.Segment ) )
					segmentlists.Add( point.Segment, new RangeList { new Range( index ) } );
				else
				{
					if( lastPoint != null )
					{
						if( Equals( point.Segment, lastPoint.Segment ) )
							segmentlists[ point.Segment ].Last().End = index;
						else
							segmentlists[ point.Segment ].Add( new Range( index ) );
					}
				}
			}
		}

		/// <summary>
		/// Collects the states.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="statelists">The statelists.</param>
		/// <param name="index">The index.</param>
		/// <param name="lastPoint">The last point.</param>
		private static void CollectStates( Point point, IDictionary<PointState, RangeList> statelists, int index, Point lastPoint )
		{
			if( point.State != PointState.None )
			{
				if( !statelists.ContainsKey( point.State ) )
					statelists.Add( point.State, new RangeList { new Range( index ) } );
				else
				{
					if( lastPoint != null )
					{
						if( point.State == lastPoint.State )
							statelists[ point.State ].Last().End = index;
						else
							statelists[ point.State ].Add( new Range( index ) );
					}
				}
			}
		}

		/// <summary>
		/// Collects the meta data.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="propertyLists">The property lists.</param>
		/// <param name="index">The index.</param>
		/// <param name="lastPoint">The last point.</param>
		private static void CollectMetaData( Point point, IDictionary<Property, RangeList> propertyLists, int index, Point lastPoint )
		{
			foreach( var property in point.PropertyList )
			{
				if( !propertyLists.ContainsKey( property ) )
					propertyLists.Add( property, new RangeList { new Range( index ) } );
				else
				{
					if( lastPoint != null )
					{
						if( lastPoint.PropertyList.Contains( property ) )
							propertyLists[ property ].Last().End = index;
						else
							propertyLists[ property ].Add( new Range( index ) );
					}
				}
			}
		}

		/// <summary>
		/// Writes the tolerance information with the specified <see cref="XmlWriter"/>.
		/// </summary>
		/// <param name="writer">The xml writer.</param>
		/// <param name="tolerancelists">The tolerances.</param>
		private static void WriteTolerances( XmlWriter writer, IReadOnlyCollection<KeyValuePair<Tolerance, RangeList>> tolerancelists )
		{
			if( tolerancelists.Count > 0 )
			{
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
		}

		/// <summary>
		/// Writes the segment information with the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The xml writer.</param>
		/// <param name="segmentlists">The segmentlists.</param>
		private static void WriteSegments( XmlWriter writer, IReadOnlyCollection<KeyValuePair<Segment, RangeList>> segmentlists )
		{
			if( segmentlists.Count > 0 )
			{
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
		}

		/// <summary>
		/// Writes the state information with the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The xml writer.</param>
		/// <param name="statelists">The statelists.</param>
		private static void WriteStates( XmlWriter writer, IReadOnlyCollection<KeyValuePair<PointState, RangeList>> statelists )
		{
			if( statelists.Count > 0 )
			{
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
		}

		/// <summary>
		/// Writes the meta data with the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The xml writer.</param>
		/// <param name="propertyLists">The property lists.</param>
		private static void WriteMetaData( XmlWriter writer, IReadOnlyCollection<KeyValuePair<Property, RangeList>> propertyLists )
		{
			if( propertyLists.Count > 0 )
			{
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
		}

		#endregion
	}
}