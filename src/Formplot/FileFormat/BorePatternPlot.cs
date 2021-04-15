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
	/// Shows points on a plane, which have a planar deviation in a certain direction.
	/// </summary>
	public sealed class BorePatternPlot : Formplot<CurvePoint,CurveGeometry>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BorePatternPlot"/> class.
		/// </summary>
		public BorePatternPlot() : base( FormplotTypes.BorePattern )
		{}

		#endregion
	}
}