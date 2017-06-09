#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.NGPS.Serialization
{
	#region usings

	using ICSharpCode.SharpZipLib.Zip;
	using NUnit.Framework;

	using System;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Xml;
	using Zeiss.IMT.Collections;
	using Zeiss.IMT.Globals;
	using Zeiss.IMT.NGPS.Element.Formplots.Data.FileFormat;
	using Zeiss.IMT.NGPS.Globals;
	using Zeiss.IMT.NGPS.Tests;
	using Zeiss.IMT.PiWeb.Common.Util;
	using Zeiss.IMT.PiWeb.Formplot.FileFormat;
	using Zeiss.IMT.Qdb;
	using Property = Zeiss.IMT.PiWeb.Formplot.FileFormat.Property;
	using Segment = Zeiss.IMT.PiWeb.Formplot.FileFormat.Segment;
	using Tolerance = Zeiss.IMT.PiWeb.Formplot.FileFormat.Tolerance;

	#endregion

	[TestFixture]
	[Category( WellKnownTestCategories.Rolling )]
	public class FormplotTest
	{
		#region methods

		[Test]
		public void Test01_EmptyPlot()
		{
			var x = XmlConvert.ToString( TimeSpan.Zero );
			x = XmlConvert.ToString( TimeSpan.MinValue );
			x = XmlConvert.ToString( TimeSpan.MaxValue );

			var plot = new EmptyPlot
			{
				Properties = new[]
				{
					Property.Create( "BeliebigerName", "Beliebiger Text", "Beschreibung" ),
					Property.Create( "BeliebigerIntegerWert", 42 ),
					Property.Create( "BeliebigesDatum", new DateTime( 2012, 12, 21, 12, 02, 22, 123, CultureInfo.CurrentCulture.Calendar, DateTimeKind.Local ) ),
					Property.Create( "BeliebigeZeitspanne", TimeSpan.FromTicks( 162351234 ) ),
					Property.Create( "BeliebigerDoubleWert", Math.PI )
				}
			};


			TestPlot( plot );
		}

		[Test]
		public void Test02_RoundnessFormplot()
		{
			var points = new ReadOnlyList<CirclePoint>( new[]
			{
				new CirclePoint(Segment.Create( "Segment 001" ),1.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CirclePoint(Segment.Create( "Segment 001" ),2.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CirclePoint(Segment.Create( "Segment 001" ),3.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CirclePoint(Segment.Create( "Segment 001" ),4.1, 0.15) { State = PointState.Outlier | PointState.Overlap },
				new CirclePoint(Segment.Create( "Segment 001" ),5.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( null, -0.1 ) },
				new CirclePoint(Segment.Create( "Segment 001" ),6.1, 0.124) { State = PointState.Outlier },
				new CirclePoint(Segment.Create( "Segment 001" ),7.1, 0.124) { State = PointState.Outlier },
				new CirclePoint(Segment.Create( "Segment 001" ),8.1, 0.124) { State = PointState.Outlier },
				new CirclePoint(Segment.Create( "Segment 001" ),9.1, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new CirclePoint(Segment.Create( "Segment 001" ),10.1, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new CirclePoint(Segment.Create( "Segment 001" ),11.1, 0.124),
				new CirclePoint(Segment.Create( "Segment 001" ),12.1, 0.124) { Tolerance = new Tolerance( -0.3, 0.5 ) },
				new CirclePoint(Segment.Create( "Segment 001" ),13.1, 0.124),
				new CirclePoint(Segment.Create( "Segment 001" ),14.1, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) }
			} );

			var plot = new RoundnessPlot()
			{
				Properties = new[]
				{
					Property.Create( "BeliebigerName", "Beliebiger Text" ),
					Property.Create( "BeliebigerIntegerWert", 42 ),
					Property.Create( "BeliebigesDatum", new DateTime( 2012, 12, 21, 12, 02, 22, 123, CultureInfo.CurrentCulture.Calendar, DateTimeKind.Local ) ),
					Property.Create( "BeliebigeZeitspanne", TimeSpan.FromTicks( 162351234 ) ),
					Property.Create( "BeliebigerDoubleWert", Math.PI )
				},
				DefaultErrorScaling = Math.PI,
				Tolerance = new Tolerance( -0.5, 0.5 ),
				Points = points,
				Actual = new CircleGeometry
				{
					CoordinateSystem = new CoordinateSystem
					{
						Origin = new Vector { X = 2001.1548, Y = 107.664, Z = 5.159 },
						Axis1 = new Vector { X = 1.456, Y = 0.158, Z = 1.0 / 7.0 },
						Axis2 = new Vector { X = 0.589, Y = 1.256, Z = 0.98465 },
						Axis3 = new Vector { X = 0.6465846, Y = 0.65465, Z = 1.46548 }
					},
					Radius = 1
				}
			};

			TestPlot( plot );
		}

		[Test]
		public void Test03_FlatnessFormplot()
		{
			var points = new ReadOnlyList<PlanePoint>( new[]
			{
				new PlanePoint(Segment.Create( "Segment 001" ),2.1, 1.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new PlanePoint(Segment.Create( "Segment 001" ),2.1, 2.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new PlanePoint(Segment.Create( "Segment 001" ),2.1, 3.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new PlanePoint(Segment.Create( "Segment 001" ),3.1, 4.1, 0.15) { State = PointState.Outlier | PointState.Overlap },
				new PlanePoint(Segment.Create( "Segment 001" ),3.1, 5.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( null, -0.1 ) },
				new PlanePoint(Segment.Create( "Segment 001" ),3.1, 6.1, 0.124) { State = PointState.Outlier },
				new PlanePoint(Segment.Create( "Segment 001" ),4, 7.1, 0.124) { State = PointState.Outlier },
				new PlanePoint(Segment.Create( "Segment 001" ),4, 8.1, 0.124) { State = PointState.Outlier },
				new PlanePoint(Segment.Create( "Segment 001" ),4, 9.1, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new PlanePoint(Segment.Create( "Segment 001" ),4, 10.1, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new PlanePoint( Segment.Create( "Segment 001" ),7, 11.1, 0.124),
				new PlanePoint( Segment.Create( "Segment 001" ),7, 12.1, 0.124) { Tolerance = new Tolerance( -0.3, 0.5 ) },
				new PlanePoint( Segment.Create( "Segment 001" ),7, 13.1, 0.124),
				new PlanePoint( Segment.Create( "Segment 001" ),7, 14.1, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) }
			} );

			var plot = new FlatnessPlot()
			{
				Properties = new[]
				{
					Property.Create( "BeliebigerName", "Beliebiger Text" ),
					Property.Create( "BeliebigerIntegerWert", 42 ),
					Property.Create( "BeliebigesDatum", new DateTime( 2012, 12, 21, 12, 02, 22, 123, CultureInfo.CurrentCulture.Calendar, DateTimeKind.Local ) ),
					Property.Create( "BeliebigeZeitspanne", TimeSpan.FromTicks( 162351234 ) ),
					Property.Create( "BeliebigerDoubleWert", Math.PI )
				},
				DefaultErrorScaling = 3.14159,
				Tolerance = new Tolerance( -0.3, 0.5 ),
				Points = points,
				Actual = new PlaneGeometry
				{
					CoordinateSystem = new CoordinateSystem
					{
						Origin = new Vector { X = 2001.1548, Y = 107.664, Z = 5.159 },
						Axis1 = new Vector { X = 1.456, Y = 0.158, Z = 1.0 / 7.0 },
						Axis2 = new Vector { X = 0.589, Y = 1.256, Z = 0.98465 },
						Axis3 = new Vector { X = 0.6465846, Y = 0.65465, Z = 1.46548 }
					},
					Length1 = 1,
					Length2 = 1
				}
			};

			TestPlot( plot );
		}

		[Test]
		public void Test04_CurveFormplot()
		{
			var points = new ReadOnlyList<CurvePoint>( new[]
			{
				new CurvePoint(Segment.Create( "Segment 001" ), new Vector( 2.1, 1.1, 0.15 ), new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CurvePoint(Segment.Create( "Segment 001" ), new Vector( 2.1, 2.1, 0.15 ), new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CurvePoint(Segment.Create( "Segment 001" ),new Vector( 2.1, 3.1, 0.15 ), new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CurvePoint(Segment.Create( "Segment 001" ),new Vector( 2.1, 4.1, 0.15 ), new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap },
				new CurvePoint(Segment.Create( "Segment 001" ),new Vector( 2.1, 5.1, 0.15 ), new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( null, -0.1 ) },
				new CurvePoint(Segment.Create( "Segment 001" ),new Vector( 3.1, 6.1, 0.124 ), new Vector { X = 1 }, 0.124) { State = PointState.Outlier },
				new CurvePoint(Segment.Create( "Segment 001" ),new Vector( 3.1, 7.1, 0.124 ), new Vector { X = 1 }, 0.124) { State = PointState.Outlier },
				new CurvePoint(Segment.Create( "Segment 001" ),new Vector( 3.1, 8.1, 0.124 ), new Vector { X = 1 }, 0.124) { State = PointState.Outlier },
				new CurvePoint(Segment.Create( "Segment 001" ),new Vector( 3.1, 9.1, 0.124 ), new Vector { X = 1 }, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new CurvePoint(Segment.Create( "Segment 001" ),new Vector( 4, 10.1, 0.124 ), new Vector { X = 1 }, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new CurvePoint( Segment.Create( "Segment 001" ),new Vector( 6, 11.1, 0.124 ), new Vector { X = 1 }, 0.124),
				new CurvePoint( Segment.Create( "Segment 001" ),new Vector( 7, 12.1, 0.124 ), new Vector { X = 1 }, 0.124) { Tolerance = new Tolerance( -0.3, 0.5 ) },
				new CurvePoint( Segment.Create( "Segment 001" ),new Vector( 7.1, 13.1, 0.124 ), new Vector { X = 1 }, 0.124),
				new CurvePoint( Segment.Create( "Segment 001" ),new Vector( 7.1, 14.1, 0.124 ), new Vector { X = 1 }, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) }
			} );

			var plot = new CurveProfilePlot()
			{
				Properties = new[]
				{
					Property.Create( "BeliebigerName", "Beliebiger Text" ),
					Property.Create( "BeliebigerIntegerWert", 42 ),
					Property.Create( "BeliebigesDatum", new DateTime( 2012, 12, 21, 12, 02, 22, 123, CultureInfo.CurrentCulture.Calendar, DateTimeKind.Local ) ),
					Property.Create( "BeliebigeZeitspanne", TimeSpan.FromTicks( 162351234 ) ),
					Property.Create( "BeliebigerDoubleWert", Math.PI )
				},
				DefaultErrorScaling = 3.14159,
				Tolerance = new Tolerance( -0.3, 0.5 ),
				Points = points,
				Actual = new CurveGeometry
				{
					CoordinateSystem = new CoordinateSystem
					{
						Origin = new Vector { X = 2001.1548, Y = 107.664, Z = 5.159 },
						Axis1 = new Vector { X = 1.456, Y = 0.158, Z = 1.0 / 7.0 },
						Axis2 = new Vector { X = 0.589, Y = 1.256, Z = 0.98465 },
						Axis3 = new Vector { X = 0.6465846, Y = 0.65465, Z = 1.46548 }
					}
				}
			};

			TestPlot( plot );
		}

		[Test]
		public void Test05_StraightnessFormplot()
		{
			var points = new ReadOnlyList<LinePoint>( new[]
			{
				new LinePoint(Segment.Create( "Segment 001" ),1.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new LinePoint(Segment.Create( "Segment 001" ),2.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new LinePoint(Segment.Create( "Segment 001" ),3.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new LinePoint(Segment.Create( "Segment 001" ),4.1, 0.15) { State = PointState.Outlier | PointState.Overlap },
				new LinePoint(Segment.Create( "Segment 001" ),5.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( null, -0.1 ) },
				new LinePoint(Segment.Create( "Segment 001" ),6.1, 0.124) { State = PointState.Outlier },
				new LinePoint(Segment.Create( "Segment 001" ),7.1, 0.124) { State = PointState.Outlier },
				new LinePoint(Segment.Create( "Segment 001" ),8.1, 0.124) { State = PointState.Outlier },
				new LinePoint(Segment.Create( "Segment 001" ),9.1, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new LinePoint(Segment.Create( "Segment 001" ),10.1, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new LinePoint(Segment.Create( "Segment 001" ),11.1, 0.124),
				new LinePoint(Segment.Create( "Segment 001" ),12.1, 0.124) { Tolerance = new Tolerance( -0.3, 0.5 ) },
				new LinePoint(Segment.Create( "Segment 001" ),13.1, 0.124),
				new LinePoint(Segment.Create( "Segment 001" ),14.1, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) }
			} );

			var plot = new StraightnessPlot
			{
				Properties = new[]
				{
					Property.Create( "BeliebigerName", "Beliebiger Text" ),
					Property.Create( "BeliebigerIntegerWert", 42 ),
					Property.Create( "BeliebigesDatum", new DateTime( 2012, 12, 21, 12, 02, 22, 123, CultureInfo.CurrentCulture.Calendar, DateTimeKind.Local ) ),
					Property.Create( "BeliebigeZeitspanne", TimeSpan.FromTicks( 162351234 ) ),
					Property.Create( "BeliebigerDoubleWert", Math.PI )
				},
				DefaultErrorScaling = 3.14159,
				Tolerance = new Tolerance( -0.3, 0.5 ),
				Points = points,
				Actual = new LineGeometry
				{
					CoordinateSystem = new CoordinateSystem
					{
						Origin = new Vector { X = 2001.1548, Y = 107.664, Z = 5.159 },
						Axis1 = new Vector { X = 1.456, Y = 0.158, Z = 1.0 / 7.0 },
						Axis2 = new Vector { X = 0.589, Y = 1.256, Z = 0.98465 },
						Axis3 = new Vector { X = 0.6465846, Y = 0.65465, Z = 1.46548 }
					}
				}
			};

			TestPlot( plot );
		}

		[Test]
		public void Test06_CylindricityFormplot()
		{
			var points = new ReadOnlyList<CylinderPoint>( new[]
			{

				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),1.1, 0.35, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),2.1, 0.35, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),3.1, 0.35, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),4.1, 0.35, 0.15) { State = PointState.Outlier | PointState.Overlap },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),5.1, 0.35, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( null, -0.1 ) },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),6.1, 0.35, 0.124) { State = PointState.Outlier },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),7.1, 0.35, 0.124) { State = PointState.Outlier },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),8.1, 0.35, 0.124) { State = PointState.Outlier },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),9.1, 0.35, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),10.1, 0.35, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),11.1, 0.35, 0.124),
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),12.1, 0.35, 0.124) { Tolerance = new Tolerance( -0.3, 0.5 ) },
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),13.1, 0.35, 0.124),
				new CylinderPoint(Segment.Create( "Segment 001", SegmentTypes.Circle ),14.1, 0.35, 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },

				new CylinderPoint(Segment.Create( "Segment 002", SegmentTypes.Axis ),0.25 * Math.PI, 0.9, 0.11),
				new CylinderPoint(Segment.Create( "Segment 002", SegmentTypes.Axis ),0.5 * Math.PI, 0.8, 0.21) ,
				new CylinderPoint(Segment.Create( "Segment 002", SegmentTypes.Axis ),0.75 * Math.PI, 0.7, -0.15) ,
				new CylinderPoint(Segment.Create( "Segment 002", SegmentTypes.Axis ), Math.PI, 0.35, 0.01) { Angle = Math.PI, Height = 0.6, Deviation = 0.01, Segment = Segment.Create( "Segment 002", SegmentTypes.Axis ) },
				new CylinderPoint(Segment.Create( "Segment 002", SegmentTypes.Axis ),1.25 * Math.PI, 0.5, 0.155),
				new CylinderPoint(Segment.Create( "Segment 002", SegmentTypes.Axis ),1.5 * Math.PI, 0.4, 0.375),
				new CylinderPoint(Segment.Create( "Segment 002", SegmentTypes.Axis ),1.75 * Math.PI, 0.3, 0.5) ,
				new CylinderPoint(Segment.Create( "Segment 002", SegmentTypes.Axis ),2 * Math.PI, 0.2, -0.55),
			} );

			var plot = new CylindricityPlot()
			{
				Properties = new[]
				{
					Property.Create( "BeliebigerName", "Beliebiger Text" ),
					Property.Create( "BeliebigerIntegerWert", 42 ),
					Property.Create( "BeliebigesDatum", new DateTime( 2012, 12, 21, 12, 02, 22, 123, CultureInfo.CurrentCulture.Calendar, DateTimeKind.Local ) ),
					Property.Create( "BeliebigeZeitspanne", TimeSpan.FromTicks( 162351234 ) ),
					Property.Create( "BeliebigerDoubleWert", Math.PI )
				},
				DefaultErrorScaling = Math.PI,
				Tolerance = new Tolerance( -0.5, 0.5 ),
				Points = points,
				Actual = new CylinderGeometry
				{
					CoordinateSystem = new CoordinateSystem
					{
						Origin = new Vector { X = 2001.1548, Y = 107.664, Z = 5.159 },
						Axis1 = new Vector { X = 1.456, Y = 0.158, Z = 1.0 / 7.0 },
						Axis2 = new Vector { X = 0.589, Y = 1.256, Z = 0.98465 },
						Axis3 = new Vector { X = 0.6465846, Y = 0.65465, Z = 1.46548 }
					},
					Radius = 1,
					Height = 1
				}
			};

			TestPlot( plot );
		}

		[Test]
		public void Test06_PitchFormplot()
		{
			var points = new ReadOnlyList<PitchPoint>( new[]
			{
				new PitchPoint(Segment.Create( "Segment 001" ), 0.15) {  State = PointState.Gap, Tolerance = new Tolerance( -0.1 ) },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.15) { State = PointState.Gap, Tolerance = new Tolerance( -0.1 ) },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.15) {  State = PointState.Outlier | PointState.Overlap },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( null, -0.1 ) },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.124) { State = PointState.Outlier },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.124) { State = PointState.Outlier },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.124) { State = PointState.Outlier },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.124),
				new PitchPoint(Segment.Create( "Segment 001" ), 0.124) { Tolerance = new Tolerance( -0.3, 0.5 ) },
				new PitchPoint(Segment.Create( "Segment 001" ), 0.124),
				new PitchPoint(Segment.Create( "Segment 001" ), 0.124) { Tolerance = new Tolerance( -0.4, 0.4 ) }
			} );

			var plot = new PitchPlot
			{
				Properties = new[]
				{
					Property.Create( "BeliebigerName", "Beliebiger Text" ),
					Property.Create( "BeliebigerIntegerWert", 42 ),
					Property.Create( "BeliebigesDatum", new DateTime( 2012, 12, 21, 12, 02, 22, 123, CultureInfo.CurrentCulture.Calendar, DateTimeKind.Local ) ),
					Property.Create( "BeliebigeZeitspanne", TimeSpan.FromTicks( 162351234 ) ),
					Property.Create( "BeliebigerDoubleWert", Math.PI )
				},
				DefaultErrorScaling = Math.PI,
				Tolerance = new Tolerance( -0.5, 0.5 ),
				Points = points,
			};

			TestPlot( plot );
		}

		[Test]
		public void Test07_BorePatternFormplot()
		{
			var tolerances = new ReadOnlyList<Tolerance>( new[]
			{
				new Tolerance( -0.1 ) { ToleranceType = ToleranceType.Rectangular, RectangleToleranceHeight = -0.1 },
				new Tolerance( null, -0.1 ) { ToleranceType = ToleranceType.Rectangular, RectangleToleranceWidth = -0.1 },
				new Tolerance( -0.4, 0.4 ) { ToleranceType = ToleranceType.Rectangular, RectangleToleranceHeight = -0.4, RectangleToleranceWidth = 0.4 },
				new Tolerance( -0.3, 0.5 ) { ToleranceType = ToleranceType.Rectangular, RectangleToleranceHeight = -0.3, RectangleToleranceWidth = 0.5 },
				new Tolerance( 0, 0.25 ) { ToleranceType = ToleranceType.Circular, CircularToleranceRadius = 0.25 }
			} );

			var points = new ReadOnlyList<BorePatternPoint>( new[]
			{
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 2.1, 1.1, 0.15 ),new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = tolerances[ 0 ] },
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 2.1, 1.1, 0.15 ),new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = tolerances[ 1 ] },
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 2.1, 2.1, 0.15 ),new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = tolerances[ 2 ] },
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 2.1, 3.1, 0.15 ),new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = tolerances[ 3 ] },
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 2.1, 4.1, 0.15 ),new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap },
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 2.1, 5.1, 0.15 ),new Vector { X = 1 }, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = tolerances[ 4 ] },
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 3.1, 6.1, 0.124 ),new Vector { X = 1 }, 0.124) { State = PointState.Outlier },
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 3.1, 7.1, 0.124 ),new Vector { X = 1 }, 0.124) { State = PointState.Outlier },
				new BorePatternPoint(Segment.Create( "Segment 001" ), new Vector( 3.1, 8.1, 0.124 ),new Vector { X = 1 }, 0.124) {  State = PointState.Outlier },
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 3.1, 9.1, 0.124 ),new Vector { X = 1 }, 0.124) {Tolerance = tolerances[ 2 ] },
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 4, 10.1, 0.124 ),new Vector { X = 1 }, 0.124) { Tolerance = tolerances[ 2 ] },
				new BorePatternPoint(Segment.Create( "Segment 001" ), new Vector( 6, 11.1, 0.124 ),new Vector { X = 1 }, 0.124),
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 7, 12.1, 0.124 ),new Vector { X = 1 }, 0.124) { Tolerance = tolerances[ 3 ] },
				new BorePatternPoint(Segment.Create( "Segment 001" ), new Vector( 7.1, 13.1, 0.124 ),new Vector { X = 1 }, 0.124),
				new BorePatternPoint(Segment.Create( "Segment 001" ),new Vector( 7.1, 14.1, 0.124 ),new Vector { X = 1 }, 0.124) { Tolerance = tolerances[ 2 ] }
			} );

			var plot = new BorePatternPlot
			{
				Properties = new[]
				{
					Property.Create( "BeliebigerName", "Beliebiger Text" ),
					Property.Create( "BeliebigerIntegerWert", 42 ),
					Property.Create( "BeliebigesDatum", new DateTime( 2012, 12, 21, 12, 02, 22, 123, CultureInfo.CurrentCulture.Calendar, DateTimeKind.Local ) ),
					Property.Create( "BeliebigeZeitspanne", TimeSpan.FromTicks( 162351234 ) ),
					Property.Create( "BeliebigerDoubleWert", Math.PI )
				},
				DefaultErrorScaling = 3.14159,
				Tolerance = new Tolerance { RectangleToleranceHeight = -0.3, RectangleToleranceWidth = 0.5, ToleranceType = ToleranceType.Rectangular },
				Points = points,
				Actual = new CurveGeometry
				{
					CoordinateSystem = new CoordinateSystem
					{
						Origin = new Vector { X = 2001.1548, Y = 107.664, Z = 5.159 },
						Axis1 = new Vector { X = 1.456, Y = 0.158, Z = 1.0 / 7.0 },
						Axis2 = new Vector { X = 0.589, Y = 1.256, Z = 0.98465 },
						Axis3 = new Vector { X = 0.6465846, Y = 0.65465, Z = 1.46548 }
					}
				}
			};

			TestPlot( plot );
		}

		[Test]
		public void Test08_CircleInProfileFormplot()
		{
			var points = new ReadOnlyList<CircleInProfilePoint>( new[]
			{
				new CircleInProfilePoint(Segment.Create( "Segment 001" ), 2.1, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CircleInProfilePoint(Segment.Create( "Segment 001" ), 1.1, 0.25) { Tolerance = new Tolerance( -0.1 ) },
				new CircleInProfilePoint(Segment.Create( "Segment 001" ), 1.0, 0.3) { Tolerance = new Tolerance( 0.1 ) },
				new CircleInProfilePoint(Segment.Create( "Segment 001" ), 0.5, 0.01) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( 0.1 ) },
				new CircleInProfilePoint(Segment.Create( "Segment 001" ), -2.1, 0.1) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( 0.1 ) },
				new CircleInProfilePoint(Segment.Create( "Segment 001" ), -1.1, 0.05) { Tolerance = new Tolerance( 0.1 ) },
				new CircleInProfilePoint(Segment.Create( "Segment 001" ), -1.0, 0.15) { State = PointState.Outlier | PointState.Overlap, Tolerance = new Tolerance( -0.1 ) },
				new CircleInProfilePoint(Segment.Create( "Segment 001" ), -0.5, 0.5) { Tolerance = new Tolerance( -0.1 ) }
			} );

			var plot = new CircleInProfilePlot()
			{
				Properties = new[]
				{
					Property.Create( "BeliebigerName", "Beliebiger Text" ),
					Property.Create( "BeliebigerIntegerWert", 42 ),
					Property.Create( "BeliebigesDatum", new DateTime( 2012, 12, 21, 12, 02, 22, 123, CultureInfo.CurrentCulture.Calendar, DateTimeKind.Local ) ),
					Property.Create( "BeliebigeZeitspanne", TimeSpan.FromTicks( 162351234 ) ),
					Property.Create( "BeliebigerDoubleWert", Math.PI )
				},
				DefaultErrorScaling = 3.14159,
				Tolerance = new Tolerance( -0.3, 0.5 ),
				Points = points,
				Nominal = new CircleInProfileGeometry
				{
					FirstTouchingPoint = new CircleInProfilePoint( Segment.Create( "Segment 001" ), 190, 0.0 ),
					SecondTouchingPoint = new CircleInProfilePoint( Segment.Create( "Segment 001" ), 355, 0.0 ),
				},
				Actual = new CircleInProfileGeometry
				{
					CoordinateSystem = new CoordinateSystem
					{
						Origin = new Vector { X = 2001.1548, Y = 107.664, Z = 5.159 },
						Axis1 = new Vector { X = 1.456, Y = 0.158, Z = 1.0 / 7.0 },
						Axis2 = new Vector { X = 0.589, Y = 1.256, Z = 0.98465 },
						Axis3 = new Vector { X = 0.6465846, Y = 0.65465, Z = 1.46548 }
					},
					Radius = 1,
					FirstTouchingPoint = new CircleInProfilePoint( Segment.Create( "Segment 001" ), 189, 0.0 ) { Tolerance = new Tolerance( -0.1, 0.2 ) },
					SecondTouchingPoint = new CircleInProfilePoint( Segment.Create( "Segment 001" ), 354, 0.0 ) { Tolerance = new Tolerance( -0.2, 0.3 ) },
					MaxGapPoint = new CircleInProfilePoint( Segment.Create( "Segment 001" ), 271, 0.51 )
				}
			};

			TestPlot( plot );
		}

		[Test]
		public void Test09_FourierFormplot()
		{
			var points = new ReadOnlyList<FourierPoint>( new[]
			{
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 0, 0.1 ) { Tolerance = new Tolerance( null, 0.25 ) },
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 1, 0.095 ) { Tolerance = new Tolerance( null, 0.1 ) },
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 2, 0.081 ) { Tolerance = new Tolerance( null, 0.09 ) },
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 3, 0.079 ) { Tolerance = new Tolerance( null, 0.08 ) },
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 4, 0.075 ) { Tolerance = new Tolerance( null, 0.075 ) },
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 5, 0.71 ) { Tolerance = new Tolerance( null, 0.07 ) },
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 6, 0.06 ) { Tolerance = new Tolerance( null, 0.065 ) },
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 7, 0.057 ) { Tolerance = new Tolerance( null, 0.05 ) },
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 8, 0.048 ) { Tolerance = new Tolerance( null, 0.042 ) },
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 9, 0.042 ) { Tolerance = new Tolerance( null, 0.038 ) },
				new FourierPoint( new Segment( "Fourier", SegmentTypes.None ), 10, 0.021 ) { Tolerance = new Tolerance( null, 0.025 ) }
			} );

			var plot = new FourierPlot
			{
				Properties = new[]
				{
					Property.Create( "BeliebigerName", "Beliebiger Text" ),
					Property.Create( "BeliebigerIntegerWert", 42 ),
					Property.Create( "BeliebigesDatum", new DateTime( 2012, 12, 21, 12, 02, 22, 123, CultureInfo.CurrentCulture.Calendar, DateTimeKind.Local ) ),
					Property.Create( "BeliebigeZeitspanne", TimeSpan.FromTicks( 162351234 ) ),
					Property.Create( "BeliebigerDoubleWert", Math.PI )
				},
				DefaultErrorScaling = Math.PI,
				Tolerance = new Tolerance( null, 0.5 ),
				Points = points,
			};

			TestPlot( plot );
		}

		[Test]
		public void Test11_FormplotFiles()
		{
			var formplotFiles = Directory.GetFiles( GlobalSetup.SharedFiles, "*" + MimeTypes.FormplotFileExtension, SearchOption.AllDirectories )
				.Concat( Directory.GetFiles( GlobalSetup.PiWebData, "*" + MimeTypes.FormplotFileExtension, SearchOption.AllDirectories ) );

			foreach( var formplotFile in formplotFiles )
			{
				using( var stream = File.OpenRead( formplotFile ) )
				{
					Assert.NotNull( Formplot.ReadFrom( stream ) );

					Console.Out.WriteLine( "Test of formplot file '{0}' successful.", formplotFile );
				}
			}

			var propertyFiles = Directory.GetFiles( GlobalSetup.SharedFiles, "*" + MimeTypes.PropertiesFileExtension, SearchOption.AllDirectories )
				.Concat( Directory.GetFiles( GlobalSetup.PiWebData, "*" + MimeTypes.PropertiesFileExtension, SearchOption.AllDirectories ) );

			foreach( var file in propertyFiles )
			{
				using( var stream = File.OpenRead( file ) )
				{
					Assert.NotNull( Formplot.ReadFrom( stream ) );
				}
			}
		}

		private static Tuple<string, byte[]> PlotToData( Formplot plot )
		{
			byte[] buffer;

			using( var binaryStream = new MemoryStream() )
			{
				plot.WriteTo( binaryStream );
				buffer = binaryStream.ToArray();
			}

			using( var steam = new MemoryStream( buffer, 0, buffer.Length, false, true ) )
			using( var zipFile = new ZipFile( steam ) )
			{
				var header = zipFile.GetEntry( "header.xml" );
				var points = zipFile.GetEntry( "plotpoints.dat" );

				using( var headerStream = zipFile.GetInputStream( header ) )
				using( var pointsStream = points != null ? zipFile.GetInputStream( points ) : null )
				{
					return Tuple.Create( Encoding.UTF8.GetString( StreamHelper.StreamToArray( headerStream ) ), StreamHelper.StreamToArray( pointsStream ) );
				}
			}
		}

		private static void TestPlot( Formplot plot )
		{
			var before = PlotToData( plot );

			{
				var internalPlot = Converter.FileToInternal( plot );
				var newplot = Converter.InternalToFile( internalPlot );
				var after = PlotToData( newplot );

				Assert.AreEqual( before.Item1, after.Item1 );
				Assert.AreEqual( before.Item2, after.Item2 );
			}

			byte[] buffer;

			using( var stream = new MemoryStream() )
			{
				plot.WriteTo( stream );
				buffer = stream.ToArray();
			}

			using( var stream = new MemoryStream( buffer, 0, buffer.Length, false, true ) )
			{
				var newplot = Formplot.ReadFrom( stream );
				var after = PlotToData( newplot );

				Assert.AreEqual( before.Item1, after.Item1 );
				Assert.AreEqual( before.Item2, after.Item2 );
			}

			Console.WriteLine( "Test of formplot type '{0}' successful.", plot.FormplotType );
		}

		#endregion
	}
}
