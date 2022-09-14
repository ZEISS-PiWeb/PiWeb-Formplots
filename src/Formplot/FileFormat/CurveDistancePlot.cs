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
	/// A plot representing a curve distance.
	/// </summary>
	public sealed class CurveDistancePlot : Formplot<CurveDistancePoint, CurveDistanceGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public CurveDistancePlot() : base( FormplotTypes.CurveDistance )
		{ }

		#endregion
	}
}