#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Similar to a flush and gap plot, but without reference and measure profile and an additional circle.
	/// </summary>
	public sealed class FilletPlot : Formplot<FilletPoint, FilletGeometry>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FilletPlot"/> class.
		/// </summary>
		public FilletPlot() : base( FormplotTypes.Fillet )
		{
		}
	}
}