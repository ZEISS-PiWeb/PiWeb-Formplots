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
	/// Representation of a cylinder geometry.
	/// </summary>
	public class CylinderGeometry : Geometry
	{
		#region constructors
		
		/// <summary>
		/// Initializes a new instance of the <see cref="CylinderGeometry"/> class.
		/// </summary>
		public CylinderGeometry()
		{
			Radius = 1.0;
			Height = 1.0;
		}

		#endregion

		#region properties
		
		/// <summary>
		/// Gets or sets the radius.
		/// </summary>
		public double Radius { get; set; }
		
		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public double Height { get; set; }

		#endregion

		#region overrides
		
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

			writer.WriteElementString( "Radius", XmlConvert.ToString( Radius ) );
			writer.WriteElementString( "Height", XmlConvert.ToString( Height ) );
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
					case "Radius":
						Radius = XmlConvert.ToDouble( reader.ReadString() );
						break;
					case "Height":
						Height = XmlConvert.ToDouble( reader.ReadString() );
						break;
				}
			}

			CoordinateSystem = elementSystem;
		}

		#endregion
	}
}
