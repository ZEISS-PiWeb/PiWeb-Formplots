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
	/// A plot representing a curve profile
	/// </summary>
	public class CurveProfilePlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CurveProfilePlot"/> class.
		/// </summary>
		public CurveProfilePlot() : base( FormplotTypes.CurveProfile )
		{}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the nominal geometry.
		/// </summary>
		public new CurveGeometry Nominal
		{
			get { return base.Nominal as CurveGeometry; }
			set { base.Nominal = value; }
		}

		/// <summary>
		/// Gets or sets the actual geometry.
		/// </summary>
		public new CurveGeometry Actual
		{
			get { return base.Actual as CurveGeometry; }
			set { base.Actual = value; }
		}

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public new IEnumerable<CurvePoint> Points
		{
			get { return base.Points.Cast<CurvePoint>(); }
			set { base.Points = value; }
		}

		#endregion
	}
}