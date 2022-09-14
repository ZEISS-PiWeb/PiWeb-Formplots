#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.Tests.FileFormat
{
	#region usings

	using System;
	using System.IO;
	using NUnit.Framework;
	using Zeiss.PiWeb.Formplot.FileFormat;

	#endregion

	[TestFixture]
	public class CurveDistancePlotTest
	{
		#region members

		private static readonly string TestDataDirectory = Path.Combine( TestContext.CurrentContext.TestDirectory, "TestData" );

		#endregion

		[Test]
		public void Test_ConversionFromCurves()
		{
			var file = Path.Combine( TestDataDirectory, "pltx", "curve_distance_from_curves.pltx" );
			using var stream = File.OpenRead( file );
			var deserialized = Formplot.ReadFrom<CurveDistancePlot>( stream, true );

			Assert.That( deserialized, Is.Not.Null );
			Assert.That( deserialized.Segments, Has.Count.EqualTo( 1 ) );
		}
	}
}