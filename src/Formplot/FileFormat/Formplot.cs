#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	#endregion

	/// <summary>
	/// Base class for all kinds of form plots.
	/// </summary>
	public abstract class Formplot
	{
		#region members

		internal static readonly Version Version3 = new Version( 3, 0 );

		private double? _DefaultErrorScaling;

		#endregion

		#region constructors

		/// <summary>Constructor.</summary>
		protected Formplot( FormplotTypes formplotType )
		{
			FormplotType = formplotType;
			OriginalFormplotType = formplotType;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the type of the formplot.
		/// </summary>
		public FormplotTypes FormplotType { get; }

		/// <summary>
		/// Gets the type of the geometry.
		/// </summary>
		public abstract GeometryTypes GeometryType { get; }

		/// <summary>
		/// Gets the type of plot from which this plot has been converted.
		/// </summary>
		public FormplotTypes OriginalFormplotType { get; internal set; }

		/// <summary>
		/// Gets or sets the name of the software, which has written the formplot data.
		/// </summary>
		public string CreatorSoftware { get; set; } = "unknown";

		/// <summary>
		/// Gets or sets the version of the software, which has written the formplot data.
		/// </summary>
		public Version CreatorSoftwareVersion { get; set; } = new Version( 0, 0 );

		/// <summary>
		/// Gets the metadata.
		/// </summary>
		public PropertyCollection Properties { get; } = new PropertyCollection();

		/// <summary>
		/// Gets or sets the global tolerance.
		/// </summary>
		public Tolerance Tolerance { get; set; } = new Tolerance();

		/// <summary>
		/// Gets or sets the default error scaling.
		/// </summary>
		public double? DefaultErrorScaling
		{
			get => _DefaultErrorScaling;
			set => _DefaultErrorScaling = value.HasValue && value.Value > 0.0 ? value : null;
		}

		/// <summary>
		/// Gets or sets the default error scaling.
		/// </summary>
		public bool? PreserveAspectRatio
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the projection axis.
		/// </summary>
		public ProjectionAxis ProjectionAxis { get; set; } = ProjectionAxis.None;

		/// <summary>
		/// The list of segments of which this plot is composed.
		/// </summary>
		public IReadOnlyList<Segment> Segments => AbstractSegments;

		internal abstract IReadOnlyList<Segment> AbstractSegments { get; }

		/// <summary>
		/// Gets the actual geometry.
		/// </summary>
		public Geometry Actual => AbstractActual;

		internal abstract Geometry AbstractActual { get; }

		/// <summary>
		/// Gets the nominal geometry.
		/// </summary>
		public Geometry Nominal => AbstractNominal;

		internal abstract Geometry AbstractNominal { get; }

		#endregion

		#region methods

		/// <summary>
		/// Writes the formplot file content to the specified stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public abstract void WriteTo( Stream stream );

		/// <summary>
		/// Creates a clone of this form plot.
		/// </summary>
		/// <remarks>The clone is created by serilizing and deserializing the current plot.</remarks>
		public Formplot? Clone()
		{
			using var stream = new MemoryStream();

			WriteTo( stream );
			stream.Seek( 0, SeekOrigin.Begin );

			return ReadFrom( stream );
		}

		/// <summary>
		/// Reads a formplot from the specified stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="acceptedTypes">Types that should be processed further. Leave null to allow all types.</param>
		/// <returns></returns>
		public static Formplot? ReadFrom( Stream stream, FormplotTypes[]? acceptedTypes = null )
		{
			return FormplotReader.ReadFrom( stream, acceptedTypes );
		}

		/// <summary>
		/// Reads a formplot of a certain type from the specified stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="allowConversion"></param>
		/// <typeparam name="TPlot">The desired formplot type</typeparam>
		/// <returns>An instance of the desired formplot type or null.</returns>
		public static TPlot? ReadFrom<TPlot>( Stream stream, bool allowConversion = false ) where TPlot : Formplot, new()
		{
			if( !allowConversion )
			{
				var type = new TPlot().FormplotType;
				return ReadFrom( stream, new[] { type } ) as TPlot;
			}

			var untypedResult = FormplotReader.ReadFrom( stream );
			if( untypedResult is null )
				return null;

			var typedResult = untypedResult as TPlot;
			if( typedResult != null )
				return typedResult;

			return FormplotConverter.TryConvert( untypedResult, out typedResult ) ? typedResult : null;
		}

		#endregion
	}

	/// <summary>
	/// Representation of a formplot.
	/// </summary>
	public abstract class Formplot<TPoint, TGeometry> : Formplot
		where TPoint : Point<TPoint, TGeometry>, new()
		where TGeometry : Geometry, new()
	{
		#region constructors

		/// <summary>Constructor.</summary>
		/// <param name="formplotType">Type of the formplot.</param>
		protected Formplot( FormplotTypes formplotType ) : base( formplotType )
		{
			Segments = new SegmentCollection<TPoint, TGeometry>( this );

			Nominal = new TGeometry();
			Actual = new TGeometry();

			GeometryType = Actual.GeometryType;
		}

		#endregion

		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; }

		/// <summary>
		/// The list of segments of which this plot is composed.
		/// </summary>
		public new SegmentCollection<TPoint, TGeometry> Segments { get; }

		/// <summary>
		/// Gets or sets the plot points.
		/// </summary>
		public IEnumerable<TPoint> Points => Segments.SelectMany( s => s.Points );

		internal override IReadOnlyList<Segment> AbstractSegments => Segments;

		internal override Geometry AbstractNominal => Nominal;

		/// <summary>
		/// Gets or sets the nominal geometry.
		/// </summary>
		public new TGeometry Nominal { get; }

		internal override Geometry AbstractActual => Actual;

		/// <summary>
		/// Gets or sets the actual geometry.
		/// </summary>
		public new TGeometry Actual { get; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override void WriteTo( Stream stream )
		{
			FormplotWriter.WriteTo( this, stream );
		}

		/// <summary>
		/// Creates a clone of this form plot.
		/// </summary>
		/// <remarks>The clone is created by serilizing and deserializing the current plot.</remarks>
		public new Formplot<TPoint, TGeometry>? Clone()
		{
			return (Formplot<TPoint, TGeometry>?)base.Clone();
		}

		#endregion
	}
}