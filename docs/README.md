[logo]: gfx/Logo.png "PiWeb Logo"
[axiality]: gfx/32px/AxialityplotElement.png "Axiality plot"
[pattern]: gfx/32px/BorepatternplotElement.png "Pattern plot"
[roundness]: gfx/32px/CircleplotElement.png "Roundness plot"
[circleinprofile]: gfx/32px/CircleprofileplotElement.png "Circle in profile plot"
[lineprofile]:gfx/32px/CurveplotElement.png "Line profile plot"
[cylindricity]: gfx/32px/CylinderplotElement.png "Cylindricity plot"
[straightness]: gfx/32px/LineplotElement.png "Straightness plot"
[pitch]: gfx/32px/PitchplotElement.png "Pitch plot"
[flatness]: gfx/32px/PlaneplotElement.png "Flatness plot"
[roughness]: gfx/32px/RoughnessplotElement.png "Roughness plot"
[generatrix]: gfx/32px/SurfaceLineplotElement.png "Generatrix plot"


PiWeb formplot library
=========

![alt text][logo]


## Overview

- [Introduction](#introduction)
- [Installation](#installation)
- [Features](#features)
- [Basic Usage](#basic-usage)
- [Learn more](#learn-more)

## Introduction

The **PiWeb formplot library** provides an easy to use interface for reading and especially writing PiWeb formplot data. PiWeb formplot files are zip-compressed archives, containing three files:

* *fileversion.txt*: Contains the formplot **file version**.
* *header.xml*: Contains **structural information**, such as geometry, tolerances, segments and the plots element system.
* *plotpoints.dat*: Contains **binary plot ploints**, composed from position, normal, deviations and others, depending on the plot type.

To simplify and shorten the progress of writing formplot files, we published the formplot library!


## Installation

The **PiWeb Formplot library** is available via [NuGet](https://www.nuget.org/packages/Zeiss.IMT.PiWeb.Formplots/):

```
PM> Install-Package Zeiss.IMT.PiWeb.Formplots
```
Or compile the library by yourself. Requirements:

* Microsoft Visual Studio 2015
* Microsoft .NET Framework v4.5

## Features

Following _element types_ with their respective _formplot types_ are supported by the library:



|  |Element type | Formplot type |
|---|------------- |-------------|
| ![][axiality]| [Axiality](#axiality) | `Cylindricity` |
| ![][circleinprofile]| [Circle in profile](#circleinprofile) | `CircleInProfile` |  
| ![][cylindricity]| [Cylindricity](#cylindricity) |  `Cylindricity` |
| ![][flatness]| [Flatness](#flatness) |  `Flatness` |
| ![][generatrix]| [Generatrix](#generatrix) |  `Cylindricity` |
| ![][lineprofile]| [Line profile](Curve.md) |  `CurveProfile` |
| ![][pattern]| [Pattern](#pattern) |  `BorePattern` |
| ![][pitch]| [Pitch](#pitch) |  `Pitch` |
| ![][roughness]| [Roughness](#roughness) |  `Straightness` |
| ![][roundness]| [Roundness](#roundness) |  `Roundness` |
| ![][straightness]| [Straightness](#straightness) |  `Straightness` |

## Basic Usage

1. Create a plot of the desired plot type and a list of plot points
```csharp
var plot = new StraightnessPlot();
var points = new List<LinePoint>();
```
2. Fill your point array
```csharp
for( var i = 0; i < pointCount; i++ )
{
	points.Add( new LinePoint( segment, position, deviation ) );
}
```
3. Add the point list to your plot
```csharp
plot.Points = points;
```
4. Write your plot file, e.g. using the [PiWeb API](https://github.com/ZEISS-PiWeb/PiWeb-Api)
```csharp
plot.WriteTo( outputStream );
```

## Learn more

>Create a documentation here
