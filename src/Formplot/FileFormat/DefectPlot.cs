#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Contains defects.
	/// </summary>
	public sealed class DefectPlot : Formplot<Defect, DefectGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public DefectPlot() : base( FormplotTypes.Defect )
		{ }

		#endregion
	}
}