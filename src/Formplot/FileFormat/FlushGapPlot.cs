#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017-2021                        */
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

		/// <summary>Constructor.</summary>
		public FlushGapPlot() : base( FormplotTypes.FlushGap )
		{ }

		#endregion
	}
}