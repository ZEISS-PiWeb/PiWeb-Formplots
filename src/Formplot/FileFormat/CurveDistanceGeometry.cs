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
	/// Representation of a curve distance geometry.
	/// </summary>
	public class CurveDistanceGeometry : Geometry
	{
		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType => GeometryTypes.CurveDistance;

		#endregion
	}
}