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
	/// Shows deviations from a nominal circle geometry.
	/// </summary>
	public class RoundnessPlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RoundnessPlot"/> class.
		/// </summary>
		public RoundnessPlot() : base( FormplotTypes.Roundness )
		{}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the nominal geometry.
		/// </summary>
		public new CircleGeometry Nominal
		{
			get => base.Nominal as CircleGeometry;
			set => base.Nominal = value;
		}

		/// <summary>
		/// Gets or sets the actual geometry.
		/// </summary>
		public new CircleGeometry Actual
		{
			get => base.Actual as CircleGeometry;
			set => base.Actual = value;
		}

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public new IEnumerable<CirclePoint> Points
		{
			get => base.Points.Cast<CirclePoint>();
			set => base.Points = value;
		}

		#endregion
	}
}