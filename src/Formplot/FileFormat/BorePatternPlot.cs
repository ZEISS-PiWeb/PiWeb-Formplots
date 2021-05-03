#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Shows points on a plane, which have a planar deviation in a certain direction.
	/// </summary>
	public sealed class BorePatternPlot : Formplot<CurvePoint, CurveGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public BorePatternPlot() : base( FormplotTypes.BorePattern )
		{ }

		#endregion
	}
}