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
	/// Represents a point of a pitch.
	/// </summary>
	public class PitchPoint : Point
	{
		#region constructors

		internal PitchPoint()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="PitchPoint"/> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="deviation">The deviation.</param>
		public PitchPoint( Segment segment, double deviation ) : base( segment )
		{
			Deviation = deviation;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <remarks>
		/// The position is not stored in the formplot file. Instead, the array index is used.
		/// </remarks>
		public int Position { get; set; }

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
			var buffer = BitConverter.GetBytes( Deviation );
			stream.Write( buffer, 0, buffer.Length );
		}

		/// <summary>
		/// Reads the point from a binary data stream
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="index"></param>
		internal override void ReadFromStream( Stream stream, int index )
		{
			var buffer = GetBuffer( stream, sizeof( double ) );
			var deviation = BitConverter.ToDouble( buffer, 0 * sizeof( double ) );

			Position = index;
			Deviation = deviation;
		}

		#endregion
	}
}