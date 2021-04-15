#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Xml;

	#endregion

	/// <summary>
	/// Representation of a defect geometry.
	/// </summary>
	public sealed class DefectGeometry : Geometry
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DefectGeometry"/> class.
		/// </summary>
		public DefectGeometry()
		{
			Size = new Vector( 1.0, 1.0, 1.0 );
			VoxelSize = new Vector( 1.0, 1.0, 1.0 );
		}

		#endregion

		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.Defect;

		/// <summary>
		/// Gets or sets the reference size.
		/// </summary>
		public Vector Size { get; set; }

		/// <summary>
		/// Gets or sets the size of a single voxel.
		/// </summary>
		public Vector VoxelSize { get; set; }

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

			writer.WriteStartElement( "Size" );
			Size.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteStartElement( "VoxelSize" );
			VoxelSize.Serialize( writer );
			writer.WriteEndElement();
		}

		/// <summary>
		/// Reads the geometry information from the specified <see cref="XmlReader" />.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="version">The version of the formplot file.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		protected override bool DeserializeItem( XmlReader reader, Version version )
		{
			if( base.DeserializeItem( reader, version ) )
				return true;

			switch( reader.Name )
			{
				case "Size":
					Size = Vector.Deserialize( reader );
					return true;
				case "VoxelSize":
					VoxelSize = Vector.Deserialize( reader );
					return true;
			}

			return false;
		}

		#endregion
	}
}