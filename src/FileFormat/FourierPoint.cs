#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.IO;

	#endregion

	/// <summary>
	/// Represents a point of a Fourierplot
	/// </summary>
	public class FourierPoint : Point
	{
		#region constructors

		internal FourierPoint()
		{
			Segment = new Segment( "Fourier", SegmentTypes.None );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FourierPoint" /> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="harmonic">The harmonic of fundamental frequency.</param>
		/// <param name="amplitude">The amplitude.</param>
		public FourierPoint( Segment segment, uint harmonic, double amplitude ) : base( segment )
		{
			Harmonic = harmonic;
			Amplitude = amplitude;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the harmonic of fundamental frequency.
		/// </summary>
		public uint Harmonic { get; set; }

		/// <summary>
		/// Gets or sets the deviation in mm.
		/// </summary>
		public double Amplitude { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Writes the point into a binary data stream.
		/// </summary>
		/// <param name="stream"></param>
		internal override void WriteToStream( Stream stream )
		{
			var buffer = BitConverter.GetBytes( Harmonic );

			if( BitConverter.IsLittleEndian )
				Array.Reverse( buffer );

			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Amplitude );
			stream.Write( buffer, 0, buffer.Length );
		}

		/// <summary>
		/// Reads the point from a binary data stream
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="index"></param>
		internal override void ReadFromStream( Stream stream, int index )
		{
			var buffer = GetBuffer( stream, sizeof( uint ) + sizeof( double ) );

			if( BitConverter.IsLittleEndian )
				Array.Reverse( buffer, 0, sizeof( uint ) );

			var harmonic = BitConverter.ToUInt32( buffer, 0 );
			var amplitude = BitConverter.ToDouble( buffer, 1 * sizeof( uint ) );

			Harmonic = harmonic;
			Amplitude = amplitude;
		}

		#endregion
	}
}