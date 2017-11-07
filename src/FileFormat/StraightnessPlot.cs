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
	/// Shows deviations from a straight nominal line.
	/// </summary>
	public class StraightnessPlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="StraightnessPlot"/> class.
		/// </summary>
		public StraightnessPlot() : base( FormplotTypes.Straightness )
		{}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the nominal geometry.
		/// </summary>
		public new LineGeometry Nominal
		{
			get { return base.Nominal as LineGeometry; }
			set { base.Nominal = value; }
		}

		/// <summary>
		/// Gets or sets the actual geometry.
		/// </summary>
		public new LineGeometry Actual
		{
			get { return base.Actual as LineGeometry; }
			set { base.Actual = value; }
		}

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public new IEnumerable<LinePoint> Points
		{
			get { return base.Points.Cast<LinePoint>(); }
			set { base.Points = value; }
		}

		#endregion
	}
}