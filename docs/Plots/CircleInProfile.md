[preview]: gfx/CircleInProfile.png "Circle-in-profile plot"
<br/>
### Circle-in-profile plot

![Circle in profile plot][preview]

#### Description

The circle-in-profile plot is displaying a circle, fitted into measured points. It's a specialized version of the circle plot, with the additional ability to display angular deviations, and touching points, with their respective tolerances.

#### Point structure

* Angle (radians)
* Deviation

#### Geometry

The circle-in-profile geometry can be used to define two touching points (`FirstTouchingPoint` and `SecondTouchingPoint`) and a point of maximum distance (`MaxGapPoint`). The __nominal__ touching point defines the angle, around which the tolerance of the point spans. The __acual__ touching point defines the tolerance, as well as the actual angle of the touching point. The same logic applies to the `MaxGapPoint`.
These special points can be part of the plots point list, but are usually defined separately.

#### Example

```csharp
var plot = new CircleInProfilePlot();

var segment = new Segment<CircleInProfilePoint, CircleInProfileGeometry>( "All", SegmentTypes.Line );
plot.Segments.Add( segment )

var rand = new Random( DateTime.Now.Millisecond );

var angleShift = ( rand.NextDouble() - 0.5 ) * 0.25;

for( var i = 0; i < count; i++ )
{
	var angle = (double)i / count * Math.PI;

	var deviation = Math.Abs( Math.Sin( angle ) - Math.Sin( 0.25 * Math.PI ) ) * 0.1 + ( rand.NextDouble() - 0.5 ) * 0.005;
	var point = new CircleInProfilePoint( angle + angleShift, deviation );


	if( i == count / 4 )
	{
		plot.Nominal.FirstTouchingPoint = new CircleInProfilePoint( angle, 0 );

		//To create an angular tolerance, add it to the actual touching points.
		//The tolerance spans around the nominal touching point angle.
		plot.Actual.FirstTouchingPoint = new CircleInProfilePoint( point.Angle, point.Deviation )
		{
			Tolerance = new Tolerance( -0.1, 0.1 )
		};
	}

	if( i == count / 4 * 3 )
	{
		plot.Nominal.SecondTouchingPoint = new CircleInProfilePoint( angle, 0 );

		//To create an angular tolerance, add it to the actual touching points.
		//The tolerance spans around the nominal touching point angle.
		plot.Actual.SecondTouchingPoint = new CircleInProfilePoint( point.Angle, point.Deviation )
		{
			Tolerance = new Tolerance( -0.1, 0.1 )
		};
	}

	if( i == count / 2 )
	{
		plot.Nominal.MaxGapPoint = new CircleInProfilePoint( angle, 0 );
		plot.Actual.MaxGapPoint = new CircleInProfilePoint( point.Angle, point.Deviation );
	}
	
	segment.Points.Add( point );
}

plot.Tolerance = new Tolerance( 0.1 );
plot.DefaultErrorScaling = 250;
```
<br/>
<br/>
