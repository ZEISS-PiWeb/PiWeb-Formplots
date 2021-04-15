#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Similar to a curve plot, but with additional geometry features.
	/// </summary>
	public sealed class FlushGapPlot : Formplot<FlushGapPoint, FlushGapGeometry>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FlushGapPlot"/> class.
		/// </summary>
		public FlushGapPlot() : base( FormplotTypes.FlushGap )
		{
		}

		#endregion
	}
}