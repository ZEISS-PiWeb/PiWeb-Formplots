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
	/// Shows deviations from a nominal cylinder or a cylinder axis
	/// </summary>
	public sealed class CylindricityPlot : Formplot<CylinderPoint, CylinderGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public CylindricityPlot() : base( FormplotTypes.Cylindricity ){}

		#endregion
	}
}