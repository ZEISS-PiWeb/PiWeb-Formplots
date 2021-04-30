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
	using System.Linq;

	/// <summary>
	/// Defect Voxel (Pixel).
	/// </summary>
	public readonly struct Mesh
	{
		/// <summary>Constructor.</summary>
		/// <param name="indices">The mesh indices.</param>
		/// <param name="vertices">The mesh vertices in form [x,y,z,x,y,z,...]</param>
		public Mesh( int[] indices, float[] vertices )
		{
			Indices = indices;
			Vertices = vertices;
		}

		/// <summary>
		/// Gets or sets the indices of the mesh shape.
		/// </summary>
		public int[] Indices { get; }

		/// <summary>
		/// Gets or sets the vertices of the mesh shape.
		/// </summary>
		public float[] Vertices { get; }

		/// <summary>
		/// Reads a mesh from the specified <paramref name="reader"/>
		/// </summary>
		internal static Mesh? Read( BinaryReader reader )
		{
			var indiceCount = reader.ReadInt32();
			if( indiceCount == 0 )
				return null;

			var indices = new int[ indiceCount ];

			for( var i = 0; i < indiceCount; i++ )
				indices[ i ] = reader.ReadInt32();

			var verticeCount = reader.ReadInt32();
			if( verticeCount == 0 )
				return null;

			var vertices = new float[ verticeCount ];

			for( var i = 0; i < verticeCount; i++ )
				vertices[ i ] = reader.ReadSingle();

			return new Mesh( indices, vertices );
		}

		/// <summary>
		/// Writes the voxel to the specified <paramref name="writer"/>.
		/// </summary>
		internal void Write( BinaryWriter writer )
		{
			writer.Write( Indices.Length );

			foreach( var index in Indices )
				writer.Write( index );

			writer.Write( Vertices.Length );

			foreach( var vertex in Vertices )
				writer.Write( vertex );
		}

		/// <summary>
		/// Checks the mesh for consistency.
		/// </summary>
		/// <exception cref="FormatException">The mesh had inconsistencies</exception>
		internal void Check( Defect defect )
		{
			if( Indices.Length % 3 != 0 )
				throw new FormatException( "The number of indices in a mesh must be a multiple of three." );

			if( Vertices.Length % 3 != 0 )
				throw new FormatException( "The number of vertices in a mesh must be a multiple of three." );

			if( Indices.Max() + 1 > Vertices.Length / 3 )
				throw new FormatException( "The highest index in a mesh must not exceed the triple number of vertices." );

			var xmin = defect.Position.X;
			var xmax = defect.Position.X + defect.Size.X;
			var ymin = defect.Position.Y;
			var ymax = defect.Position.Y + defect.Size.Y;
			var zmin = defect.Position.Z;
			var zmax = defect.Position.Z + defect.Size.Z;

			for( var i = 0; i < Vertices.Length / 3; i += 3 )
			{
				var x = Vertices[ i * 3 ];
				var y = Vertices[ i * 3 + 1 ];
				var z = Vertices[ i * 3 + 2 ];

				if( x < xmin || x > xmax || y < ymin || y > ymax || z < zmin || z > zmax )
					throw new FormatException( "The vertices of a mesh must lie within the bounds of the defect." );
			}
		}
	}
}