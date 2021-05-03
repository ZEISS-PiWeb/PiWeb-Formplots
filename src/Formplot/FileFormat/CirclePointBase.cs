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

	using System.IO;

	#endregion

	/// <summary>
	/// Describes a point in a circle
	/// </summary>
	public abstract class CirclePointBase<TPoint, TGeometry> : Point<TPoint, TGeometry>
		where TPoint : Point<TPoint, TGeometry>, new()
		where TGeometry : Geometry, new()
	{
		#region constructors

		/// <inheritdoc/>
		internal CirclePointBase() { }

		/// <summary>Constructor.</summary>
		/// <param name="angle">The angle.</param>
		/// <param name="deviation">The deviation.</param>
		internal CirclePointBase( double angle, double deviation )
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
		/// Gets or sets the deviation of this point.
		/// </summary>
		public double Deviation { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void WriteToStream( BinaryWriter writer )
		{
			writer.Write( Angle );
			writer.Write( Deviation );
		}

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader )
		{
			Angle = reader.ReadDouble();
			Deviation = reader.ReadDouble();
		}

		#endregion
	}
}