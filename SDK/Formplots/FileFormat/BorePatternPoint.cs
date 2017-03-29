#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2014                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Describes a bore pattern point
	/// </summary>
	public class BorePatternPoint : CurvePoint
	{
		#region constructors

		internal BorePatternPoint()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="BorePatternPoint"/> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="position">The position.</param>
		/// <param name="direction">The direction.</param>
		/// <param name="deviation">The deviation.</param>
		public BorePatternPoint( Segment segment, Vector position, Vector direction, double deviation ) :
			base( segment, position, direction, deviation )
		{}

		#endregion
	}
}