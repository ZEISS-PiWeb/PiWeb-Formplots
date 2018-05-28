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

	using System.Collections.Generic;
	using System.Linq;

	#endregion

	/// <summary>
	/// Shows deviations of indexed data points, like a bar chart.
	/// </summary>
	public class PitchPlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PitchPlot"/> class.
		/// </summary>
		public PitchPlot() : base( FormplotTypes.Pitch )
		{}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public new IEnumerable<PitchPoint> Points
		{
			get => base.Points.Cast<PitchPoint>();
			set => base.Points = value;
		}

		#endregion
	}
}