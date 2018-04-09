#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	#region usings

	using System.Collections.Generic;
	using System.Linq;

	#endregion

	/// <summary>
	/// Contains defects
	/// </summary>
	/// <seealso cref="Zeiss.IMT.PiWeb.Formplot.FileFormat.Formplot" />
	public class DefectPlot : Formplot
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DefectPlot"/> class.
		/// </summary>
		public DefectPlot() : base( FormplotTypes.Defect )
		{
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public new IEnumerable<Defect> Points
		{
			get { return base.Points.Cast<Defect>(); }
			set { base.Points = value; }
		}

		#endregion
	}
}