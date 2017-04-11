[preview]: gfx/Cylindricity.png "Cylindricity plot"

### Cylindricity plot

![cylindricity plot][preview]

#### Description

The cylindricity plot displays deviations on a cylinder, usually a helix or circles on multiple height levels. The cylindricity plot shares its data structures with the [axiality plot](Axiality.md) and the [generatrix plot](Generatrix.md).

#### Point structure

* Angle (radians)
* Height
* Deviation (from circle)

The points should use `Helix` or `Circle` as their segment type. Please note that the plot won't display the points in case they are all located on the same height. If they are specified like this on purpose, use the [circle plot](Circle.md) instead.

#### Geometry

The plot can display a height axis. The range of the axis is determined by multiplying the points height range with the actual geometries height. The other geometry parameters are currently unused.

#### Example

```csharp
var plot = new CylindricityPlot();
var points = new List<CylinderPoint>();

var rand = new Random( DateTime.Now.Millisecond );
var segment = new Segment( "All", SegmentTypes.Helix );

//The range of the displayed z-axis will be this height multiplied with the points height range.
plot.Actual.Height = 10;

for( var i = 0; i < count; i++ )
{
	var angle = ( double ) i / count * 2.0 * spins * Math.PI;
	var deviation = 0.1 * ( Math.Sin( angle ) + ( rand.NextDouble() - 0.5 ) * 0.2 );
	var height = ( double ) i / count;

	var point = new CylinderPoint( segment, angle, height, deviation );
	points.Add( point );
}

plot.Tolerance = new Tolerance( -0.1, 0.1 );
plot.DefaultErrorScaling = 100;
plot.Points = points;
```

#### Remarks

* The points may not be located in a single X-Y plane (share one height)
* When displaying multiple circles on different heights, use a different segment for every circle, to avoid the lines beeing connected.
