#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System.IO;

	#endregion

	/// <summary>
	/// An item of the defect plot.
	/// </summary>
	public sealed class Defect : Point<Defect, DefectGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public Defect() { }

		/// <summary>Constructor.</summary>
		/// <param name="position">The position.</param>
		/// <param name="size">The size.</param>
		/// <param name="voxels"></param>
		public Defect( Vector position, Vector size, Voxel[] voxels )
		{
			Position = position;
			Size = size;
			Voxels = voxels;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		public Vector Position { get; set; }

		/// <summary>
		/// Gets or sets the size.
		/// </summary>
		/// <value>
		/// The size.
		/// </value>
		public Vector Size { get; set; }

		/// <summary>
		/// Gets or sets the shape.
		/// </summary>
		public Voxel[]? Voxels { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader )
		{
			Position = new Vector( reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble() );
			Size = new Vector( reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble() );

			var length = reader.ReadInt32();
			Voxels = new Voxel[ length ];

			for( var i = 0; i < length; i++ )
			{
				var px = reader.ReadDouble();
				var py = reader.ReadDouble();
				var pz = reader.ReadDouble();

				var sx = reader.ReadDouble();
				var sy = reader.ReadDouble();
				var sz = reader.ReadDouble();

				Voxels[ i ] = new Voxel( new Vector( px, py, pz ), new Vector( sx, sy, sz ) );
			}
		}

		/// <inheritdoc />
		internal override void WriteToStream( BinaryWriter writer )
		{
			writer.Write( Position.X );
			writer.Write( Position.Y );
			writer.Write( Position.Z );

			writer.Write( Size.X );
			writer.Write( Size.Y );
			writer.Write( Size.Z );

			if( Voxels == null || Voxels.Length == 0 )
			{
				writer.Write( 0 );
				return;
			}

			writer.Write( Voxels.Length );
			foreach( var voxel in Voxels )
			{
				writer.Write( voxel.Position.X );
				writer.Write( voxel.Position.Y );
				writer.Write( voxel.Position.Z );

				writer.Write( voxel.Size.X );
				writer.Write( voxel.Size.Y );
				writer.Write( voxel.Size.Z );
			}
		}

		#endregion
	}

	/// <summary>
	/// Defect Voxel (Pixel).
	/// </summary>
	public readonly struct Voxel
	{
		/// <summary>Constructor.</summary>
		/// <param name="position">The position.</param>
		/// <param name="size">The size.</param>
		public Voxel( Vector position, Vector size )
		{
			Position = position;
			Size = size;
		}

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		public Vector Position { get; }

		/// <summary>
		/// Gets or sets the size.
		/// </summary>
		/// <value>
		/// The size.
		/// </value>
		public Vector Size { get; }
	}
}