#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2025                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.Tests.FileFormat
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using NUnit.Framework;
	using Zeiss.PiWeb.Formplot.Common;
	using Zeiss.PiWeb.Formplot.FileFormat;

	[TestFixture]
	[Parallelizable( ParallelScope.All )]
	public class FormplotConverterTest
	{
		[Test]
		public void Test_Convert_Straightness_To_Curve()
		{
			var straightness = new StraightnessPlot
			{
				Actual =
				{
					Position = new Vector( 3, 3, 3 ),
					Direction = new Vector( 0, 0, 1 ),
					Deviation = new Vector( 0, 1 ),
					Length = 2
				}
			};
			straightness.Segments.Add( new Segment<LinePoint, LineGeometry>
			{
				Points =
				{
					new LinePoint( 0, 0.1 ),
					new LinePoint( 1, 0.2 ) { Tolerance = new Tolerance( 0, 0.3 ) }
				}
			} );

			var result = FormplotConverter.TryConvert<CurveProfilePlot>( straightness, out var curve );

			Assert.That( result, Is.True );
			Assert.That( curve, Is.Not.Null );
			Assert.That( curve.Points, Has.Exactly( 2 ).Items );

			var points = curve.Points.ToArray();

			Assert.That( points[ 0 ].Position, Is.EqualTo( new Vector( 0, 0, 0 ) ) );
			Assert.That( points[ 0 ].Direction, Is.EqualTo( new Vector( 0, 0, 1 ) ) );
			Assert.That( points[ 0 ].Deviation, Is.EqualTo( 0.1 ) );
			Assert.That( points[ 1 ].Position, Is.EqualTo( new Vector( 2, 0, 0 ) ) );
			Assert.That( points[ 1 ].Direction, Is.EqualTo( new Vector( 0, 0, 1 ) ) );
			Assert.That( points[ 1 ].Deviation, Is.EqualTo( 0.2 ) );
			Assert.That( points[ 1 ].Tolerance, Is.EqualTo( new Tolerance( 0, 0.3 ) ) );
		}

		[Test]
		public void Test_Convert_Roundness_To_Curve()
		{
			var roundness = new RoundnessPlot
			{
				Actual =
				{
					Radius = 2
				}
			};
			roundness.Segments.Add( new Segment<CirclePoint, CircleGeometry>
			{
				Points =
				{
					new CirclePoint( 0, 0.1 ),
					new CirclePoint( 0.5 * Math.PI, 0.15 ) { Tolerance = new Tolerance( 0, 0.3 ) }
				}
			} );

			var result = FormplotConverter.TryConvert<CurveProfilePlot>( roundness, out var curve );

			Assert.That( result, Is.True );
			Assert.That( curve, Is.Not.Null );
			Assert.That( curve.Points, Has.Exactly( 2 ).Items );

			var points = curve.Points.ToArray();

			Assert.That( points[ 0 ].Position, Is.EqualTo( new Vector( 2 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 0 ].Direction, Is.EqualTo( new Vector( 1 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 0 ].Deviation, Is.EqualTo( 0.1 ) );
			Assert.That( points[ 1 ].Position, Is.EqualTo( new Vector( 0, 2 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 1 ].Direction, Is.EqualTo( new Vector( 0, 1 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 1 ].Deviation, Is.EqualTo( 0.15 ) );
			Assert.That( points[ 1 ].Tolerance, Is.EqualTo( new Tolerance( 0, 0.3 ) ).Using( VectorComparer.Instance ) );
		}

		[Test]
		public void Test_Convert_Cylindricity_To_Curve()
		{
			var cylindricity = new CylindricityPlot
			{
				Actual =
				{
					Height = 3,
					Radius = 2
				}
			};
			cylindricity.Segments.Add( new Segment<CylinderPoint, CylinderGeometry>
			{
				Points =
				{
					new CylinderPoint( 0, 1, 0.1 ),
					new CylinderPoint( 0.5 * Math.PI, 2, 0.15 )
				}
			} );

			var result = FormplotConverter.TryConvert<CurveProfilePlot>( cylindricity, out var curve );

			Assert.That( result, Is.True );
			Assert.That( curve, Is.Not.Null );
			Assert.That( curve.Points, Has.Exactly( 2 ).Items );

			var points = curve.Points.ToArray();

			Assert.That( points[ 0 ].Position, Is.EqualTo( new Vector( 2, 0, 3 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 0 ].Direction, Is.EqualTo( new Vector( 1 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 0 ].Deviation, Is.EqualTo( 0.1 ) );
			Assert.That( points[ 1 ].Position, Is.EqualTo( new Vector( 0, 2, 6 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 1 ].Direction, Is.EqualTo( new Vector( 0, 1 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 1 ].Deviation, Is.EqualTo( 0.15 ) );
		}

		[Test]
		public void Test_Convert_Flatness_To_Curve()
		{
			var flatness = new FlatnessPlot
			{
				Actual =
				{
					Length1 = 4,
					Length2 = 3
				}
			};
			flatness.Segments.Add( new Segment<PlanePoint, PlaneGeometry>
			{
				Points =
				{
					new PlanePoint( 0, 0, 0.1 ),
					new PlanePoint( 2, 2, 0.15 )
				}
			} );

			var result = FormplotConverter.TryConvert<CurveProfilePlot>( flatness, out var curve );

			Assert.That( result, Is.True );
			Assert.That( curve, Is.Not.Null );
			Assert.That( curve.Points, Has.Exactly( 2 ).Items );

			var points = curve.Points.ToArray();

			Assert.That( points[ 0 ].Position, Is.EqualTo( new Vector( 0 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 0 ].Direction, Is.EqualTo( new Vector( 0, 0, 1 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 0 ].Deviation, Is.EqualTo( 0.1 ) );
			Assert.That( points[ 1 ].Position, Is.EqualTo( new Vector( 8, 6 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 1 ].Direction, Is.EqualTo( new Vector( 0, 0, 1 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 1 ].Deviation, Is.EqualTo( 0.15 ) );
		}

		[Test]
		public void Test_Convert_BorePattern_To_Curve()
		{
			var borepattern = new BorePatternPlot();
			borepattern.Segments.Add( new Segment<CurvePoint, CurveGeometry>
			{
				Points =
				{
					new CurvePoint( new Vector( 1, 1, 1 ), new Vector( 2, 2, 2 ), 0.1 ),
					new CurvePoint( new Vector( 3, 3, 3 ), new Vector( 4, 4, 4 ), 0.15 )
				}
			} );

			var result = FormplotConverter.TryConvert<CurveProfilePlot>( borepattern, out var curve );

			Assert.That( result, Is.True );
			Assert.That( curve, Is.Not.Null );
			Assert.That( curve.Points, Has.Exactly( 2 ).Items );

			var points = curve.Points.ToArray();

			Assert.That( points[ 0 ].Position, Is.EqualTo( new Vector( 1, 1, 1 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 0 ].Direction, Is.EqualTo( new Vector( 2, 2, 2 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 0 ].Deviation, Is.EqualTo( 0.1 ) );
			Assert.That( points[ 1 ].Position, Is.EqualTo( new Vector( 3, 3, 3 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 1 ].Direction, Is.EqualTo( new Vector( 4, 4, 4 ) ).Using( VectorComparer.Instance ) );
			Assert.That( points[ 1 ].Deviation, Is.EqualTo( 0.15 ) );
		}

		private class VectorComparer : IEqualityComparer<Vector>
		{
			public static readonly VectorComparer Instance = new();

			public bool Equals( Vector x, Vector y )
			{
				return x.X.IsCloseTo( y.X ) && x.Y.IsCloseTo( y.Y ) && x.Z.IsCloseTo( y.Z );
			}

			public int GetHashCode( Vector obj )
			{
				return 0;
			}
		}
	}
}