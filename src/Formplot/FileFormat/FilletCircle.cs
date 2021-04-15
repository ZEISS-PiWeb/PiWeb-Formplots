#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;

	#endregion

	/// <summary>
	/// Represents the circle of a fillet plot, which consists of a radius and a FilletPoint
	/// </summary>
	public sealed class FilletCircle
	{
		#region constructors

		internal FilletCircle()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="FilletCircle" /> class.
		/// </summary>
		/// <param name="radius">The radius.</param>
		/// <param name="center">The center point of circle.</param>
		public FilletCircle( double radius, FilletPoint center )
		{
			if( double.IsNaN( radius ) || radius <= double.Epsilon )
				throw new ArgumentException( $"Radius is not a valid value: {radius}" );

			Radius = double.IsNaN( radius ) ? -1 : radius;

			if( center?.Segment == null || center.Position == null || center.Direction == null || double.IsNaN( center.Deviation ) )
				throw new ArgumentException( $"center is invalid" );

			Center = new FilletPoint( center.Position, center.Direction, center.Deviation );
		}

		#endregion

		#region properties

		/// <summary>
		/// Radius of FilletCircle
		/// </summary>
		public double Radius { get; set; }

		/// <summary>
		/// Center point of FilletCircle
		/// </summary>
		public FilletPoint? Center { get; set; }

		#endregion
	}
}