#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	#region usings

	using System.Collections.Generic;
	using System.Linq;

	#endregion

	/// <summary>
	/// Shows the deviation (amplitude) per harmonic => a.k.a. Fourierplot
	/// </summary>
	public class FourierPlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FourierPlot"/> class.
		/// </summary>
		public FourierPlot() : base( FormplotTypes.Fourier )
		{}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public new IEnumerable<FourierPoint> Points
		{
			get { return base.Points.Cast<FourierPoint>(); }
			set { base.Points = value; }
		}

		#endregion
	}
}