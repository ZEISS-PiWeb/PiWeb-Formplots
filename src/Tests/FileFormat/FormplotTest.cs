#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.Tests.FileFormat
{
	#region usings
	
	using System;
	using System.IO;
	using System.Linq;
	using NUnit.Framework;
	using Zeiss.PiWeb.Formplot.FileFormat;

	#endregion

	[TestFixture, Parallelizable( ParallelScope.All )]
	public class FormplotTest
	{
		#region members

		private static readonly string TestDataDirectory = Path.Combine( TestContext.CurrentContext.TestDirectory, "TestData");
		private static readonly string[] AllFormplotFiles = Directory.GetFiles( TestDataDirectory, "*" + MimeTypes.FormplotFileExtension, SearchOption.AllDirectories );
		private static readonly string[] AllPropertyFiles = Directory.GetFiles( TestDataDirectory, "*" + MimeTypes.PropertiesFileExtension, SearchOption.AllDirectories );

		private static readonly Tolerance[] TestTolerances =
		{
			new(),
			new( 77 ),
			new( null, 77 ),
			new() { ToleranceType = ToleranceType.Spatial, SpatialTolerance = new Vector( 1, 2, 3 ) },
			new() { ToleranceType = ToleranceType.Rectangular, RectangleToleranceHeight = 2, RectangleToleranceWidth = 3 },
			new() { ToleranceType = ToleranceType.Circular, CircularToleranceRadius = 7.5 },
			new( 12, 13 ) { WarningLevel = 99 },
			new( 12, 13 ) { WarningLevel = 12 },
			new( 12, 13 ) { RectangleToleranceWidth = 134 },
			new( 12, 13 ) { RectangleToleranceHeight = 134 },
			new( 12, 13 ) { RectangleToleranceWidth = 234, RectangleToleranceHeight = 134 },
			new( 12, 13 ) { SpatialTolerance = new Vector( 1, 2, 3 ) }
		};

		#endregion

		#region methods

		[Test, TestCaseSource( nameof( AllFormplotFiles ) )]
		public void BatchTest_FormplotFiles( string formplotFile )
		{
			using var stream = File.OpenRead( formplotFile );
			Assert.That( Formplot.ReadFrom( stream ), Is.Not.Null );
		}

		[Test, TestCaseSource( nameof( AllPropertyFiles ) )]
		public void BatchTest_PropertyFiles( string propertyFile )
		{
			using var stream = File.OpenRead( propertyFile );
			Assert.That( Formplot.ReadFrom( stream ), Is.Not.Null );
		}

		[Test]
		public void Test_Property_Serialization()
		{
			var i = 0;
			var plot = new EmptyPlot();
			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			CompareProperties( plot.Properties, clone.Properties );
		}

		[Test]
		public void Test_BasicProperties_Serialization()
		{
			var plot = new StraightnessPlot
			{
				CreatorSoftware = "Bla",
				CreatorSoftwareVersion = new Version( 1, 2, 3, 4 ),
				Tolerance = new Tolerance( -3, 8 ),
				ProjectionAxis = ProjectionAxis.Y,
				DefaultErrorScaling = 17,
				PreserveAspectRatio = true
			};

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			CompareBasicProperties( clone, plot );
		}

		[Test, TestCaseSource( nameof( TestTolerances ) )]
		public void Test_Tolerance_Serialization( Tolerance tolerance )
		{
			var plot = new DefectPlot { Tolerance = tolerance };
			var clone = plot.Clone();
		
			Assert.That( clone.Tolerance, Is.EqualTo( tolerance ) );
		}

		[Test]
		public void SerializationSmokeTest_DefectPlot()
		{
			var plot = new DefectPlot();
			var segment = new Segment<Defect, DefectGeometry>( "Segment", SegmentTypes.None );

			var voxel = new Voxel( new Vector( 1, 2, 3 ), new Vector( 4, 5, 6 ) );
			var point = new Defect( new Vector( 1, 2, 3 ), new Vector( 4, 5, 6 ), new[] { voxel } )
			{
				State = PointState.Gap | PointState.Outlier
			};

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_StraightnessPlot()
		{
			var plot = new StraightnessPlot();
			var segment = new Segment<LinePoint, LineGeometry>( "Segment", SegmentTypes.Line );

			var point = new LinePoint( 12, 19 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_RoundnessPlot()
		{
			var plot = new RoundnessPlot();
			var segment = new Segment<CirclePoint, CircleGeometry>( "Segment", SegmentTypes.Circle );

			var point = new CirclePoint( 12, 19 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_FlatnessPlot()
		{
			var plot = new FlatnessPlot();
			var segment = new Segment<PlanePoint, PlaneGeometry>( "Segment", SegmentTypes.Line );

			var point = new PlanePoint( 12, 19, 13 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_CurveProfilePlot()
		{
			var plot = new CurveProfilePlot();
			var segment = new Segment<CurvePoint, CurveGeometry>( "Segment", SegmentTypes.Line );

			var point = new CurvePoint( new Vector( 1, 2, 3 ), new Vector( 4, 5, 6 ), 13 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_BorePatternPlot()
		{
			var plot = new BorePatternPlot();
			var segment = new Segment<CurvePoint, CurveGeometry>( "Segment", SegmentTypes.Line );

			var point = new CurvePoint( new Vector( 1, 2, 3 ), new Vector( 4, 5, 6 ), 13 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_FilletPlot()
		{
			var plot = new FilletPlot();
			var segment = new Segment<FilletPoint, FilletGeometry>( "Segment", SegmentTypes.None );

			var point = new FilletPoint( new Vector( 1, 2, 3 ), new Vector( 4, 5, 6 ), 13 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_CircleInProfilePlot()
		{
			var plot = new CircleInProfilePlot();
			var segment = new Segment<CircleInProfilePoint, CircleInProfileGeometry>( "Segment", SegmentTypes.Line );

			var point = new CircleInProfilePoint( 2.5, -23 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_FlushGapPlot()
		{
			var plot = new FlushGapPlot();
			var segment = new Segment<FlushGapPoint, FlushGapGeometry>( "Segment", SegmentTypes.BendingCircle );

			var point = new FlushGapPoint( new Vector( 1, 2, 3 ), new Vector( 4, 5, 6 ), 12.3 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_FourierPlot()
		{
			var plot = new FourierPlot();
			var segment = new Segment<FourierPoint, EmptyGeometry>( "Segment", SegmentTypes.None );

			var point = new FourierPoint( 23456, 12.7 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_CylindricityPlot()
		{
			var plot = new CylindricityPlot();
			var segment = new Segment<CylinderPoint, CylinderGeometry>( "Segment", SegmentTypes.Helix );

			var point = new CylinderPoint( 17, 12, 13 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		[Test]
		public void SerializationSmokeTest_PitchPlot()
		{
			var plot = new PitchPlot();
			var segment = new Segment<PitchPoint, PitchGeometry>( "Segment", SegmentTypes.Line );

			var point = new PitchPoint( -99 );

			plot.Segments.Add( segment );
			segment.Points.Add( point );

			GenerateExampleProperties( plot );

			var clone = plot.Clone();
			ComparePlot( clone, plot );

			var clonePoint = clone.Points.FirstOrDefault();
			ComparePoint( clonePoint, point );
		}

		private static void ComparePlot( Formplot actual, Formplot expected )
		{
			CompareBasicProperties( actual, expected );
			CompareProperties( expected.Properties, actual.Properties );
			CompareSegment( expected.Segments.FirstOrDefault(), actual.Segments.FirstOrDefault() );
		}

		private static void CompareBasicProperties( Formplot actual, Formplot expected )
		{
			Assert.That( actual.CreatorSoftware, Is.EqualTo( expected.CreatorSoftware ) );
			Assert.That( actual.CreatorSoftwareVersion, Is.EqualTo( expected.CreatorSoftwareVersion ) );
			Assert.That( actual.PreserveAspectRatio, Is.EqualTo( expected.PreserveAspectRatio ) );
			Assert.That( actual.FormplotType, Is.EqualTo( expected.FormplotType ) );
			Assert.That( actual.OriginalFormplotType, Is.EqualTo( expected.OriginalFormplotType ) );
			Assert.That( actual.DefaultErrorScaling, Is.EqualTo( expected.DefaultErrorScaling ) );
			Assert.That( actual.PreserveAspectRatio, Is.EqualTo( expected.PreserveAspectRatio ) );
			Assert.That( actual.Tolerance, Is.EqualTo( expected.Tolerance ) );
		}

		private static void ComparePoint( Point actual, Point expected )
		{
			Assert.That( actual, Is.Not.Null );
			Assert.That( actual.Segment, Is.Not.Null );
			Assert.That( actual.State, Is.EqualTo( expected.State ) );
			Assert.That( actual.Index, Is.EqualTo( expected.Index ) );
			Assert.That( actual.Tolerance, Is.EqualTo( expected.Tolerance ) );

			CompareProperties( actual.Properties, expected.Properties );
		}

		private static void ComparePoint( CircleInProfilePoint actual, CircleInProfilePoint expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual.Angle, Is.EqualTo( expected.Angle ) );
			Assert.That( actual.Deviation, Is.EqualTo( expected.Deviation ) );
		}

		private static void ComparePoint( FlushGapPoint actual, FlushGapPoint expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual.Deviation, Is.EqualTo( expected.Deviation ) );
		}

		private static void ComparePoint( PitchPoint actual, PitchPoint expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual.Deviation, Is.EqualTo( expected.Deviation ) );
		}

		private static void ComparePoint( FourierPoint actual, FourierPoint expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual.Amplitude, Is.EqualTo( expected.Amplitude ) );
			Assert.That( actual.Harmonic, Is.EqualTo( expected.Harmonic ) );
		}

		private static void ComparePoint( Defect actual, Defect expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual, Is.Not.Null );
			Assert.That( actual.Segment, Is.Not.Null );
			Assert.That( actual.Position, Is.EqualTo( expected.Position ) );
			Assert.That( actual.Size, Is.EqualTo( expected.Size ) );
			Assert.That( actual.State, Is.EqualTo( expected.State ) );

			var cloneVoxel = actual.Voxels.FirstOrDefault();
			var voxel = expected.Voxels.FirstOrDefault();

			Assert.That( cloneVoxel, Is.Not.Null );
			Assert.That( cloneVoxel.Position, Is.EqualTo( voxel.Position ) );
			Assert.That( cloneVoxel.Size, Is.EqualTo( voxel.Size ) );
		}

		private static void ComparePoint( LinePoint actual, LinePoint expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual, Is.Not.Null );
			Assert.That( actual.Segment, Is.Not.Null );
			Assert.That( actual.Position, Is.EqualTo( expected.Position ) );
			Assert.That( actual.Deviation, Is.EqualTo( expected.Deviation ) );
		}

		private static void ComparePoint( FilletPoint actual, FilletPoint expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual.Position, Is.EqualTo( expected.Position ) );
			Assert.That( actual.Direction, Is.EqualTo( expected.Direction ) );
			Assert.That( actual.Deviation, Is.EqualTo( expected.Deviation ) );
		}

		private static void ComparePoint( CurvePoint actual, CurvePoint expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual.Position, Is.EqualTo( expected.Position ) );
			Assert.That( actual.Direction, Is.EqualTo( expected.Direction ) );
			Assert.That( actual.Deviation, Is.EqualTo( expected.Deviation ) );
		}

		private static void ComparePoint( CirclePoint actual, CirclePoint expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual.Angle, Is.EqualTo( expected.Angle ) );
			Assert.That( actual.Deviation, Is.EqualTo( expected.Deviation ) );
		}

		private static void ComparePoint( PlanePoint actual, PlanePoint expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual.Coordinate1, Is.EqualTo( expected.Coordinate1 ) );
			Assert.That( actual.Coordinate2, Is.EqualTo( expected.Coordinate2 ) );
			Assert.That( actual.Deviation, Is.EqualTo( expected.Deviation ) );
		}

		private static void ComparePoint( CylinderPoint actual, CylinderPoint expected )
		{
			ComparePoint( actual, (Point)expected );

			Assert.That( actual.Angle, Is.EqualTo( expected.Angle ) );
			Assert.That( actual.Height, Is.EqualTo( expected.Height ) );
			Assert.That( actual.Deviation, Is.EqualTo( expected.Deviation ) );
		}

		private static void CompareSegment( Segment expected, Segment actual  )
		{
			Assert.That( actual, Is.Not.Null );
			Assert.That( actual.Name, Is.EqualTo( expected.Name ) );
			Assert.That( actual.SegmentType, Is.EqualTo( expected.SegmentType ) );
			Assert.That( actual.Position, Is.EqualTo( expected.Position ) );
			Assert.That( actual.Points, Has.Exactly( expected.Points.Count ).Items );
		}
		private static void CompareProperties( PropertyCollection expectedProperties, PropertyCollection actualProperties )
		{
			foreach( var expected in expectedProperties )
			{
				var actual = actualProperties[ expected.Name ];

				Assert.That( actual, Is.Not.Null );
				Assert.That( actual.Name, Is.EqualTo( expected.Name ) );
				Assert.That( actual.Description, Is.EqualTo( expected.Description ) );
				Assert.That( actual.Value, Is.EqualTo( expected.Value ) );
			}
		}

		private static void GenerateExampleProperties( Formplot plot )
		{
			plot.Properties.Add( Property.Create( "TestLong", 0, "Test-Long" ) );
			plot.Properties.Add( Property.Create( "TestDouble", 0.0, "Test-Double" ) );
			plot.Properties.Add( Property.Create( "TestString", "String", "Test-String" ) );
			plot.Properties.Add( Property.Create( "TestDate", new DateTime( 2020, 9, 4, 10, 40, 12 ), "Test-Date" ) );
			plot.Properties.Add( Property.Create( "TestTimespan", TimeSpan.FromMinutes( 12.456 ), "Test-Timespan" ) );
		}

		#endregion
	}
}