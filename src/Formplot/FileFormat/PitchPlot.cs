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
	/// Shows deviations of indexed data points, like a bar chart.
	/// </summary>
	public sealed class PitchPlot : Formplot<PitchPoint,PitchGeometry>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PitchPlot"/> class.
		/// </summary>
		public PitchPlot() : base( FormplotTypes.Pitch )
		{}

		#endregion
	}
}