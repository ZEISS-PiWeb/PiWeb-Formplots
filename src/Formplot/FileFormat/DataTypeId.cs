#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Determines the datatype of a <see cref="Property"/> instance an.
	/// </summary>
	public enum DataTypeId
	{
		/// <summary>
		/// Natural number
		/// </summary>
		Integer = 1,

		/// <summary>
		/// Floating point number
		/// </summary>
		Double = 2,

		/// <summary>
		/// String
		/// </summary>
		String = 4,

		/// <summary>
		/// Timestamp
		/// </summary>
		DateTime = 16,

		/// <summary>
		/// Timespan
		/// </summary>
		TimeSpan = 32,
	}
}