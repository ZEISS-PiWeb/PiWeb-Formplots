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
	/// Shows deviations from a straight nominal line.
	/// </summary>
	public sealed class StraightnessPlot : Formplot<LinePoint, LineGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public StraightnessPlot() : base( FormplotTypes.Straightness )
		{ }

		#endregion
	}
}