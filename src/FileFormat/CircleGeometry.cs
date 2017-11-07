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
	using System.Globalization;
	using System.Xml;

	#endregion

	/// <summary>
	/// Describes a circle geometry
	/// </summary>
	public class CircleGeometry : Geometry
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CircleGeometry"/> class.
		/// </summary>
		public CircleGeometry()
		{
			Radius = 1.0;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the radius.
		/// </summary>
		public double Radius { get; set; }
		
		#endregion

		#region methods

		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public override void Serialize( XmlWriter writer )
		{
			if( writer == null )
			{
				throw new ArgumentNullException( nameof( writer ) );
			}

			writer.WriteStartElement( "CoordinateSystem" );
			CoordinateSystem.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteElementString( "Radius", XmlConvert.ToString( Radius ) );
		}

		/// <summary>
		/// Reads the geometry information from the specified <see cref="XmlReader" />.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		protected override void Deserialize( XmlReader reader )
		{
			if( reader == null )
			{
				throw new ArgumentNullException( nameof( reader ) );
			}

			while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
			{
				switch( reader.Name )
				{
					case "CoordinateSystem":
						CoordinateSystem = CoordinateSystem.Deserialize( reader );
						break;
					case "Radius":
						Radius = XmlConvert.ToDouble( reader.ReadString() );
						break;
				}
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "Radius={0}, CoordinateSystem={{{1}}}", Radius, CoordinateSystem );
		}

		#endregion
	}
}