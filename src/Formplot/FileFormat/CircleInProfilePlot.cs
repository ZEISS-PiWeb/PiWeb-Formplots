﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	/// <summary>
	/// Shows a circle, which has been fitted into a measured geometry.
	/// </summary>
	public sealed class CircleInProfilePlot : Formplot<CircleInProfilePoint, CircleInProfileGeometry>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public CircleInProfilePlot() : base( FormplotTypes.CircleInProfile )
		{ }

		#endregion
	}
}