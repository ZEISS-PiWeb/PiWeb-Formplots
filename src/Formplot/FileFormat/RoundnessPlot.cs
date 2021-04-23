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
	/// Shows deviations from a nominal circle geometry.
	/// </summary>
	public sealed class RoundnessPlot : Formplot<CirclePoint, CircleGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public RoundnessPlot() : base( FormplotTypes.Roundness )
		{ }

		#endregion
	}
}