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
	/// Stellt einen Zylinderpunkt dar.
	/// </summary>
	public class CylinderPoint : Point
	{
		#region constructors

		internal CylinderPoint()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="CylinderPoint"/> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="angle">The angle.</param>
		/// <param name="height">The height.</param>
		/// <param name="deviation">The deviation.</param>
		public CylinderPoint( Segment segment, double angle, double height, double deviation ) : base( segment )
		{
			Angle = angle;
			Height = height;
			Deviation = deviation;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the angle in radians.
		/// </summary>
		public double Angle { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public double Height { get; set; }

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
			var buffer = BitConverter.GetBytes( Angle );
			stream.Write( buffer, 0, buffer.Length );

			buffer = BitConverter.GetBytes( Height );
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

			Angle = BitConverter.ToDouble( buffer, 0 * sizeof( double ) );
			Height = BitConverter.ToDouble( buffer, 1 * sizeof( double ) );
			Deviation = BitConverter.ToDouble( buffer, 2 * sizeof( double ) );
		}

		#endregion
	}
}