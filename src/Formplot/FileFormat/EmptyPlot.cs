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
	/// A plot which is only used to manage properties.
	/// </summary>
	public sealed class EmptyPlot : Formplot<EmptyPoint, EmptyGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public EmptyPlot() : base( FormplotTypes.None ) { }

		#endregion
	}
}