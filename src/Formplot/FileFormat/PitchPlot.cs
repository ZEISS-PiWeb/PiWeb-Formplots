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
	/// Shows deviations of indexed data points, like a bar chart.
	/// </summary>
	public sealed class PitchPlot : Formplot<PitchPoint,PitchGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public PitchPlot() : base( FormplotTypes.Pitch )
		{}

		#endregion
	}
}