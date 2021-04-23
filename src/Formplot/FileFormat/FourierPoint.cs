#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.IO;

	#endregion

	/// <summary>
	/// Represents a point of a Fourier plot
	/// </summary>
	public sealed class FourierPoint : Point<FourierPoint,EmptyGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public FourierPoint( ) { }

		/// <summary>Constructor.</summary>
		/// <param name="harmonic">The harmonic of fundamental frequency.</param>
		/// <param name="amplitude">The amplitude.</param>
		public FourierPoint( uint harmonic, double amplitude )
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

		/// <inheritdoc />
		internal override void WriteToStream( BinaryWriter writer )
		{
			var buffer = BitConverter.GetBytes( Harmonic );

			if( BitConverter.IsLittleEndian )
				Array.Reverse( buffer );

			writer.Write( buffer );
			writer.Write( Amplitude );
		}

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader )
		{
			var buffer = reader.ReadBytes( sizeof( uint ) );

			if( BitConverter.IsLittleEndian )
				Array.Reverse( buffer, 0, sizeof( uint ) );

			Harmonic = BitConverter.ToUInt32( buffer, 0 );
			Amplitude = reader.ReadDouble();
		}

		#endregion
	}
}