#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
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
	/// Represents a defect geometry.
	/// </summary>
	public sealed class DefectGeometry : Geometry
	{
		#region constructors

		/// <summary>Constructor.</summary>
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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