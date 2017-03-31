[logo]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/Logo.png "PiWeb Logo"
[axiality]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/AxialityplotElement.png "Axiality plot"
[pattern]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/BorepatternplotElement.png "Pattern plot"
[roundness]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/CircleplotElement.png "Roundness plot"
[circleinprofile]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/CircleprofileplotElement.png "Circle in profile plot"
[lineprofile]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/CurveplotElement.png "Line profile plot"
[cylindricity]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/CylinderplotElement.png "Cylindricity plot"
[straightness]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/LineplotElement.png "Straightness plot"
[pitch]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/PitchplotElement.png "Pitch plot"
[flatness]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/PlaneplotElement.png "Flatness plot"
[roughness]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/RoughnessplotElement.png "Roughness plot"
[generatrix]: https://github.com/ZEISS-PiWeb/PiWeb-Formplots/blob/master/docs/gfx/SurfaceLineplotElement.png "Generatrix plot"


PiWeb-Formplots
=========

![alt text][logo]

The PiWeb Formplot library helps to read and write the PiWeb formplot data format. The library provides an easy to use interface to create the different formplot types.

#### Supported formplot types



| Element type | Formplot type |
| ------------- |:-------------:| 
| ![alt text][axiality] Axiality | `Cylindricity` |
| ![alt text][circleinprofile] Circle in profile | `CircleInProfile` |  
| ![alt text][cylindricity] Cylindricity |  `Cylindricity` | 
| ![alt text][flatness] Flatness |  `Flatness` | 
| ![alt text][generatrix] Generatrix |  `Cylindricity` | 
| ![alt text][lineprofile] Line profile |  `CurveProfile` | 
| ![alt text][pattern] Pattern |  `BorePattern` | 
| ![alt text][pitch] Pitch |  `Pitch` | 
| ![alt text][roughness] Roughness |  `Straightness` | 
| ![alt text][roundness] Roundness |  `Roundness` | 
| ![alt text][straightness] Straightness |  `Straightness` | 

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
