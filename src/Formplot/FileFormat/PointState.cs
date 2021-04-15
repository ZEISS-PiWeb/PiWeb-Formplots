#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;

	#endregion

	/// <summary>
	/// The possible states of a point
	/// </summary>
	[Flags]
	public enum PointState
	{
		/// <summary>
		/// No state.
		/// </summary>
		None = 0x0,

		/// <summary>
		/// An outlier point.
		/// </summary>
		Outlier = 0x1,

		/// <summary>
		/// A point overlapping another point.
		/// </summary>
		Overlap = 0x2,

		/// <summary>
		/// Tagged because of a tilted force.
		/// </summary>
		TiltedForce = 0x4,

		/// <summary>
		/// Tagged because of a tilted acceleration.
		/// </summary>
		TiltedAcceleration = 0x8,

		/// <summary>
		/// Point lies out of the evaluation range.
		/// </summary>
		EvaluationRange = 0x10,

		/// <summary>
		/// Point is a duplicate of another point.
		/// </summary>
		Duplicate = 0x20,

		/// <summary>
		/// Point has been tagged by the controllers groove masking.
		/// </summary>
		IgnoreGroove1 = 0x40,

		/// <summary>
		/// Point has been tagged by the controllers groove masking.
		/// </summary>
		IgnoreGroove2 = 0x80,

		/// <summary>
		/// Der Taster hatte keinen Kontakt zum Material.
		/// </summary>
		AirScanning = 0x100,

		/// <summary>
		/// A calculated point.
		/// </summary>
		Virtual = 0x200,

		/// <summary>
		/// Point marks the beginning or end of a gap.
		/// </summary>
		Gap = 0x400,

		/// <summary>
		/// All states
		/// </summary>
		All = Outlier | Overlap | TiltedForce | TiltedAcceleration | EvaluationRange | Duplicate | IgnoreGroove1 | IgnoreGroove2 | AirScanning
	}
}