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
	/// Shows deviations from a straight nominal line.
	/// </summary>
	public sealed class StraightnessPlot : Formplot<LinePoint, LineGeometry>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="StraightnessPlot"/> class.
		/// </summary>
		public StraightnessPlot() : base( FormplotTypes.Straightness )
		{}

		#endregion
	}
}