[preview]: gfx/Generatrix.png "Generatrix plot"
<br/>
### Generatrix plot

![Generatrix plot][preview]

#### Description

The generatrix plot is a two dimensional presentation of vertical lines on a cylinder. The generatrix plot shares its data structures with the [cylindricity plot](Cylindricity.md) and the [axiality plot](Axiality.md).

#### Point structure

* Angle (radians)
* Height
* Deviation (from cylinder)

The points should use `Line` as their segment type. Usually, the generatrix renders at least two segments. Based on a segments point angles, it is either displayed at the left or the right side of the plot. When using three or more segments, multiple segments share the same drawing space. The points of a segment are rotated into the viewing plane in case their angle isn't constant.

#### Geometry

The plot can display a height and a radius axis. The range of these axis is determined by multiplying the point coordinate ranges with the `Height` and `Radius` parameters of the _actual_ geometry. The other geometry parameters are currently unused.

#### Example

```csharp
var plot = new CylindricityPlot();
var points = new List<CylinderPoint>();

var rand = new Random( DateTime.Now.Millisecond );
var left = new Segment( "Left", SegmentTypes.Line);
var right = new Segment( "Right", SegmentTypes.Line );

//The x- and y-axis of the plot will span over the radius and height * plotpoints min/max.
plot.Actual.Height = 15;
plot.Actual.Radius = 5;

for( var i = 0; i < count; i++ )
{
	var deviation = rand.NextDouble() * 0.1;
	var height = (double)i / count;

	var point = new CylinderPoint( left, 0.0, height, deviation );
	points.Add( point );

	point = new CylinderPoint( right, 0.5 * Math.PI, height, deviation );
	points.Add( point );
}

plot.Tolerance = new Tolerance( -0.1, 0.1 );
plot.DefaultErrorScaling = 100;
plot.Points = points;
```

#### Remarks

* The points may not be located in a single X-Y plane (share one height)
* The points of a segment may have different angles, the plot will rotate them back into one plane.
<br/>
<br/>
