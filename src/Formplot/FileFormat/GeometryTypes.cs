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
		/// Flush and Gap
		/// </summary>
		FlushGap,

		/// <summary>
		/// Defect
		/// </summary>
		Defect,

		/// <summary>
		/// Fillet
		/// </summary>
		Fillet,

		/// <summary>
		/// Pitch
		/// </summary>
		Pitch,

		/// <summary>
		/// Curve distance
		/// </summary>
		CurveDistance,
	}
}