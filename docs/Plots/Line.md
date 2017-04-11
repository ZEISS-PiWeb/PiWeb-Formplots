[preview]: gfx/Line.png "Line plot"

### Line plot

![line plot][preview]

#### Description

The line plot displays deviations from a straight line in both positive and negative direction.

#### Points

Line points consist of...
* Position (1D)
* Deviation

The positions don't have to be equidistant like in the example.

#### Geometry

The plot axis can be modified with the _actual_ geometry parameters `Length` and `Position`. The `Length` parameter will be multiplied to the points position range to determine the value range of the plot, while the `Position` parameter is added as an offset. The plots `ProjectionAxis` determines, which coordinate of the `Position` is used as offset. 

#### Example

```csharp
var plot = new StraightnessPlot();
var points = new List<LinePoint>();

var rand = new Random( DateTime.Now.Millisecond );
var segment = new Segment( "All", SegmentTypes.None );

plot.Actual.Length = 5.0; //All positions will be multiplied with the length when the plot is drawn.

//The actual position and length, together with the projection axis, will result in an offset on the plots x-axis.
//In this example, the plot's value will start at 8.0;
plot.Actual.Position = new Vector( 0, 8 );
plot.ProjectionAxis = ProjectionAxis.Y;

for( var i = 0; i < pointCount; i++ )
{
	var position = ( double ) i / pointCount;
	var deviation = 0.1 * ( Math.Sin( position * 2.0 * Math.PI ) + ( rand.NextDouble() - 0.5 ) * 0.1 );
	var point = new LinePoint( segment, position, deviation );

	points.Add( point );
}

plot.Tolerance = new Tolerance( -0.1, 0.1 );
plot.DefaultErrorScaling = 100;
plot.Points = points;
```
#### Remarks

* While the plot might have resemblance with a mathematical function plotter, PiWeb assumes that the deviations are much smaller than the position. The error scaling can't be smaller than 1.0.
