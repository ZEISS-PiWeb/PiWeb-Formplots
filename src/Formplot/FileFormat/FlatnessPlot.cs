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
	/// Shows points on a plane, which have a deviation orthogonal to the plane.
	/// </summary>
	public sealed class FlatnessPlot : Formplot<PlanePoint,PlaneGeometry>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FlatnessPlot"/> class.
		/// </summary>
		public FlatnessPlot() : base( FormplotTypes.Flatness )
		{}

		#endregion
	}
}