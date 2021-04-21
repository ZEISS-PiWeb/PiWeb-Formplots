#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Xml;

	#endregion

	/// <summary>
	/// Represents a line geometry
	/// </summary>
	public sealed class LineGeometry : Geometry
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LineGeometry"/> class.
		/// </summary>
		public LineGeometry()
		{
			Position = new Vector();
			Direction = new Vector( 1, 0, 0 );
			Deviation = new Vector( 0, 1, 0 );
			Length = 1.0;
		}

		#endregion

		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.Line;

		/// <summary>
		/// Gets the position vector where the line starts.
		/// </summary>
		public Vector Position { get; set; }

		/// <summary>
		/// Gets the direction vector.
		/// </summary>
		public Vector Direction { get; set; }

		/// <summary>
		/// Gets the direction vector of the deviation. (Normal direction, probing direction)
		/// </summary>
		public Vector Deviation { get; set; }

		/// <summary>
		/// Gets the length of the line in millimeters.
		/// </summary>
		public double Length { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException">writer</exception>
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			writer.WriteStartElement( "Position" );
			Position.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteStartElement( "Direction" );
			Direction.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteStartElement( "Deviation" );
			Deviation.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteElementString( "Length", XmlConvert.ToString( Length ) );
		}

		/// <summary>
		/// Reads the geometry information from the specified <see cref="XmlReader" />.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="version">The version of the formplot file.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		protected override bool DeserializeItem( XmlReader reader, Version version )
		{
			//Compatibility with first version of calypso straightness plot, which switched Axis1 and Axis3
			//Was handled with xsl until PiWeb 7.4

			if( reader.Name == "ElementSystem" )
			{
				CoordinateSystem = CoordinateSystem.Deserialize( reader );

				var axis1 = CoordinateSystem.Axis3;
				CoordinateSystem.Axis3 = CoordinateSystem.Axis1;
				CoordinateSystem.Axis1 = axis1;

				return true;
			}

			if( base.DeserializeItem( reader, version ) )
				return true;

			switch( reader.Name )
			{
				case "Position":
					Position = Vector.Deserialize( reader );
					return true;
				case "Direction":
					Direction = Vector.Deserialize( reader );
					return true;
				case "Deviation":
					Deviation = Vector.Deserialize( reader );
					return true;
				case "Length":
					Length = XmlConvert.ToDouble( reader.ReadString() );

					//Compatibility with first version of calypso straightness plot
					//Was handled with xsl until PiWeb 7.4

					if( version == new Version( 1, 0, 0 ) )
					{
						Position = new Vector();
						Direction = new Vector( 1, 0, 0 );
						Deviation = new Vector( 0, 0, 1 );
					}

					return true;
				default:
					return false;
			}
		}

		#endregion
	}
}