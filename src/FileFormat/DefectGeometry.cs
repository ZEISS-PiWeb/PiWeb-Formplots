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
	/// Representation of a defect geometry.
	/// </summary>
	public class DefectGeometry : Geometry
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DefectGeometry"/> class.
		/// </summary>
		public DefectGeometry()
		{
			Size = new Vector( 1.0, 1.0, 1.0 );
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the reference size. You should either specify normalized defect- and voxel positions, or set the reference size.
		/// </summary>
		public Vector Size { get; set; }

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
				throw new ArgumentNullException( nameof(writer) );
			}

			writer.WriteStartElement( "CoordinateSystem" );
			CoordinateSystem.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteStartElement( "Size" );
			Size.Serialize( writer );
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
					case "Size":
						Size = Vector.Deserialize( reader );
						break;
				}
			}

			CoordinateSystem = elementSystem;
		}

		#endregion
	}
}