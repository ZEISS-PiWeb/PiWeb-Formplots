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
	using System.Globalization;
	using System.Xml;

	#endregion

	/// <summary>
	/// Contains the curve entry- and exit points as well as the gap points of two curves.
	/// </summary>
	/// <seealso cref="Zeiss.IMT.PiWeb.Formplot.FileFormat.CurveGeometry" />
	public class FlushGapGeometry : CurveGeometry
	{
		#region properties

		/// <summary>
		/// Get or set the reference profile for geometry of Flush- and Gapplot
		/// </summary>
		public FlushGapProfile ReferenceProfile { get; private set; } = new FlushGapProfile();

		/// <summary>
		/// Get or set the measure profile  for geometry of Flush- and Gapplot
		/// </summary>
		public FlushGapProfile MeasureProfile { get; private set; } = new FlushGapProfile();

		/// <summary>
		/// Get or set the value for the Flush of Flush- and Gapplot
		/// </summary>
		public double FlushValue { get; set; }

		/// <summary>
		/// Get or set the type of connection between the Flush points of reference and measure profile
		/// </summary>
		public FlushPointConnectionType FlushConnectionType { get; set; } = FlushPointConnectionType.Orthogonal;

		/// <summary>
		/// Get or set the value for the Gap of Flush- and Gapplot
		/// </summary>
		public double GapValue { get; set; }

		/// <summary>
		/// Get or set the type of connection between the Gap points of reference and measure profile
		/// </summary>
		public FlushPointConnectionType GapConnectionType { get; set; } = FlushPointConnectionType.Orthogonal;

		#endregion

		#region methods



		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException">writer</exception>
		public override void Serialize( XmlWriter writer )
		{
			if( writer == null )
			{
				throw new ArgumentNullException( nameof( writer ) );
			}

			writer.WriteStartElement( "CoordinateSystem" );
			CoordinateSystem.Serialize( writer );
			writer.WriteEndElement();

			if( ReferenceProfile != null )
			{
				writer.WriteStartElement( "ReferenceProfile" );
				ReferenceProfile.Serialize( writer );
				writer.WriteEndElement();
			}

			if( MeasureProfile != null )
			{
				writer.WriteStartElement( "MeasureProfile" );
				MeasureProfile.Serialize( writer );
				writer.WriteEndElement();
			}

			writer.WriteStartElement( "Flush" );
			writer.WriteAttributeString( "ConnectionType", FlushConnectionType.ToString() );
			writer.WriteString( XmlConvert.ToString( FlushValue ) );
			writer.WriteEndElement();

			writer.WriteStartElement( "Gap" );
			writer.WriteAttributeString( "ConnectionType", GapConnectionType.ToString() );
			writer.WriteString( XmlConvert.ToString( GapValue ) );
			writer.WriteEndElement();
			
		}

		/// <summary>
		/// Reads the geometry information from the specified <see cref="XmlReader" />.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected override void Deserialize( XmlReader reader )
		{
			var elementSystem = new CoordinateSystem();

			while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
			{
				switch( reader.Name )
				{
					case "CoordinateSystem":
						elementSystem = CoordinateSystem.Deserialize( reader );
						break;
					case "ReferenceProfile":
						ReferenceProfile = FlushGapProfile.Deserialize( reader );
						break;
					case "MeasureProfile":
						MeasureProfile = FlushGapProfile.Deserialize( reader );
						break;
					case "Flush":
						{
							if( Enum.TryParse<FlushPointConnectionType>( reader.GetAttribute( "ConnectionType" ), out var connectionType ) )
								FlushConnectionType = connectionType;
							
							FlushValue = Property.ObjectToNullableDouble( reader.ReadString(), CultureInfo.InvariantCulture ) ?? 0.0;

							break;
						}
					case "Gap":
						{
							if( Enum.TryParse<FlushPointConnectionType>( reader.GetAttribute( "ConnectionType" ), out var connectionType ) )
								GapConnectionType = connectionType;

							GapValue = Property.ObjectToNullableDouble( reader.ReadString(), CultureInfo.InvariantCulture ) ?? 0.0;

							break;
						}
				}
			}

			CoordinateSystem = elementSystem;
		}

		#endregion
	}
}