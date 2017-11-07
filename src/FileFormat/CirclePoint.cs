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
	/// Describes a point in a circle
	/// </summary>
	public class CirclePoint : Point
	{
		#region constructors

		internal CirclePoint()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="CirclePoint"/> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="angle">The angle.</param>
		/// <param name="deviation">The deviation.</param>
		public CirclePoint( Segment segment, double angle, double deviation ) : base( segment )
		{
			Angle = angle;
			Deviation = deviation;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the angle.
		/// </summary>
		/// <value>
		/// The angle.
		/// </value>
		public double Angle { get; set; }

		/// <summary>
		/// Gets or sets the deviation.
		/// </summary>
		/// <value>
		/// The deviation.
		/// </value>
		public double Deviation { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Writes the point into a binary data stream.
		/// </summary>
		/// <param name="stream"></param>
		internal override void WriteToStream( Stream stream )
		{
			var buffer = BitConverter.GetBytes( Angle );
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

			Angle = BitConverter.ToDouble( buffer, 0 * sizeof( double ) );
			Deviation = BitConverter.ToDouble( buffer, 1 * sizeof( double ) );
		}

		#endregion
	}
}