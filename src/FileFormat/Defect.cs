#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.IO;

	#endregion

	/// <summary>
	/// An item of the defect plot
	/// </summary>
	/// <seealso cref="Zeiss.IMT.PiWeb.Formplot.FileFormat.Point" />
	public class Defect : Point
	{
		#region constructors

		internal Defect()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Defect" /> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="position">The position.</param>
		/// <param name="size">The size.</param>
		public Defect( Segment segment, Vector position, Vector size ) : base( segment )
		{
			Position = position;
			Size = size;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the index.
		/// </summary>
		/// <value>
		/// The index.
		/// </value>
		public int Index { get; private set; }

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
		public Voxel[] Voxels { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Reads from stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="index">The index.</param>
		internal override void ReadFromStream( Stream stream, int index )
		{
			var buffer = GetBuffer( stream, 6 * sizeof( double ) + 1 * sizeof(Int32) );

			var posX = BitConverter.ToDouble( buffer, 0 * sizeof( double ) );
			var posY = BitConverter.ToDouble( buffer, 1 * sizeof( double ) );
			var posZ = BitConverter.ToDouble( buffer, 2 * sizeof( double ) );
			var sizeX = BitConverter.ToDouble( buffer, 3 * sizeof( double ) );
			var sizeY = BitConverter.ToDouble( buffer, 4 * sizeof( double ) );
			var sizeZ = BitConverter.ToDouble( buffer, 5 * sizeof( double ) );
			var length = BitConverter.ToInt32(buffer, 6 * sizeof(double));

			var pixelData = GetBuffer(stream, length * 6 * sizeof(double));
			var voxels = new Voxel[ length ];

			for( var i = 0; i < length; i++ )
			{
				
				var px = BitConverter.ToDouble(pixelData, (0 + i * 6) * sizeof(double));
				var py = BitConverter.ToDouble(pixelData, (1 + i * 6) * sizeof(double));
				var pz = BitConverter.ToDouble(pixelData, (2 + i * 6) * sizeof(double));

				var sx = BitConverter.ToDouble(pixelData, (3 + i * 6) * sizeof(double));
				var sy = BitConverter.ToDouble(pixelData, (4 + i * 6) * sizeof(double));
				var sz = BitConverter.ToDouble(pixelData, (5 + i * 6) * sizeof(double));

				voxels[ i ] = new Voxel( new Vector( px, py, pz ), new Vector( sx, sy, sz ) );
			}

			Voxels = voxels;
			Index = index;
			Position = new Vector { X = posX, Y = posY, Z = posZ };
			Size = new Vector { X = sizeX, Y = sizeY, Z = sizeZ };
		}

		/// <summary>
		/// Writes to stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		internal override void WriteToStream( Stream stream )
		{
			byte[] buffer = BitConverter.GetBytes( Position.X );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Position.Y );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Position.Z );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Size.X );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Size.Y );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Size.Z );
			stream.Write( buffer, 0, buffer.Length );

			if( Voxels == null || Voxels.Length == 0)
			{
				buffer = BitConverter.GetBytes(0);
				stream.Write(buffer, 0, buffer.Length);

				return;
			}

			buffer = BitConverter.GetBytes(Voxels.Length);
			stream.Write(buffer, 0, buffer.Length);

			foreach( var voxel in Voxels)
			{
				buffer = BitConverter.GetBytes(voxel.Position.X);
				stream.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(voxel.Position.Y);
				stream.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(voxel.Position.Z);
				stream.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(voxel.Size.X);
				stream.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(voxel.Size.Y);
				stream.Write(buffer, 0, buffer.Length);

				buffer = BitConverter.GetBytes(voxel.Size.Z);
				stream.Write(buffer, 0, buffer.Length);
			}

		}

		#endregion
	}

	/// <summary>
	/// Defect Voxel (Pixel)
	/// </summary>
	public struct Voxel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Voxel" /> struct.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="size">The size.</param>
		public Voxel(Vector position, Vector size)
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
		public Vector Position { get; set; }

		/// <summary>
		/// Gets or sets the size.
		/// </summary>
		/// <value>
		/// The size.
		/// </value>
		public Vector Size { get; set; }
	}
}