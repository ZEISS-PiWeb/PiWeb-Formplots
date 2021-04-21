#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2014-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Types of tolerances.
	/// </summary>
	public enum ToleranceType
	{
		/// <summary>
		/// Upper and lower tolerance.
		/// </summary>
		Default,

		/// <summary>
		/// Circular tolerance.
		/// </summary>
		Circular,

		/// <summary>
		/// Rectangular tolerance.
		/// </summary>
		Rectangular,

		/// <summary>
		/// Thee dimensional tolerance
		/// </summary>
		Spatial
	}
}
