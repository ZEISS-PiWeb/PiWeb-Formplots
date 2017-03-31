![alt text][logo]

[logo]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/Logo.png "PiWeb Logo"

```csharp
var plot = new StraightnessPlot();
var points = new List<LinePoint>();

var rand = new Random( DateTime.Now.Millisecond );
var segment = new Segment( "All", SegmentTypes.None );

for( var i = 0; i < 100; i++ )
{
  var position = ( double ) i / 100 * 3;
  var deviation = 0.1 * ( Math.Sin( position * 2.0 * Math.PI ) + ( rand.NextDouble() - 0.5 ) * 0.5 );
  var point = new LinePoint( segment, position, deviation );

  points.Add( point );
}

plot.Tolerance = new Tolerance( -0.1, 0.1 );
plot.DefaultErrorScaling = 100;
plot.Points = points;

plot.WriteTo( outputStream );
```
