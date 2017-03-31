[logo]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/Logo.png "PiWeb Logo"
[axiality]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/AxialityplotElement.png "Axiality plot"
[pattern]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/BorepatternplotElement.png "Pattern plot"
[roundness]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/CircleplotElement.png "Roundness plot"
[circleinprofile]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/CircleprofileplotElement.png "Circle in profile plot"
[lineprofile]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/CurveplotElement.png "Line profile plot"
[cylindricity]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/CylinderplotElement.png "Cylindricity plot"
[straightness]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/LineplotElement.png "Straightness plot"
[pitch]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/PitchplotElement.png "Pitch plot"
[flatness]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/PlaneplotElement.png "Flatness plot"
[roughness]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/RoughnessplotElement.png "Roughness plot"
[generatrix]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/32px/SurfaceLineplotElement.png "Generatrix plot"


PiWeb-Formplots
=========

![alt text][logo]

The PiWeb Formplot library helps to read and write the PiWeb formplot data format. The library provides an easy to use interface to create the different formplot types.

#### Supported formplot types



|  |Element type | Formplot type |
|---|------------- |-------------| 
| ![][axiality]| Axiality | `Cylindricity` |
| ![][circleinprofile]| Circle in profile | `CircleInProfile` |  
| ![][cylindricity]| Cylindricity |  `Cylindricity` | 
| ![][flatness]| Flatness |  `Flatness` | 
| ![][generatrix]| Generatrix |  `Cylindricity` | 
| ![][lineprofile]| Line profile |  `CurveProfile` | 
| ![][pattern]| Pattern |  `BorePattern` | 
| ![][pitch]| Pitch |  `Pitch` | 
| ![][roughness]| Roughness |  `Straightness` | 
| ![][roundness]| Roundness |  `Roundness` | 
| ![][straightness]| Straightness |  `Straightness` | 

#### Creating a formplot and writing it to a stream:

```cs
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


#### Data format:
