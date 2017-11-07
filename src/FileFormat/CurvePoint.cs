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
	using System.IO;

	#endregion

	/// <summary>
	/// Stellt einen Kurvenpunkt dar.
	/// </summary>
	public class CurvePoint : Point
	{
		#region constructors

		internal CurvePoint() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CurvePoint"/> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="position">The position.</param>
		/// <param name="direction">The direction.</param>
		/// <param name="deviation">The deviation.</param>
		public CurvePoint( Segment segment, Vector position, Vector direction, double deviation ) : base( segment )
		{
			Position = position;
			Direction = direction;
			Deviation = deviation;
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
		/// Gets or sets the direction.
		/// </summary>
		public Vector Direction { get; set; }


		/// <summary>
		/// Gets or sets the deviation.
		/// </summary>
		public double Deviation { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Writes the point into a binary data stream.
		/// </summary>
		/// <param name="stream"></param>
		internal override void WriteToStream( Stream stream )
		{
			byte[] buffer = BitConverter.GetBytes( Position.X );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Position.Y );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Position.Z );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Direction.X );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Direction.Y );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Direction.Z );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Deviation );
			stream.Write( buffer, 0, buffer.Length );
		}

		/// <summary>
		/// Reads the point from a binary data stream
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="index"></param>
		internal override void ReadFromStream( Stream stream, int index )
		{
			var buffer = GetBuffer( stream, 7 * sizeof( double ) );

			var posX = BitConverter.ToDouble( buffer, 0 * sizeof( double ) );
			var posY = BitConverter.ToDouble( buffer, 1 * sizeof( double ) );
			var posZ = BitConverter.ToDouble( buffer, 2 * sizeof( double ) );
			var dirX = BitConverter.ToDouble( buffer, 3 * sizeof( double ) );
			var dirY = BitConverter.ToDouble( buffer, 4 * sizeof( double ) );
			var dirZ = BitConverter.ToDouble( buffer, 5 * sizeof( double ) );
			var deviation = BitConverter.ToDouble( buffer, 6 * sizeof( double ) );

			Position = new Vector { X = posX, Y = posY, Z = posZ };
			Direction = new Vector { X = dirX, Y = dirY, Z = dirZ };
			Deviation = deviation;
		}

		#endregion
	}
}