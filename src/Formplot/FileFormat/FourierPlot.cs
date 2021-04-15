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
	/// Shows the deviation (amplitude) per harmonic => a.k.a. Fourierplot
	/// </summary>
	public sealed class FourierPlot : Formplot<FourierPoint, EmptyGeometry>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FourierPlot"/> class.
		/// </summary>
		public FourierPlot() : base( FormplotTypes.Fourier )
		{}

		#endregion
	}
}