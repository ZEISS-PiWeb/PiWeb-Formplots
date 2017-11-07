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

	using System;

	#endregion

	/// <summary>
	/// Represents a segment of points.
	/// </summary>
	public class Segment : IEquatable<Segment>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Segment"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="segmentType">Type of the segment.</param>
		public Segment( string name, SegmentTypes segmentType )
		{
			Name = name;
			SegmentType = segmentType;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the type.
		/// </summary>
		public SegmentTypes SegmentType { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Creates a new <see cref="Segment"/> instance.
		/// </summary>
		public static Segment Create( string name, SegmentTypes segmentType = SegmentTypes.None )
		{
			return new Segment( name, segmentType );
		}

		/// <summary>
		/// Determines, whether the specified <see cref="Segment"/> instances are equal.
		/// </summary>
		private static bool Equals( Segment a, Segment b )
		{
			if( ReferenceEquals( a, b ) )
			{
				return true;
			}
			else if( a != null && b != null )
			{
				return a.SegmentType == b.SegmentType &&
				       Equals( a.Name, b.Name );
			}

			return false;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals( object obj )
		{
			return Equals( this, obj as Segment );
		}


		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ SegmentType.GetHashCode();
		}

		#endregion

		#region interface IEquatable<Segment>

		/// <summary>
		/// Determines whether the specified <see cref="Segment" />, is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="Segment" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="Segment" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals( Segment other )
		{
			return Equals( this, other );
		}

		#endregion
	}
}