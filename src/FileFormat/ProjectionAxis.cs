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
	/// Possible projections on coordinate axis
	/// </summary>
	public enum ProjectionAxis
	{
		/// <summary>
		/// No projection
		/// </summary>
		None,

		/// <summary>
		/// Projection on the X axis
		/// </summary>
		X,

		/// <summary>
		/// Projection on the Y axis
		/// </summary>
		Y,

		/// <summary>
		/// Projection on the Z axis
		/// </summary>
		Z
	}
}