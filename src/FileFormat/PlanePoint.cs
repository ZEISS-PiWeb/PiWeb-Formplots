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
	/// Represents a point on a plane.
	/// </summary>
	public class PlanePoint : Point
	{
		#region constructors

		internal PlanePoint()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="PlanePoint"/> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="coordinate1">The coordinate in the first plane axis direction.</param>
		/// <param name="coordinate2">The coordinate in the second plane axis direction.</param>
		/// <param name="deviation">The deviation.</param>
		public PlanePoint( Segment segment, double coordinate1, double coordinate2, double deviation ) : base( segment )
		{
			Coordinate1 = coordinate1;
			Coordinate2 = coordinate2;
			Deviation = deviation;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the coordinate of the first plane axis.
		/// </summary>
		public double Coordinate1 { get; set; }

		/// <summary>
		/// Gets or sets the coordinate of the second plane axis.
		/// </summary>
		public double Coordinate2 { get; set; }

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
		internal override void WriteToStream( Stream stream)
		{
			var buffer = BitConverter.GetBytes( Coordinate1 );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Coordinate2 );
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
			var buffer = GetBuffer( stream, 3 * sizeof( double ) );

			Coordinate1 = BitConverter.ToDouble( buffer, 0 * sizeof( double ) );
			Coordinate2 = BitConverter.ToDouble( buffer, 1 * sizeof( double ) );
			Deviation = BitConverter.ToDouble( buffer, 2 * sizeof( double ) );
		}

		#endregion
	}
}