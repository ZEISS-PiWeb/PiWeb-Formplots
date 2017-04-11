[preview]: gfx/Pitch.png "Pitch plot"

### Pitch plot

![pitch plot][preview]

#### Description

The pitch plot shows single deviations, presented as equidistant bars on a bar chart. Unlike the [line plot](Line.md), the pitch plots axis has no dimension, but displays the point index instead.

#### Point structure

* Position (int)
* Deviation

Please note that the `Position` parameter will __not be serialized__ and therefore doesn't have to be set. Instead, the array index is used. The `Position` can be used to identify a point out of the lists context.

#### Example

```csharp
var plot = new PitchPlot();
var points = new List<PitchPoint>();

var rand = new Random( DateTime.Now.Millisecond );
var segment = new Segment( "All", SegmentTypes.None );

for( var i = 0; i < count; i++ )
{
	var deviation = 0.1 * ( Math.Sin( i * 0.05 * Math.PI ) + ( rand.NextDouble() - 0.5 ) * 0.5 );

	//The pitch point has a position property, but it won't be written. Instead, the point order matters.
	var point = new PitchPoint( segment, deviation );
	points.Add( point );
}

plot.Tolerance = new Tolerance( -0.1, 0.1 );
plot.DefaultErrorScaling = 100;
plot.Points = points;
```
