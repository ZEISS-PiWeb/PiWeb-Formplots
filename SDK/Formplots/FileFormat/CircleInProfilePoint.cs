#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2014                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.IO;

	#endregion

	/// <summary>
	/// Describes a point of a circle in a profile.
	/// </summary>
	public class CircleInProfilePoint : CirclePoint
	{
		#region constructors

		internal CircleInProfilePoint()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CircleInProfilePoint"/> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="angle">The angle.</param>
		/// <param name="deviation">The deviation.</param>
		public CircleInProfilePoint( Segment segment, double angle, double deviation ) : base( segment, angle, deviation )
		{
			Angle = angle;
			Deviation = deviation;
		}

		#endregion

	}
}