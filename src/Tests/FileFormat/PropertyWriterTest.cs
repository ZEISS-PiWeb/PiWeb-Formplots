namespace Zeiss.PiWeb.Formplot.Tests.FileFormat
{
	using System.IO;
	using NUnit.Framework;
	using Zeiss.PiWeb.Formplot.FileFormat;

	[TestFixture]
	public class PropertyWriterTest
	{
		#region methods

		[Test]
		public void Test_RangeLists()
		{
			const int pointCount = 10;
			const string propertyName = "Any";

			var plot = new StraightnessPlot();
			var segment = new Segment<LinePoint, LineGeometry>( string.Empty, SegmentTypes.Line );

			for( var i = 0; i < pointCount; i++ )
			{
				var point = new LinePoint( 0, 0 );
				point.Properties.Add( Property.Create( propertyName, i % 2 ) );
				segment.Points.Add( point );
			}

			plot.Segments.Add( segment );
			var stream = new MemoryStream();
			plot.WriteTo( stream );
			stream.Seek( 0, SeekOrigin.Begin );
			var clone = Formplot.ReadFrom<StraightnessPlot>( stream );

			Assert.That( clone, Is.Not.Null );
			Assert.That( clone.Segments.Count, Is.EqualTo( 1 ) );
			Assert.That( clone.Segments[ 0 ].Points.Count, Is.EqualTo( pointCount ) );

			for( var i = 0; i < 10; i++ )
			{
				Assert.That( clone.Segments[ 0 ].Points[ i ].Properties[ propertyName ]?.Value, Is.EqualTo( i % 2 ) );
			}
		}

		#endregion
	}
}