#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Represents a flash gap point.
	/// </summary>
	public sealed class FlushGapPoint : CurvePointBase<FlushGapPoint, FlushGapGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public FlushGapPoint() { }

		/// <summary>Constructor.</summary>
		/// <param name="position">The position.</param>
		/// <param name="direction">The direction.</param>
		/// <param name="deviation">The deviation.</param>
		public FlushGapPoint( Vector position, Vector direction, double deviation )
			: base( position, direction, deviation )
		{ }

		#endregion
	}
}