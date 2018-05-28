#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Represents a point of a flush and gap plot
	/// </summary>
	/// <seealso cref="Zeiss.IMT.PiWeb.Formplot.FileFormat.CurvePoint" />
	public class FlushGapPoint : CurvePoint
	{
		#region constructors

		internal FlushGapPoint()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FlushGapPoint" /> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="position">The position.</param>
		/// <param name="direction">The direction.</param>
		/// <param name="deviation">The deviation.</param>
		public FlushGapPoint( Segment segment, Vector position, Vector direction, double deviation ) : base( segment, position, direction, deviation )
		{
		}
		
		#endregion
	}
}