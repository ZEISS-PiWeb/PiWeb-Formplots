#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Determines how the inner flush point connects to the tangent of the outer flush point
	/// </summary>
	public enum FlushPointConnectionType
	{
		/// <summary>
		/// Connects the inner flush point with an orthogonal line to the tangent of the outer flush point
		/// </summary>
		Orthogonal
	}
}