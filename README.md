[![build on develop](https://github.com/ZEISS-PiWeb/PiWeb-Formplots/actions/workflows/develop.yml/badge.svg?branch=develop&event=push)](https://github.com/ZEISS-PiWeb/PiWeb-Formplots/actions/workflows/develop.yml)
[![License](https://img.shields.io/badge/License-BSD%203--Clause-blue.svg)](https://opensource.org/licenses/mit)
[![NuGet](https://img.shields.io/nuget/v/Zeiss.PiWeb.Formplot?logo=nuget)](https://www.nuget.org/packages/Zeiss.PiWeb.Formplot/)

[logo]: docs/img/Logo.png "PiWeb Logo"
[axiality]: docs/img/32px/AxialityplotElement.png "Axiality plot"
[pattern]: docs/img/32px/BorepatternplotElement.png "Pattern plot"
[roundness]: docs/img/32px/CircleplotElement.png "Roundness plot"
[circleinprofile]: docs/img/32px/CircleprofileplotElement.png "Circle in profile plot"
[lineprofile]:docs/img/32px/CurveplotElement.png "Line profile plot"
[cylindricity]: docs/img/32px/CylinderplotElement.png "Cylindricity plot"
[straightness]: docs/img/32px/LineplotElement.png "Straightness plot"
[pitch]: docs/img/32px/PitchplotElement.png "Pitch plot"
[flatness]: docs/img/32px/PlaneplotElement.png "Flatness plot"
[roughness]: docs/img/32px/RoughnessplotElement.png "Roughness plot"
[generatrix]: docs/img/32px/SurfaceLineplotElement.png "Generatrix plot"
[fourier]: docs/img/32px/FourierplotElement.png "Fourier plot"
[defect]: docs/img/32px/DefectElement.png "Defect image"


# PiWeb Formplot Library

| ![Zeiss IQS Logo](docs/img/logo_128x128.png) | The **PiWeb formplot library** provides an easy to use interface for reading and especially writing formplot data for the quality data management system [ZEISS PiWeb](http://www.zeiss.com/industrial-metrology/en_de/products/software/piweb.html). |
|-|:-|


# Overview

- [Introduction](#introduction)
- [Installation](#installation)
- [Basic Usage](#basic-usage)
- [Manual](#manual)
	- [Formplot types](#formplot-types)
	- [Properties](#properties)
	- [Writing plots into PiWeb](#writing-plots-into-piWeb)
- [Contributing](#contributing)


# Introduction

PiWeb formplot files are zip-compressed archives, containing three files:

* *fileversion.txt*: Contains the formplot **file version**.
* *header.xml*: Contains **structural information**, such as geometry, tolerances, segments and the plots element system.
* *plotpoints.dat*: Contains **binary plot ploints**, composed from position, normal, deviations and others, depending on the plot type.

To simplify and shorten the progress of writing formplot files, we published the formplot library!


# Installation

The **PiWeb Formplot library** is available via NuGet.


## Versions >= 2.0.0

Get it at [NuGet.org](https://www.nuget.org/packages/Zeiss.PiWeb.Formplot/).

```cmd
PM> Install-Package Zeiss.PiWeb.Formplot
```

Or compile the library by yourself. Requirements:

* Microsoft Visual Studio 2019
* Microsoft .NET Standard 2.0


## Older versions

Get it at [NuGet](https://www.nuget.org/packages/Zeiss.IMT.PiWeb.Formplots/).

```
PM> Install-Package Zeiss.IMT.PiWeb.Formplots
```

Or compile the library by yourself. Requirements:

* Microsoft Visual Studio 2015
* Microsoft .NET Framework v4.5

# Basic Usage

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

# Manual

## Formplot types

Following _element types_ with their respective _formplot types_ are supported by the library. Please use the links in the table for detailed information about certain plot types:

|  |Element type | Formplot type |
|---|------------- |-------------|
| ![][axiality]| [Axiality](docs/Plots/Axiality.md) | `Cylindricity` |
| ![][circleinprofile]| [Circle in profile](docs/Plots/CircleInProfile.md) | `CircleInProfile` |  
| ![][cylindricity]| [Cylindricity](docs/Plots/Cylindricity.md) |  `Cylindricity` |
| ![][fourier]| [Fourier](docs/Plots/Fourier.md) |  `Fourier` |
| ![][generatrix]| [Generatrix](docs/Plots/Generatrix.md) |  `Cylindricity` |
| ![][lineprofile]| [Line profile](docs/Plots/LineProfile.md) |  `CurveProfile` |
| ![][pattern]| [Pattern](docs/Plots/Pattern.md) |  `BorePattern` |
| ![][pitch]| [Pitch](docs/Plots/Pitch.md) |  `Pitch` |
| ![][flatness]| [Plane](docs/Plots/Plane.md) |  `Flatness` |
| ![][roughness]| [Roughness](docs/Plots/Line.md) |  `Straightness` |
| ![][roundness]| [Roundness](docs/Plots/Circle.md) |  `Roundness` |
| ![][straightness]| [Straightness](docs/Plots/Line.md) |  `Straightness` |
| ![][defect]| [Defect](docs/Plots/Defect.md) |  `Defect` |

## Properties

Formplots may contain additional properties that are accessible by PiWeb. You can add properties to your plot like the following:

```csharp
plot.Properties.Add( Property.Create( "myPropertyKey", "myPropertyValue", "propertyDescription" ) );
```
The available datatypes are `string`, `long`, `double`, `DateTime` and `TimeSpan`. The description is optional.
You can access the properties in PiWeb by using the following expression. 

```csharp
${Qdb.Property("My property")}
```

> The element must have its databinding set to the measurement value containing the formplot.

## Writing plots into PiWeb

You can either upload formplot files manually by using the PiWeb Planner, or by using the PiWeb API like the following:

```csharp
public static async Task WriteToDatabase( 
	Formplot plot, 
	RawDataServiceRestClient rawClient, 
	Guid measurementUuid, 
	Guid characteristicUuid )
{
	using( var stream = new MemoryStream() )
	{
		plot.WriteTo( stream );
		var data = stream.ToArray();

		var target = RawDataTargetEntity.CreateForValue( measurementUuid, characteristicUuid );

		await rawClient.CreateRawData( new RawDataInformation
		{
			FileName = "plot.pltx",								
			MimeType = "application/x-zeiss-piweb-formplot",
			Key = -1,
			Created = DateTime.Now,
			LastModified = DateTime.Now,
			MD5 = new Guid( MD5.Create().ComputeHash( data ) ),
			Size = data.Length,
			Target = target
		}, data );
	}
}
```

>Please note that, in any case, The file extension and the mimetype are *mandatory*. Files with other mimetypes or extensions will not be recognized as formplot data.


# Contributing

This repository makes use of resuable workflows from [ZEISS-PiWeb/github-actions](https://github.com/ZEISS-PiWeb/github-actions). Read the documentation (especially about automated semantic versioning) before committing any changes.