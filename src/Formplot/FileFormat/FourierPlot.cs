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
	/// Shows the deviation (amplitude) per harmonic => a.k.a. Fourier plot
	/// </summary>
	public sealed class FourierPlot : Formplot<FourierPoint, EmptyGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public FourierPlot() : base( FormplotTypes.Fourier )
		{}

		#endregion
	}
}