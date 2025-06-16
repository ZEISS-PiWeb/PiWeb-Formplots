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
	using System.Linq;

	#endregion

	/// <summary>
	/// Can be used to convert a formplot to a distinct type.
	/// </summary>
	public static class FormplotConverter
	{
		#region methods

		/// <summary>
		/// Converts the specified source plot to the target type.
		/// </summary>
		/// <param name="source">Source plot</param>
		/// <param name="target">Target plot</param>
		/// <typeparam name="TPlot">Target plot type</typeparam>
		/// <returns><c>true</c> if the conversion was successful, otherwise <c>false</c></returns>
		public static bool TryConvert<TPlot>( Formplot source, out TPlot? target )
			where TPlot : Formplot

		{
			if( typeof( TPlot ) == typeof( StraightnessPlot ) )
			{
				target = ConvertToStraightness( source ) as TPlot;
				return target != null;
			}

			if( typeof( TPlot ) == typeof( RoundnessPlot ) )
			{
				target = ConvertToRoundness( source ) as TPlot;
				return target != null;
			}

			if( typeof( TPlot ) == typeof( CurveProfilePlot ) )
			{
				target = ConvertToCurveProfile( source ) as TPlot;
				return target != null;
			}

			if( typeof( TPlot ) == typeof( CurveDistancePlot ) )
			{
				target = ConvertToCurveDistance( source ) as TPlot;
				return target != null;
			}

			target = null;
			return false;
		}

		private static StraightnessPlot? ConvertToStraightness( Formplot source )
		{
			return source switch
			{
				CylindricityPlot cylindricity       => ConvertToStraightness( cylindricity ),
				CurveProfilePlot curveProfile       => ConvertCurveToStraightness( curveProfile ),
				FlushGapPlot flushGap               => ConvertCurveToStraightness( flushGap ),
				FilletPlot fillet                   => ConvertCurveToStraightness( fillet ),
				FlatnessPlot flatness               => ConvertToStraightness( flatness ),
				RoundnessPlot roundness             => ConvertCircleToStraightness( roundness ),
				CircleInProfilePlot circleInProfile => ConvertCircleToStraightness( circleInProfile ),
				_                                   => null
			};
		}

		private static RoundnessPlot? ConvertToRoundness( Formplot source )
		{
			return source switch
			{
				CylindricityPlot cylindricity => ConvertToRoundness( cylindricity ),
				_                             => null
			};
		}

		private static CurveProfilePlot? ConvertToCurveProfile( Formplot source )
		{
			return source switch
			{
				BorePatternPlot typed   => ConvertToCurveProfile( typed ),
				CurveDistancePlot typed => ConvertToCurveProfile( typed ),
				CylindricityPlot typed  => ConvertToCurveProfile( typed ),
				StraightnessPlot typed  => ConvertToCurveProfile( typed ),
				RoundnessPlot typed     => ConvertToCurveProfile( typed ),
				FlatnessPlot typed      => ConvertToCurveProfile( typed ),
				_                       => null
			};
		}

		private static CurveDistancePlot? ConvertToCurveDistance( Formplot source )
		{
			return source switch
			{
				CurveProfilePlot curve => ConvertToCurveDistance( curve ),
				_                      => null
			};
		}

		private static void CopyDefaults( Formplot source, Formplot target )
		{
			target.OriginalFormplotType = source.FormplotType;
			target.ProjectionAxis = source.ProjectionAxis;
			target.Properties.Set( source.Properties );
			target.Tolerance = source.Tolerance;
			target.DefaultErrorScaling = source.DefaultErrorScaling;
			target.CreatorSoftware = source.CreatorSoftware;
			target.CreatorSoftwareVersion = source.CreatorSoftwareVersion;
			target.PreserveAspectRatio = source.PreserveAspectRatio;
		}

		private static void CopyDefaults( Point source, Point target )
		{
			if( source.HasProperties )
				target.Properties.Set( source.Properties );
			if( source.HasTolerance )
				target.Tolerance = source.Tolerance;
			target.State = source.State;
			target.Index = source.Index;
		}

		private static double AdjustAngle( double angle )
		{
			var factor = (int)( angle / 360 );
			if( angle < 0 ) factor--;

			return angle - factor * 360;
		}

		private static StraightnessPlot ConvertCircleToStraightness<TPoint, TGeometry>( Formplot<TPoint, TGeometry> source )
			where TPoint : CirclePointBase<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			var result = new StraightnessPlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			const double radToDeg = 180 / Math.PI;

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<LinePoint, LineGeometry>( segment.Name, segment.SegmentType );
				var points = new List<LinePoint>( segment.Points.Count );

				result.Segments.Add( resultSegment );

				foreach( var point in segment.Points )
				{
					var resultPoint = new LinePoint( AdjustAngle( point.Angle * radToDeg ), point.Deviation );
					CopyDefaults( point, resultPoint );
					points.Add( resultPoint );
				}

				foreach( var point in points.OrderBy( p => p.Position ) )
					resultSegment.Points.Add( point );
			}

			return result;
		}

		private static StraightnessPlot ConvertCurveToStraightness<TPoint, TGeometry>( Formplot<TPoint, TGeometry> source )
			where TPoint : CurvePointBase<TPoint, TGeometry>, new()
			where TGeometry : Geometry, new()
		{
			var result = new StraightnessPlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			Vector? lastPoint = null;
			var currentLength = 0.0;

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<LinePoint, LineGeometry>( segment.Name, segment.SegmentType );

				result.Segments.Add( resultSegment );

				foreach( var point in segment.Points )
				{
					if( lastPoint == null )
						lastPoint = point.Position;
					else
					{
						var dx = point.Position.X - lastPoint.Value.X;
						var dy = point.Position.Y - lastPoint.Value.Y;
						var dz = point.Position.Z - lastPoint.Value.Z;

						lastPoint = point.Position;

						currentLength += Math.Sqrt( dx * dx + dy * dy + dz * dz );
					}

					var resultPoint = new LinePoint( currentLength, point.Deviation );
					CopyDefaults( point, resultPoint );
					resultSegment.Points.Add( resultPoint );
				}
			}

			return result;
		}

		private static StraightnessPlot ConvertToStraightness( FlatnessPlot source )
		{
			var result = new StraightnessPlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			Vector? lastPoint = null;
			var currentLength = 0.0;

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<LinePoint, LineGeometry>( segment.Name, segment.SegmentType );

				result.Segments.Add( resultSegment );

				foreach( var point in segment.Points )
				{
					if( lastPoint == null )
						lastPoint = new Vector( point.Coordinate1, point.Coordinate2 );
					else
					{
						var dx = point.Coordinate1 - lastPoint.Value.X;
						var dy = point.Coordinate2 - lastPoint.Value.Y;

						lastPoint = new Vector( point.Coordinate1, point.Coordinate2 );

						currentLength += Math.Sqrt( dx * dx + dy * dy );
					}

					var resultPoint = new LinePoint( currentLength, point.Deviation );
					CopyDefaults( point, resultPoint );
					resultSegment.Points.Add( resultPoint );
				}
			}

			return result;
		}

		private static StraightnessPlot ConvertToStraightness( CylindricityPlot source )
		{
			var result = new StraightnessPlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			var heightFactor = source.Actual.Height;

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<LinePoint, LineGeometry>( segment.Name, segment.SegmentType );

				result.Segments.Add( resultSegment );

				foreach( var point in segment.Points )
				{
					var resultPoint = new LinePoint( point.Height * heightFactor, point.Deviation );

					CopyDefaults( point, resultPoint );

					resultSegment.Points.Add( resultPoint );
				}
			}

			return result;
		}

		private static RoundnessPlot ConvertToRoundness( CylindricityPlot source )
		{
			var result = new RoundnessPlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			result.Actual.Radius = source.Actual.Radius;
			result.Nominal.Radius = source.Nominal.Radius;

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<CirclePoint, CircleGeometry>( segment.Name, segment.SegmentType )
				{
					Position = segment.Points.FirstOrDefault()?.Height * source.Actual.Height ?? 0.0
				};
				result.Segments.Add( resultSegment );

				foreach( var point in segment.Points )
				{
					var resultPoint = new CirclePoint( point.Angle, point.Deviation );
					resultSegment.Points.Add( resultPoint );
					resultPoint.State = point.State;
				}
			}

			return result;
		}

		private static CurveProfilePlot ConvertToCurveProfile( BorePatternPlot source )
		{
			var result = new CurveProfilePlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			//Segments and points have a reference to the plot, so we must copy them, despite being of the same type.
			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<CurvePoint, CurveGeometry>( segment.Name, segment.SegmentType );

				foreach( var point in segment.Points )
				{
					var resultPoint = new CurvePoint( point.Position, point.Direction, point.Deviation );
					CopyDefaults( point, resultPoint );
					resultSegment.Points.Add( resultPoint );
				}

				result.Segments.Add( resultSegment );
			}

			return result;
		}

		private static CurveProfilePlot ConvertToCurveProfile( CylindricityPlot source )
		{
			var result = new CurveProfilePlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<CurvePoint, CurveGeometry>( segment.Name, segment.SegmentType );
				foreach( var point in segment.Points )
				{
					var x = Math.Cos( point.Angle ) * source.Actual.Radius;
					var y = Math.Sin( point.Angle ) * source.Actual.Radius;

					var curvePoint = new CurvePoint(
						new Vector( x, y, point.Height * source.Actual.Height ),
						new Vector( x, y ).Normalized(),
						point.Deviation );

					CopyDefaults( point, curvePoint );

					resultSegment.Points.Add( curvePoint );
				}

				result.Segments.Add( resultSegment );
			}

			return result;
		}

		private static CurveProfilePlot ConvertToCurveProfile( StraightnessPlot source )
		{
			var result = new CurveProfilePlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<CurvePoint, CurveGeometry>( segment.Name, segment.SegmentType );
				foreach( var point in segment.Points )
				{
					var curvePoint = new CurvePoint(
						new Vector( point.Position * source.Actual.Length ),
						new Vector( 0, 1 ),
						point.Deviation );

					CopyDefaults( point, curvePoint );

					resultSegment.Points.Add( curvePoint );
				}

				result.Segments.Add( resultSegment );
			}

			return result;
		}

		private static CurveProfilePlot ConvertToCurveProfile( RoundnessPlot source )
		{
			var result = new CurveProfilePlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<CurvePoint, CurveGeometry>( segment.Name, segment.SegmentType );
				foreach( var point in segment.Points )
				{
					var x = Math.Cos( point.Angle ) * source.Actual.Radius;
					var y = Math.Sin( point.Angle ) * source.Actual.Radius;

					var curvePoint = new CurvePoint(
						new Vector( x, y ),
						new Vector( x, y ).Normalized(),
						point.Deviation );

					CopyDefaults( point, curvePoint );

					resultSegment.Points.Add( curvePoint );
				}

				result.Segments.Add( resultSegment );
			}

			return result;
		}

		private static CurveProfilePlot ConvertToCurveProfile( FlatnessPlot source )
		{
			var result = new CurveProfilePlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;


			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<CurvePoint, CurveGeometry>( segment.Name, segment.SegmentType );
				foreach( var point in segment.Points )
				{
					var curvePoint = new CurvePoint(
						new Vector( point.Coordinate1 * source.Actual.Length1, point.Coordinate2 * source.Actual.Length2 ),
						new Vector( 0, 0, 1 ),
						point.Deviation );

					CopyDefaults( point, curvePoint );

					resultSegment.Points.Add( curvePoint );
				}

				result.Segments.Add( resultSegment );
			}

			return result;
		}

		private static CurveProfilePlot ConvertToCurveProfile( CurveDistancePlot source )
		{
			var result = new CurveProfilePlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<CurvePoint, CurveGeometry>( segment.Name, segment.SegmentType );
				foreach( var point in segment.Points )
				{
					resultSegment.Points.Add( new CurvePoint(
						point.FirstPosition,
						point.FirstDirection,
						point.FirstDeviation ) );
				}

				result.Segments.Add( resultSegment );
			}

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<CurvePoint, CurveGeometry>( segment.Name, segment.SegmentType );
				foreach( var point in segment.Points )
				{
					resultSegment.Points.Add( new CurvePoint(
						point.SecondPosition,
						point.SecondDirection,
						point.SecondDeviation )
					{
						State = point.State
					} );
				}

				result.Segments.Add( resultSegment );
			}

			return result;
		}

		private static CurveDistancePlot? ConvertToCurveDistance( CurveProfilePlot source )
		{
			if( source.Segments.Count % 2 != 0 )
				return null;

			var result = new CurveDistancePlot();

			CopyDefaults( source, result );

			result.Actual.CoordinateSystem = source.Actual.CoordinateSystem;
			result.Nominal.CoordinateSystem = source.Nominal.CoordinateSystem;

			var targetSegmentCount = source.Segments.Count / 2;

			for( var i = 0; i < targetSegmentCount; i++ )
			{
				var firstSegment = source.Segments[ i ];
				var secondSegment = source.Segments[ i + targetSegmentCount ];

				if( firstSegment.Points.Count != secondSegment.Points.Count )
					return null;

				var resultSegment = new Segment<CurveDistancePoint, CurveDistanceGeometry>( firstSegment.Name, firstSegment.SegmentType );

				for( var j = 0; j < firstSegment.Points.Count; j++ )
				{
					var firstPoint = firstSegment.Points[ j ];
					var secondPoint = secondSegment.Points[ j ];

					var firstPointActual = new Vector(
						firstPoint.Position.X + firstPoint.Direction.X * firstPoint.Deviation,
						firstPoint.Position.Y + firstPoint.Direction.Y * firstPoint.Deviation,
						firstPoint.Position.Z + firstPoint.Direction.Z * firstPoint.Deviation );

					var secondPointActual = new Vector(
						secondPoint.Position.X + secondPoint.Direction.X * secondPoint.Deviation,
						secondPoint.Position.Y + secondPoint.Direction.Y * secondPoint.Deviation,
						secondPoint.Position.Z + secondPoint.Direction.Z * secondPoint.Deviation );

					var distance = Math.Sqrt( ( firstPointActual.X - secondPointActual.X ) * ( firstPointActual.X - secondPointActual.X ) +
											( firstPointActual.Y - secondPointActual.Y ) * ( firstPointActual.Y - secondPointActual.Y ) +
											( firstPointActual.Z - secondPointActual.Z ) * ( firstPointActual.Z - secondPointActual.Z ) );

					var resultPoint = new CurveDistancePoint(
						firstPoint.Position,
						firstPoint.Direction,
						firstPoint.Deviation,
						secondPoint.Position,
						secondPoint.Direction,
						secondPoint.Deviation,
						distance )
					{
						State = firstPoint.State | secondPoint.State
					};

					resultSegment.Points.Add( resultPoint );
				}

				result.Segments.Add( resultSegment );
			}

			return result;
		}

		#endregion
	}
}