#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Xml;

	#endregion

	/// <summary>
	/// Represents a line geometry
	/// </summary>
	public class LineGeometry : Geometry
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LineGeometry"/> class.
		/// </summary>
		public LineGeometry()
		{
			Position = new Vector();
			Direction = new Vector( 1.0 );
			Deviation = new Vector( 0.0, 1.0 );
			Length = 1.0;
		}

		#endregion

		#region properties

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
		public override void Serialize( XmlWriter writer )
		{
			if( writer == null )
			{
				throw new ArgumentNullException( nameof( writer ) );
			}

			writer.WriteStartElement( "CoordinateSystem" );
			CoordinateSystem.Serialize( writer );
			writer.WriteEndElement();

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
		protected override void Deserialize( XmlReader reader )
		{
			while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
			{
				switch( reader.Name )
				{
					case "CoordinateSystem":
						CoordinateSystem = CoordinateSystem.Deserialize( reader );
						break;
					case "Position":
						Position = Vector.Deserialize( reader );
						break;
					case "Direction":
						Direction = Vector.Deserialize( reader );
						break;
					case "Deviation":
						Deviation = Vector.Deserialize( reader );
						break;
					case "Length":
						Length = XmlConvert.ToDouble( reader.ReadString() );
						break;
				}
			}
		}

		#endregion
	}
}