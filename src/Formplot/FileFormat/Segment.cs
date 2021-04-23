#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Collections.Generic;

	#endregion

	/// <summary>
	/// Represents a segment of points.
	/// </summary>
	public sealed class Segment<TPoint,TGeometry> : Segment, IEquatable<Segment<TPoint,TGeometry>>
		where TPoint : Point<TPoint,TGeometry>, new()
		where TGeometry : Geometry, new()
	{
		#region constructors

		/// <summary>Constructor.</summary>
		/// <param name="name">The name.</param>
		/// <param name="segmentType">Type of the segment.</param>
		public Segment( string name, SegmentTypes segmentType ) : base( name, segmentType )
		{
			Points = new PointCollection<TPoint, TGeometry>( this );
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the points
		/// </summary>
		public new PointCollection<TPoint,TGeometry> Points { get; }

		/// <summary>
		/// The plot to which this segment belongs.
		/// </summary>
		public new Formplot<TPoint,TGeometry>? Formplot { get; internal set; }

		/// <inheritdoc />
		internal override Formplot? AbstractFormplot => Formplot;

		/// <inheritdoc />
		internal override IReadOnlyCollection<Point> AbstractPoints => Points;

		#endregion

		#region methods

		private static bool Equals( Segment<TPoint,TGeometry> a, Segment<TPoint,TGeometry> b )
		{
			return Segment.Equals( a, b );
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion

		#region interface IEquatable<Segment<TPoint>>

		/// <inheritdoc />
		public bool Equals( Segment<TPoint,TGeometry> other )
		{
			return Equals( this, other );
		}

		#endregion
	}

	/// <summary>
	/// Represents a segment of points.
	/// </summary>
	public abstract class Segment : IEquatable<Segment>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		/// <param name="name">The name.</param>
		/// <param name="segmentType">Type of the segment.</param>
		protected Segment( string name, SegmentTypes segmentType )
		{
			Name = name;
			SegmentType = segmentType;
		}

		#endregion

		#region properties

		internal abstract Formplot? AbstractFormplot { get; }

		internal abstract IReadOnlyCollection<Point> AbstractPoints { get; }

		/// <summary>
		/// Gets the points.
		/// </summary>
		public IReadOnlyCollection<Point> Points => AbstractPoints;

		/// <summary>
		/// The plot to which this segment belongs.
		/// </summary>
		public Formplot? Formplot => AbstractFormplot;

		/// <summary>
		/// Gets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the type.
		/// </summary>
		public SegmentTypes SegmentType { get; }

		/// <summary>
		/// Gets the position of this segment. It's only set when converting from another plot type.
		/// </summary>
		public double Position { get; internal set; }

		#endregion

		#region methods

		private static bool Equals( Segment? a, Segment? b )
		{
			if( ReferenceEquals( a, b ) )
				return true;

			if( a != null && b != null )
				return a.SegmentType == b.SegmentType && Equals( a.Name, b.Name );

			return false;
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			return Equals( this, obj as Segment );
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ SegmentType.GetHashCode();
		}

		#endregion

		#region interface IEquatable<Segment>

		/// <inheritdoc />
		public bool Equals( Segment? other )
		{
			return Equals( this, other );
		}

		#endregion
	}
}