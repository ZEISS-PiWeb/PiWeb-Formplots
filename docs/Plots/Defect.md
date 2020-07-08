[preview]: gfx/Defect.png "Defect file format"
<br/>

### Defect plot

![Defect file format][preview]

#### Description

The defect file format is used to transport information about one or more defects. This includes surface defects like scratches or dirt, as well as spatial defects like cavities. PiWeb Designer offers multiple elements that can read and display the data that is written in this format. Besides the general data structure, the defect file format has nothing much in common with the other formplot formats.

#### Point structure

The format defines one point for every defect, which has a `Position` and a `Size` parameter. Be aware, that the `Position` refers to the corner of the defects bounding box, which is closest to the point of origin.

![defect position](gfx/DefectPosition.png "Defect position")

Besides the `Position` and `Size` parameters, every point contains an arbitrary number of `Voxels` that define its shape. Be aware that both, the defect `Size` and `Position` as well as the voxel `Size` and `Position` are double values. We suggest to specify all positions and sizes in voxel coordinates and to specify the properties described in the [Geometry](#geometry) section.

* Position	
* Size	
* Voxels (Array)
	* Position
	* Size

>While the datatype of all parameters is a three-dimensional vector, the format is also designed for two-dimensional data. In this case, just define two dimensions instead of three.

#### Geometry

To allow PiWeb to create various visualizations of your data, you can specify additional information that helps to join the defect data with images, volumes and CAD models. All the following properties are only relevant on the `Nominal` geometry.

* `Size:` The size of the image or volume from which the defect positions and sizes originate from. PiWeb **will not** assume this size from any provided image or volume that is used in the report. We suggest to enter the size in voxels or pixels.

* `VoxelSize:` The voxel size defaults to 1 in each dimension if not set. We need this to create a discrete rasterization of the voxel data for some visualizations in PiWeb.

* `ElementSystem:` Describes the transformation from real coordinates to your defect and voxel coordinates. We need this information to display the defects in a CAD model.

#### Example

```csharp
public static Formplot Create( BitmapSource img )
{
	var plot = new DefectPlot();
	var points = new List<Defect>();

	plot.Nominal.Size = new Vector( img.PixelWidth, img.PixelHeight );

	var data = new byte[ img.PixelWidth * img.PixelHeight * 4 ];
	img.CopyPixels( data, img.PixelWidth * 4, 0 );

	var done = new HashSet<int>();

	for( var x = 0; x < img.PixelWidth; x++ )
	{
		for( var y = 0; y < img.PixelHeight; y++ )
		{
			var position = ( y * img.PixelWidth + x );
			if( done.Contains( position ) )
				continue;

			if( IsDefect( position, data ) )
				points.Add( DetectDefect( new Pixel( x, y ), data, img.PixelWidth, img.PixelHeight, done ) );
		}
	}

	plot.Points = points;
	return plot;
}
```

<details>
<summary>Helper methods</summary>

```csharp
private static Defect DetectDefect( Pixel origin, byte[] data, int pixelWidth, int pixelHeight, HashSet<int> done )
{
	var found = new List<Pixel> { origin };
	var newlyFound = new List<Pixel>( 4 ) { origin };

	while( newlyFound.Count > 0 )
	{
		var pixels = newlyFound.ToArray();
		newlyFound.Clear();
		foreach( var pixel in pixels )
		{
			foreach( var neighbor in GetNeighbors( pixel, pixelWidth, pixelHeight ) )
			{
				var position = neighbor.Y * pixelWidth + neighbor.X;
				if( done.Contains( position ) )
					continue;

				done.Add( position );

				if( !IsDefect( position, data ) )
					continue;

				found.Add( neighbor );
				newlyFound.Add( neighbor );
			}
		}
	}

	found.TrimExcess();

	var voxels = found.Select( p => new Voxel( new Vector( p.X, p.Y ), new Vector( 1, 1 ) ) ).ToArray();
	var bounds = GetBounds( voxels );
	return new Defect( new Segment( "All", SegmentTypes.None ), new Vector( bounds.X, bounds.Y ), new Vector( bounds.Width, bounds.Height ) )
	{
		Voxels = voxels
	};

}

private static IEnumerable<Pixel> GetNeighbors( Pixel p, int width, int height )
{
	if( p.X - 1 >= 0 )
		yield return new Pixel( p.X - 1, p.Y );

	if( p.X + 1 < width )
		yield return new Pixel( p.X + 1, p.Y );

	if( p.Y - 1 >= 0 )
		yield return new Pixel( p.X, p.Y - 1 );

	if( p.Y + 1 < height )
		yield return new Pixel( p.X, p.Y + 1 );
}

private static bool IsDefect( int position, byte[] data )
{

	var r = data[ position * 4 ];
	var g = data[ position * 4 + 1 ];
	var b = data[ position * 4 + 2 ];
	var a = data[ position * 4 + 3 ];

	//Let's say, everthing other than white is a defect
	return r != 255 || g != 255 || b != 255 || a != 255;
}

private static Rect GetBounds( IEnumerable<Voxel> voxels )
{
	double? minx = null, miny = null, maxx = null, maxy = null;
	foreach( var voxel in voxels )
	{
		if( minx == null || voxel.Position.X < minx )
			minx = voxel.Position.X;

		if( miny == null || voxel.Position.Y < miny )
			miny = voxel.Position.Y;

		if( maxx == null || voxel.Position.X + voxel.Size.X > maxx )
			maxx = voxel.Position.X + voxel.Size.X;

		if( maxy == null || voxel.Position.Y + voxel.Size.Y > maxy )
			maxy = voxel.Position.Y + voxel.Size.Y;
	}

	return minx.HasValue ? new Rect( minx.Value, miny.Value, maxx.Value - minx.Value, maxy.Value - miny.Value ) : Rect.Empty;
}

private struct Pixel
{
	public int X { get; }

	public int Y { get; }

	public Pixel( int x, int y )
	{
		X = x;
		Y = y;
	}
}
```
</details>

#### Remarks

To add additional information to each defect, you can use the parameter `PropertyList`. You can access the properties, as well as the size and position of the defect, with the following variables in PiWeb:

| Name									| Description 													|
|---------------------------------------|---------------------------------------------------------------|
|`${DefectCount}`						|Number of defects that are bound to the element				|
|`${Defect.Index}`						|Index of the defect in the defect file							|
|`${Defect.VoxelPosition(Dimension)}`	|Voxel position of the defect in the specified dimension		|
|`${Defect.VoxelSize(Dimension)}`		|Voxel size of the defect in the specified dimension			|
|`${Defect.ActualPosition(Dimension)}`	|Real position of the defect in the specified dimension			|
|`${Defect.ActualSize(Dimension)}`		|Real size of the defect in the specified dimension				|
|`${Defect.Tolerance(Dimension)}`		|Tolerance of the defect in the specified dimension				|
|`${Defect.SegmentName}`				|Name of the segment to which the defect is attached			|
|`${Defect.SegmentType}`				|Type of the segment to which the defect is attached			|
|`${Defect.Property("Key")}`			|Value of the property with the specified key					|
|`${Defect.PropertyDescription("Key")}`	|Description of the property with the specified key				|
|`${DefectPlot.ReferenceSize}`			|Reference size in the specified dimension						|
|`${DefectPlot.VoxelSize}`				|Size of a single voxel in the specified dimension				|

The variable names have changed in PiWeb 7.4. Older reports are converted automatically when opened with PiWeb 7.4 or newer.

The defect plot introduces a spatial tolerance type. Since there are a lot of different ways on how to interpret these tolerances, there's no built in way to evaluate tolerance usage or tolerance overrun of defects. In all report elements related to defects, you have the possiblity to use system variable expressions to filter or colorize the defects.

<br/>
<br/>
