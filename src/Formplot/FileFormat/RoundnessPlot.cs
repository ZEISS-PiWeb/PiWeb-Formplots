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
	public sealed class RoundnessPlot : Formplot<CirclePoint,CircleGeometry>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RoundnessPlot"/> class.
		/// </summary>
		public RoundnessPlot() : base( FormplotTypes.Roundness )
		{

		}

		#endregion
	}
}