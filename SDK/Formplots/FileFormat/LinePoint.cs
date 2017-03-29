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
	/// Represents a point of a line
	/// </summary>
	public class LinePoint : Point
	{
		#region constructors

		internal LinePoint()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinePoint"/> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="position">The position.</param>
		/// <param name="deviation">The deviation.</param>
		public LinePoint( Segment segment, double position, double deviation ) : base( segment )
		{
			Position = position;
			Deviation = deviation;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public double Position { get; set; }

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
			byte[] buffer = BitConverter.GetBytes( Position );
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
			var buffer = GetBuffer( stream, 2 * sizeof( double ) );

			var posX = BitConverter.ToDouble( buffer, 0 * sizeof( double ) );
			var deviation = BitConverter.ToDouble( buffer, 1 * sizeof( double ) );

			Position = posX;
			Deviation = deviation;
		}

		#endregion
	}
}