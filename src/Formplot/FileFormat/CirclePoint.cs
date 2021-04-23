#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Describes a point in a circle
	/// </summary>
	public sealed class CirclePoint : CirclePointBase<CirclePoint, CircleGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public CirclePoint() { }

		/// <summary>Constructor.</summary>
		/// <param name="angle">The angle.</param>
		/// <param name="deviation">The deviation.</param>
		public CirclePoint( double angle, double deviation ) : base( angle, deviation )
		{ }

		#endregion
	}
}