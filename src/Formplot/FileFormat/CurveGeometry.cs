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
	/// Representation of a curve geometry.
	/// </summary>
	public class CurveGeometry : Geometry
	{
		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.Curve;
	}
}