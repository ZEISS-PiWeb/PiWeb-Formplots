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
	/// Describes a point in a circle
	/// </summary>
	public sealed class CircleInProfilePoint : CirclePointBase<CircleInProfilePoint, CircleInProfileGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public CircleInProfilePoint() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="CirclePoint"/> class.
		/// </summary>
		/// <param name="angle">The angle.</param>
		/// <param name="deviation">The deviation.</param>
		public CircleInProfilePoint( double angle, double deviation ) : base( angle, deviation )
		{ }

		#endregion
	}
}