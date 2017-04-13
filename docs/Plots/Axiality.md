
[preview]: gfx/Axiality.png "Axiality plot"
<br/>
### Axiality plot

![curve plot][preview]

#### Description

The axiality plot shows deviations from an imaginary coordinate axis. The axiality shares its data structures with the [cylindricity plot](Cylindricity.md) and the [generatrix plot](Generatrix.md).

#### Point structure

* Angle (radians)
* Height
* Deviation (from axis)

The points should use `Axis` as their segment type.

#### Geometry

The plot can display a height axis. The range of the axis is determined by multiplying the points height range with the _actual_ geometries `Height` parameter. The other geometry parameters are currently unused.

#### Example

```csharp
var plot = new CylindricityPlot();
var points = new List<CylinderPoint>();

var rand = new Random( DateTime.Now.Millisecond );
var segment = new Segment( "All", SegmentTypes.Axis);

//The range of the displayed z-axis will be this height multiplied with the points height range.
plot.Actual.Height = 10;

for( var i = 0; i < count; i++ )
{
	var deviation = 0.2 + rand.NextDouble() * 0.1;
	var height = (double)i / count;

	var point = new CylinderPoint( segment, rand.NextDouble() * 0.1, height, deviation );
	points.Add( point );
}

plot.Tolerance = new Tolerance( -0.1, 0.1 );
plot.DefaultErrorScaling = 100;
plot.Points = points;
```

#### Remarks

* The points may not be located in a single X-Y plane (share one height)
* The points may have different angles, the plot can rotate them back into one plane if needed.
<br/>
<br/>
