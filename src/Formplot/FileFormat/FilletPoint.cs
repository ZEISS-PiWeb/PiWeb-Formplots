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
	/// Represents a curve point.
	/// </summary>
	public sealed class FilletPoint : CurvePointBase<FilletPoint, FilletGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public FilletPoint() { }

		/// <summary>Constructor.</summary>
		/// <param name="position">The position.</param>
		/// <param name="direction">The direction.</param>
		/// <param name="deviation">The deviation.</param>
		public FilletPoint( Vector position, Vector direction, double deviation )
			: base( position, direction, deviation )
		{ }

		#endregion
	}
}