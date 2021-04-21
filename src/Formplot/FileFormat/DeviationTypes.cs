#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Determines what kind of deviation is stored.
	/// </summary>
	public enum DeviationTypes {

		/// <summary>
		/// Length in mm.
		/// </summary>
		None,

		/// <summary>
		/// Length in mm.
		/// </summary>
		Length,

		/// <summary>
		/// Angle in degrees.
		/// </summary>
		Angle
	}
}