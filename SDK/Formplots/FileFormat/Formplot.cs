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
	/// Representation of a formplot.
	/// </summary>
	public abstract class Formplot
	{
		#region members

		private ICollection<Property> _Properties = new List<Property>();
		private Tolerance _Tolerance = new Tolerance();
		private double? _DefaultErrorScaling;
		private Geometry _Nominal;
		private Geometry _Actual;
		private Point[] _Points = new Point[0];

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Formplot"/> class.
		/// </summary>
		/// <param name="formplotType">Type of the formplot.</param>
		protected Formplot( FormplotTypes formplotType )
		{
			FormplotType = formplotType;
			GeometryType = Geometry.GetGeometryTypeFromFormplotType( formplotType );

			if( GeometryType != GeometryTypes.None )
			{
				_Nominal = Geometry.Create( GeometryType );
				_Actual = Geometry.Create( GeometryType );
			}
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the type of the geometry used by this plot.
		/// </summary>
		public GeometryTypes GeometryType { get; }

		/// <summary>
		/// Gets the type of the formplot.
		/// </summary>
		public FormplotTypes FormplotType { get; }

		/// <summary>
		/// Gets or sets the name of the software, which has written the formplot data.
		/// </summary>
		public string CreatorSoftware { get; set; } = "unknown";

		/// <summary>
		/// Gets or sets the version of the software, which has written the formplot data.
		/// </summary>
		public Version CreatorSoftwareVersion { get; set; } = new Version( 0, 0 );

		/// <summary>
		/// Gets or sets the metadata.
		/// </summary>
		public ICollection<Property> Properties
		{
			get { return _Properties; }
			set { _Properties = value ?? new List<Property>(); }
		}

		/// <summary>
		/// Gets or sets the global tolerance.
		/// </summary>
		public Tolerance Tolerance
		{
			get { return _Tolerance; }
			set { _Tolerance = value ?? new Tolerance(); }
		}

		/// <summary>
		/// Gets or sets the default error scaling.
		/// </summary>
		public double? DefaultErrorScaling
		{
			get { return _DefaultErrorScaling; }
			set { _DefaultErrorScaling = value.HasValue && value.Value > 0.0 ? value : null; }
		}

		/// <summary>
		/// Gets or sets the projection axis.
		/// </summary>
		public ProjectionAxis ProjectionAxis { get; set; }

		/// <summary>
		/// Gets or sets the nominal geometry.
		/// </summary>
		public Geometry Nominal
		{
			get { return _Nominal; }
			set
			{
				if( value != null )
				{
					var t = Geometry.Create( GeometryType )?.GetType();

					if( value.GetType() != t )
					{
						throw new ArgumentException( $"geometry must be type \"{t}\"" );
					}
				}

				_Nominal = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the actual geometry.
		/// </summary>
		public Geometry Actual
		{
			get { return _Actual; }
			set
			{
				if( value != null )
				{
					var t = Geometry.Create( GeometryType )?.GetType();

					if( value.GetType() != t )
					{
						throw new ArgumentException( $"geometry must be type \"{t}\"" );
					}
				}

				_Actual = value;
			}
		}

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public IEnumerable<Point> Points
		{
			get { return _Points; }
			set
			{
				if( value != null )
				{
					var t = Point.GetPointType( FormplotType );

					if( value.Any( p => p.GetType() != t ) )
					{
						throw new ArgumentException( $"All points must be type \"{t}\"" );
					}
				}

				_Points = value?.ToArray() ?? new Point[0];
			}
		}

		#endregion

		#region methods

		/// <summary>
		/// Creates a new <see cref="FileFormat.Formplot"/> instance from the specified <paramref name="stream"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="NotSupportedException"></exception>
		public static Formplot ReadFrom( Stream stream )
		{
			return FormplotHelpers.ReadFrom( stream );
		}

		/// <summary>
		/// Creates a formplot with the specified type.
		/// </summary>
		/// <param name="type">The formplot type.</param>
		/// <returns></returns>
		public static Formplot Create( FormplotTypes type )
		{
			switch( type )
			{
				case FormplotTypes.None:
					return new EmptyPlot();
				case FormplotTypes.Roundness:
					return new RoundnessPlot();
				case FormplotTypes.Flatness:
					return new FlatnessPlot();
				case FormplotTypes.CurveProfile:
					return new CurveProfilePlot();
				case FormplotTypes.Straightness:
					return new StraightnessPlot();
				case FormplotTypes.Cylindricity:
					return new CylindricityPlot();
				case FormplotTypes.Pitch:
					return new PitchPlot();
				case FormplotTypes.BorePattern:
					return new BorePatternPlot();
				case FormplotTypes.CircleInProfile:
					return new CircleInProfilePlot();
				default:
					throw new ArgumentOutOfRangeException( nameof( type ), type, null );
			}
		}

		#endregion
	}
}