#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.IO;

	#endregion

	/// <summary>
	/// Represents a point on a plane.
	/// </summary>
	public sealed class PlanePoint : Point<PlanePoint, PlaneGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public PlanePoint() { }

		//// <summary>Constructor.</summary>
		/// <param name="coordinate1">The coordinate in the first plane axis direction.</param>
		/// <param name="coordinate2">The coordinate in the second plane axis direction.</param>
		/// <param name="deviation">The deviation.</param>
		public PlanePoint( double coordinate1, double coordinate2, double deviation )
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

		/// <inheritdoc />
		internal override void WriteToStream( BinaryWriter writer )
		{
			writer.Write( Coordinate1 );
			writer.Write( Coordinate2 );
			writer.Write( Deviation );
		}

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader, Version version )
		{
			Coordinate1 = reader.ReadDouble();
			Coordinate2 = reader.ReadDouble();
			Deviation = reader.ReadDouble();
		}

		#endregion
	}
}