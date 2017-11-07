#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	#region usings



	#endregion

	/// <summary>
	/// A plot which is only used to manage properties.
	/// </summary>
	public class EmptyPlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EmptyPlot" /> class.
		/// </summary>
		public EmptyPlot() : base( FormplotTypes.None )
		{}

		#endregion
	}
}