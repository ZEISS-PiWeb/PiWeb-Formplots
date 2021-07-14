#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	using System;
	using System.IO;

	/// <summary>
	/// Describes a cuboid with a position and a size.
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

		/// <summary>
		/// Reads a voxel from the specified <paramref name="reader" />.
		/// </summary>
		internal static Voxel Read( BinaryReader reader )
		{
			var px = reader.ReadDouble();
			var py = reader.ReadDouble();
			var pz = reader.ReadDouble();

			var sx = reader.ReadDouble();
			var sy = reader.ReadDouble();
			var sz = reader.ReadDouble();

			return new Voxel( new Vector( px, py, pz ), new Vector( sx, sy, sz ) );
		}

		/// <summary>
		/// Writes the voxel to the specified <paramref name="writer" />.
		/// </summary>
		internal void Write( BinaryWriter writer )
		{
			writer.Write( Position.X );
			writer.Write( Position.Y );
			writer.Write( Position.Z );

			writer.Write( Size.X );
			writer.Write( Size.Y );
			writer.Write( Size.Z );
		}

		/// <summary>
		/// Checks, whether the voxel is completely within the bounds of the <paramref name="defect" />.
		/// </summary>
		internal void Check( Defect defect )
		{
			if( Position.X < defect.Position.X ||
				Position.Y < defect.Position.Y ||
				Position.Z < defect.Position.Z ||
				Position.X + Size.X > defect.Position.X + defect.Size.X ||
				Position.Y + Size.Y > defect.Position.Y + defect.Size.Y ||
				Position.Z + Size.Z > defect.Position.Z + defect.Size.Z )
				throw new FormatException( "The voxels of a defect must lie within the bounds of the defect." );
		}
	}
}