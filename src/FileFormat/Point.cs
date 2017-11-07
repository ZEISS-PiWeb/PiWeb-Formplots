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
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	#endregion

	/// <summary>
	/// Describes a point.
	/// </summary>
	public abstract class Point
	{
		#region members

		private Property[] _PropertyList = new Property[ 0 ];
		private Tolerance _Tolerance = new Tolerance();

		#endregion

		#region constructors

		internal Point()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Point"/> class.
		/// </summary>
		/// <param name="segment">The segment.</param>
		protected internal Point( Segment segment )
		{
			Segment = segment;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the metadata.
		/// </summary>
		public IEnumerable<Property> PropertyList
		{
			get { return _PropertyList; }
			set { _PropertyList = value?.ToArray() ?? new Property[ 0 ]; }
		}

		/// <summary>
		/// Gets the current state.
		/// </summary>
		public PointState State { get; set; }

		/// <summary>
		/// Gets or sets the segment this point belongs to.
		/// </summary>
		public Segment Segment { get; set; }


		/// <summary>
		/// Gets or sets the tolerance of this point.
		/// </summary>
		public Tolerance Tolerance
		{
			get { return _Tolerance; }
			set { _Tolerance = value ?? new Tolerance(); }
		}

		#endregion

		#region methods

		internal static Type GetPointType( FormplotTypes formplotType )
		{
			switch( formplotType )
			{
				case FormplotTypes.Roundness:
					return typeof( CirclePoint );
				case FormplotTypes.Flatness:
					return typeof( PlanePoint );
				case FormplotTypes.CurveProfile:
					return typeof( CurvePoint );
				case FormplotTypes.Straightness:
					return typeof( LinePoint );
				case FormplotTypes.Cylindricity:
					return typeof( CylinderPoint );
				case FormplotTypes.Pitch:
					return typeof( PitchPoint );
				case FormplotTypes.BorePattern:
					return typeof( BorePatternPoint );
				case FormplotTypes.CircleInProfile:
					return typeof( CircleInProfilePoint );
				case FormplotTypes.Fourier:
					return typeof( FourierPoint );
				default:
					throw new ArgumentOutOfRangeException( nameof( formplotType ), formplotType, null );
			}
		}

		/// <summary>
		/// Returns a point for a specified formplot type.
		/// </summary>
		internal static Point Create( FormplotTypes formplotType )
		{
			switch( formplotType )
			{
				case FormplotTypes.Roundness:
					return new CirclePoint();
				case FormplotTypes.Flatness:
					return new PlanePoint();
				case FormplotTypes.CurveProfile:
					return new CurvePoint();
				case FormplotTypes.Straightness:
					return new LinePoint();
				case FormplotTypes.Cylindricity:
					return new CylinderPoint();
				case FormplotTypes.Pitch:
					return new PitchPoint();
				case FormplotTypes.BorePattern:
					return new BorePatternPoint();
				case FormplotTypes.CircleInProfile:
					return new CircleInProfilePoint();
				case FormplotTypes.Fourier:
					return new FourierPoint();
				default:
					throw new ArgumentOutOfRangeException( nameof( formplotType ), formplotType, null );
			}
		}

		/// <summary>
		/// Writes the point into a binary data stream.
		/// </summary>
		internal abstract void WriteToStream( Stream stream);

		/// <summary>
		/// Reads the point from a binary data stream
		/// </summary>
		internal abstract void ReadFromStream( Stream stream, int index );

		/// <summary>
		/// Reads the bytes to create a new point.
		/// </summary>
		protected byte[] GetBuffer( Stream stream, int length )
		{
			if( stream == null )
			{
				throw new ArgumentNullException( nameof( stream ) );
			}

			var buffer = new byte[ length ];
			var readedbytes = stream.Read( buffer, 0, buffer.Length );

			if( readedbytes < buffer.Length )
			{
				throw new EndOfStreamException( $"Not enough bytes in stream for read point type \"{GetType()}\"" );
			}

			return buffer;
		}

		#endregion
	}
}