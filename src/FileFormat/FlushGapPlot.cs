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
	/// Similar to a curve plot, but with additional geometry features.
	/// </summary>
	/// <seealso cref="Zeiss.IMT.PiWeb.Formplot.FileFormat.Formplot" />
	public class FlushGapPlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FlushGapPlot"/> class.
		/// </summary>
		public FlushGapPlot() : base( FormplotTypes.FlushGap )
		{
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the nominal geometry.
		/// </summary>
		public new FlushGapGeometry Nominal
		{
			get => base.Nominal as FlushGapGeometry;
			set => base.Nominal = value;
		}

		/// <summary>
		/// Gets or sets the actual geometry.
		/// </summary>
		public new FlushGapGeometry Actual
		{
			get => base.Actual as FlushGapGeometry;
			set => base.Actual = value;
		}

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public new IEnumerable<FlushGapPoint> Points
		{
			get => base.Points.Cast<FlushGapPoint>();
			set => base.Points = value;
		}

		#endregion
	}
}