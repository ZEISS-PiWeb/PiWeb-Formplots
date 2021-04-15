#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Contains defects
	/// </summary>
	public sealed class DefectPlot : Formplot<Defect,DefectGeometry>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DefectPlot"/> class.
		/// </summary>
		public DefectPlot() : base( FormplotTypes.Defect )
		{

		}

		#endregion
	}
}