#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
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
		/// <returns><c>true</c> if the conversion was succcessful, otherwise <c>false</c></returns>
		public static bool TryConvert<TPlot>( Formplot source, out TPlot? target )
			where TPlot : Formplot

		{
			if( typeof( TPlot ) == typeof( StraightnessPlot ) )
			{
				target = ConvertToStraightness( source ) as TPlot;
				return true;
			}

			if( typeof( TPlot ) == typeof( RoundnessPlot ) )
			{
				target = ConvertToRoundness( source ) as TPlot;
				return true;
			}

			target = null;
			return false;
		}

		private static StraightnessPlot? ConvertToStraightness( Formplot source )
		{
			switch( source )
			{
				case CylindricityPlot cylindricity: return ConvertToStraightness( cylindricity );
				case CurveProfilePlot curveProfile: return ConvertCurveToStraightness( curveProfile );
				case FlushGapPlot flushGap: return ConvertCurveToStraightness( flushGap );
				case FilletPlot fillet: return ConvertCurveToStraightness( fillet );
				case FlatnessPlot flatness: return ConvertToStraightness( flatness );
				case RoundnessPlot roundness: return ConvertCircleToStraightness( roundness );
				case CircleInProfilePlot circleInProfile: return ConvertCircleToStraightness( circleInProfile );
				default: return null;
			}
		}

		private static RoundnessPlot? ConvertToRoundness( Formplot source )
		{
			switch( source )
			{
				case CylindricityPlot cylindricity: return ConvertToRoundness( cylindricity );
				default: return null;
			}
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
			target.Properties.Set( source.Properties );
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

			double? lastAngle = null;
			const double radToDeg = 180 / Math.PI;

			foreach( var segment in source.Segments )
			{
				var resultSegment = new Segment<LinePoint, LineGeometry>( segment.Name, segment.SegmentType );
				result.Segments.Add( resultSegment );

				foreach( var point in segment.Points )
				{
					double position;
					if( !lastAngle.HasValue )
					{
						lastAngle = AdjustAngle( point.Angle * radToDeg );
						position = lastAngle.Value;
					}
					else
					{
						var angle = AdjustAngle( point.Angle * radToDeg );

						while( angle < lastAngle )
							angle += 360;

						lastAngle = angle;
						position = angle;
					}

					var resultPoint = new LinePoint( position, point.Deviation );
					CopyDefaults( point, resultPoint );
					resultSegment.Points.Add( resultPoint );
				}
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

		#endregion
	}
}