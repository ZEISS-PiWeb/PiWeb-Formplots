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
	/// Shows points on a plane, which have a deviation orthogonal to the plane.
	/// </summary>
	public class FlatnessPlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FlatnessPlot"/> class.
		/// </summary>
		public FlatnessPlot() : base( FormplotTypes.Flatness )
		{}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the nominal geometry.
		/// </summary>
		public new PlaneGeometry Nominal
		{
			get => base.Nominal as PlaneGeometry;
			set => base.Nominal = value;
		}

		/// <summary>
		/// Gets or sets the actual geometry.
		/// </summary>
		public new PlaneGeometry Actual
		{
			get => base.Actual as PlaneGeometry;
			set => base.Actual = value;
		}

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public new IEnumerable<PlanePoint> Points
		{
			get => base.Points.Cast<PlanePoint>();
			set => base.Points = value;
		}

		#endregion
	}
}