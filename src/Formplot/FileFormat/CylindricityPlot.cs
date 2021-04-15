#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
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

		/// <summary>
		/// Initializes a new instance of the <see cref="CylindricityPlot"/> class.
		/// </summary>
		public CylindricityPlot() : base( FormplotTypes.Cylindricity ){}

		#endregion
	}
}