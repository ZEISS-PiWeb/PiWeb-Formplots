#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Geometry types
	/// </summary>
	public enum GeometryTypes
	{
		/// <summary>
		/// No geometry
		/// </summary>
		None,
		/// <summary>
		/// Circle
		/// </summary>
		Circle,
		/// <summary>
		/// Plane
		/// </summary>
		Plane,
		/// <summary>
		/// Curve
		/// </summary>
		Curve,
		/// <summary>
		/// Line
		/// </summary>
		Line,
		/// <summary>
		/// Cylinder
		/// </summary>
		Cylinder,
		/// <summary>
		/// Circle in a profile
		/// </summary>
		CircleInProfile,
		/// <summary>
		/// Defect
		/// </summary>
		Defect,
	}
}
