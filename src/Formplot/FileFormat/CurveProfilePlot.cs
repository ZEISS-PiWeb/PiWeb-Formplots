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
	/// A plot representing a curve profile.
	/// </summary>
	public sealed class CurveProfilePlot : Formplot<CurvePoint, CurveGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public CurveProfilePlot() : base( FormplotTypes.CurveProfile )
		{ }

		#endregion
	}
}