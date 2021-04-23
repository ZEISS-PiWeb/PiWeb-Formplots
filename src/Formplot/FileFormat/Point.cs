#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System.IO;

	#endregion

	/// <summary>
	/// Describes a point.
	/// </summary>
	public abstract class Point<TPoint, TGeometry> : Point
		where TPoint : Point<TPoint, TGeometry>, new()
		where TGeometry : Geometry, new()
	{
		#region properties

		/// <summary>
		/// Gets the segment this point belongs to.
		/// </summary>
		public new Segment<TPoint, TGeometry>? Segment { get; internal set; }

		internal override Segment? AbstractSegment => Segment;

		#endregion
	}

	/// <summary>
	/// Describes a point.
	/// </summary>
	public abstract class Point
	{
		#region members

		private Tolerance? _Tolerance;
		private PropertyCollection? _Properties;

		#endregion

		#region properties

		internal abstract Segment? AbstractSegment { get; }

		/// <summary>
		/// Gets the metadata.
		/// </summary>
		public PropertyCollection Properties
		{
			get => _Properties ??= new PropertyCollection();
			set => _Properties = value;
		}

		/// <summary>
		/// Gets the current state.
		/// </summary>
		public PointState State { get; set; }

		/// <summary>
		/// Gets the segment this point belongs to.
		/// </summary>
		public Segment? Segment => AbstractSegment;

		/// <summary>
		/// Gets or sets the tolerance of this point.
		/// </summary>
		public Tolerance Tolerance
		{
			get => _Tolerance ??= new Tolerance();
			set => _Tolerance = value;
		}

		/// <summary>
		/// Returns <code>true</code> if this point has a tolerance and the tolerance is not empty.
		/// </summary>
		public bool HasTolerance => _Tolerance != null && !_Tolerance.IsEmpty;

		/// <summary>
		/// The point index
		/// </summary>
		public int Index { get; internal set; }

		#endregion

		#region methods

		/// <summary>
		/// Writes the point into a binary data stream.
		/// </summary>
		internal abstract void WriteToStream( BinaryWriter writer );

		/// <summary>
		/// Reads the point from a binary data stream
		/// </summary>
		internal abstract void ReadFromStream( BinaryReader reader );

		#endregion
	}
}