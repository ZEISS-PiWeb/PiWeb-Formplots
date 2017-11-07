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
	/// Shows a circle, which has been fitted into a measured geometry.
	/// </summary>
	public class CircleInProfilePlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CircleInProfilePlot"/> class.
		/// </summary>
		public CircleInProfilePlot() : base( FormplotTypes.CircleInProfile )
		{}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the nominal geometry.
		/// </summary>
		public new CircleInProfileGeometry Nominal
		{
			get { return base.Nominal as CircleInProfileGeometry; }
			set { base.Nominal = value; }
		}

		/// <summary>
		/// Gets or sets the actual geometry.
		/// </summary>
		public new CircleInProfileGeometry Actual
		{
			get { return base.Actual as CircleInProfileGeometry; }
			set { base.Actual = value; }
		}

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public new IEnumerable<CircleInProfilePoint> Points
		{
			get { return base.Points.Cast<CircleInProfilePoint>(); }
			set { base.Points = value; }
		}

		#endregion
	}
}