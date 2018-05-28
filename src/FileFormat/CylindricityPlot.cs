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
	/// Shows deviations from a nominal cylinder or a cylinder axis
	/// </summary>
	public class CylindricityPlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CylindricityPlot"/> class.
		/// </summary>
		public CylindricityPlot() : base( FormplotTypes.Cylindricity )
		{}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the nominal geometry.
		/// </summary>
		public new CylinderGeometry Nominal
		{
			get => base.Nominal as CylinderGeometry;
			set => base.Nominal = value;
		}

		/// <summary>
		/// Gets or sets the actual geometry.
		/// </summary>
		public new CylinderGeometry Actual
		{
			get => base.Actual as CylinderGeometry;
			set => base.Actual = value;
		}

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public new IEnumerable<CylinderPoint> Points
		{
			get => base.Points.Cast<CylinderPoint>();
			set => base.Points = value;
		}

		#endregion
	}
}