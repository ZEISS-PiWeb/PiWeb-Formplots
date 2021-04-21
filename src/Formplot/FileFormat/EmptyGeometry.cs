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
	/// Empty geometry used for property plot.
	/// </summary>
	public class EmptyGeometry : Geometry
	{
		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.None;

		#endregion
	}
}