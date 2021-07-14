#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.IO;

	#endregion

	/// <summary>
	/// An item of the defect plot.
	/// </summary>
	public sealed class Defect : Point<Defect, DefectGeometry>
	{
		#region constructors

		/// <inheritdoc />
		public Defect() { }

		/// <summary>Constructor.</summary>
		/// <param name="position">The position.</param>
		/// <param name="size">The size.</param>
		/// <param name="voxels">The voxels.</param>
		public Defect( Vector position, Vector size, Voxel[] voxels )
		{
			Position = position;
			Size = size;
			Voxels = voxels;
		}

		/// <summary>Constructor.</summary>
		/// <param name="position">The position.</param>
		/// <param name="size">The size.</param>
		/// <param name="shape">The mesh that describes the defects shape.</param>
		public Defect( Vector position, Vector size, Mesh shape )
		{
			Position = position;
			Size = size;
			Shape = shape;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Vector Position { get; private set; }

		/// <summary>
		/// Gets the size.
		/// </summary>
		public Vector Size { get; private set; }

		/// <summary>
		/// Gets the voxel shape.
		/// </summary>
		public Voxel[]? Voxels { get; private set; }

		/// <summary>
		/// Gets the mesh shape.
		/// </summary>
		public Mesh? Shape { get; private set; }

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader, Version version )
		{
			Position = new Vector( reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble() );
			Size = new Vector( reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble() );

			ReadVoxels( reader );

			if( version < Formplot.Version3 )
				return;

			var shape = Mesh.Read( reader );
			if( shape.Indices.Length > 0 )
				Shape = shape;
		}

		private void ReadVoxels( BinaryReader reader )
		{
			var length = reader.ReadInt32();
			if( length == 0 )
				return;

			Voxels = new Voxel[ length ];

			for( var i = 0; i < length; i++ )
			{
				Voxels[ i ] = Voxel.Read( reader );
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

			WriteVoxels( writer );
			WriteMesh( writer );
		}

		private void WriteVoxels( BinaryWriter writer )
		{
			if( Voxels == null || Voxels.Length == 0 )
			{
				writer.Write( 0 );
				return;
			}

			writer.Write( Voxels.Length );
			foreach( var voxel in Voxels )
			{
				voxel.Write( writer );
				voxel.Check( this );
			}
		}

		private void WriteMesh( BinaryWriter writer )
		{
			if( Shape.HasValue )
			{
				Shape.Value.Write( writer );
				Shape.Value.Check( this );
			}
			else
			{
				Mesh.Empty.Write( writer );
			}
		}

		#endregion
	}
}