#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Stellt einen Kurvenpunkt dar.
	/// </summary>
	public sealed class FilletPoint : CurvePointBase<FilletPoint, FilletGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public FilletPoint() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="FilletPoint"/> class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="direction">The direction.</param>
		/// <param name="deviation">The deviation.</param>
		public FilletPoint( Vector position, Vector direction, double deviation )
			: base( position, direction, deviation )
		{ }

		#endregion
	}
}