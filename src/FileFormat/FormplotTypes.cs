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
		Fourier

	}
}
