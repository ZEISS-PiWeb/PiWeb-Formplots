#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.Tests
{
	#region usings

	using System;
	using NUnit.Framework;
	using Zeiss.PiWeb.Formplot.Common;

	#endregion

	[TestFixture]
	public class DoubleCompareExtensionsTest
	{
		#region methods

		[Test]
		[TestCase( 1, 1, ExpectedResult = true )]
		[TestCase( 1, 2, ExpectedResult = false )]
		[TestCase( 0.1, 0.1, ExpectedResult = true )]
		[TestCase( 0.1, 0.2, ExpectedResult = false )]
		[TestCase( -1, -1, ExpectedResult = true )]
		[TestCase( -1, -2, ExpectedResult = false )]
		[TestCase( -0.1, -0.1, ExpectedResult = true )]
		[TestCase( -0.1, -0.2, ExpectedResult = false )]
		[TestCase( -0.1, 0.1, ExpectedResult = false )]
		[TestCase( 0.0, 0.0, ExpectedResult = true )]
		[TestCase( double.NaN, 1, ExpectedResult = false )]
		[TestCase( double.NaN, double.NaN, ExpectedResult = false )]
		[TestCase( double.PositiveInfinity, 1, ExpectedResult = false )]
		[TestCase( double.NegativeInfinity, 1, ExpectedResult = false )]
		[TestCase( double.PositiveInfinity, double.PositiveInfinity, ExpectedResult = true )]
		[TestCase( double.NegativeInfinity, double.NegativeInfinity, ExpectedResult = true )]
		[TestCase( double.PositiveInfinity, double.NegativeInfinity, ExpectedResult = false )]
		[TestCase( double.NegativeInfinity, double.PositiveInfinity, ExpectedResult = false )]
		public bool IsCloseToFloatingPointNumberTest( double x, double y )
		{
			return x.IsCloseTo( y );
		}

		[Test]
		[TestCase( 1, 1, ExpectedResult = true )]
		[TestCase( 1, 2, ExpectedResult = false )]
		[TestCase( 0.1, 0.1, ExpectedResult = true )]
		[TestCase( 0.1, 0.2, ExpectedResult = false )]
		[TestCase( -1, -1, ExpectedResult = true )]
		[TestCase( -1, -2, ExpectedResult = false )]
		[TestCase( -0.1, -0.1, ExpectedResult = true )]
		[TestCase( -0.1, -0.2, ExpectedResult = false )]
		[TestCase( -0.1, 0.1, ExpectedResult = false )]
		[TestCase( 0.0, 0.0, ExpectedResult = true )]
		[TestCase( null, null, ExpectedResult = true )]
		[TestCase( 1, null, ExpectedResult = false )]
		[TestCase( double.NaN, 1, ExpectedResult = false )]
		[TestCase( double.NaN, double.NaN, ExpectedResult = false )]
		[TestCase( double.PositiveInfinity, 1, ExpectedResult = false )]
		[TestCase( double.NegativeInfinity, 1, ExpectedResult = false )]
		[TestCase( double.PositiveInfinity, double.PositiveInfinity, ExpectedResult = true )]
		[TestCase( double.NegativeInfinity, double.NegativeInfinity, ExpectedResult = true )]
		[TestCase( double.PositiveInfinity, double.NegativeInfinity, ExpectedResult = false )]
		[TestCase( double.NegativeInfinity, double.PositiveInfinity, ExpectedResult = false )]
		public bool IsCloseToNullableFloatingPointNumberTest( double? x, double? y )
		{
			return x.IsCloseTo( y );
		}

		/// <summary>
		/// Verifies a known issue between .NET CLR x86 and x64. For x64 double roundtrip conversion via string is not safe. This is not
		/// going to be fixed by Microsoft because "we cannot fix this issue because it may affect the behaviour of existing programs".
		/// The official workaround is to use G17.
		/// https://stackoverflow.com/questions/24299692/why-is-a-round-trip-conversion-via-a-string-not-safe-for-a-double
		/// http://www.beta.microsoft.com/VisualStudio/feedbackdetail/view/914964/double-round-trip-conversion-via-a-string-is-not-safe
		/// If this test fails sometime in the future, maybe Microsoft has fixed the issue after all.
		/// </summary>
		[Test]
		public void DoubleRoundTripConversionTest()
		{
			var originValues = new[] { 0.84551240822557006 };
			foreach( var originValue in originValues )
			{
				var stringValue = originValue.ToString( "R" );
				var newValue = double.Parse( stringValue );

				Assert.True( Math.Abs( originValue - newValue ) < double.Epsilon );
				Assert.True( originValue.IsCloseTo( newValue ) );

				stringValue = originValue.ToString( "G17" );
				newValue = double.Parse( stringValue );

				Assert.True( Math.Abs( originValue - newValue ) < double.Epsilon );
				Assert.True( originValue.IsCloseTo( newValue ) );
			}
		}

		#endregion
	}
}