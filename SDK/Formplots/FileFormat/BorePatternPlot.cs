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
	/// Shows points on a plane, which have a planar deviation in a certain direction.
	/// </summary>
	public class BorePatternPlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BorePatternPlot"/> class.
		/// </summary>
		public BorePatternPlot() : base( FormplotTypes.BorePattern )
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
		public new IEnumerable<BorePatternPoint> Points
		{
			get { return base.Points.Cast<BorePatternPoint>(); }
			set { base.Points = value; }
		}

		#endregion
	}
}