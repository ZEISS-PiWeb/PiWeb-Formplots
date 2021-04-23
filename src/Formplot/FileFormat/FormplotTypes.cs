#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Formplot types
	/// </summary>
	public enum FormplotTypes
	{
		/// <summary>
		/// No specific formplot type. The instance only keeps metadata.
		/// </summary>
		None,
		/// <summary>
		/// Roundness
		/// </summary>
		Roundness,
		/// <summary>
		/// Flatness
		/// </summary>
		Flatness,
		/// <summary>
		/// Curve profile
		/// </summary>
		CurveProfile,
		/// <summary>
		/// Straightness
		/// </summary>
		Straightness,
		/// <summary>
		/// Cylindricity
		/// </summary>
		Cylindricity,
		/// <summary>
		/// Pitch
		/// </summary>
		Pitch,
		/// <summary>
		/// BorePattern
		/// </summary>
		BorePattern,
		/// <summary>
		/// CircleInProfile
		/// </summary>
		CircleInProfile,
		/// <summary>
		/// Fourier
		/// </summary>
		Fourier,
		/// <summary>
		/// Flush and Gap plot
		/// </summary>
		FlushGap,
		/// <summary>
		/// Fillet plot
		/// </summary>
		Fillet,
		/// <summary>
		/// Defect plot data
		/// </summary>
		Defect
	}
}
