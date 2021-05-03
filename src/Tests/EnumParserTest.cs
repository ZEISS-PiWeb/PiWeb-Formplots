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

	using NUnit.Framework;
	using Formplot.Common;

	#endregion

	[TestFixture]
	public class EnumParserTest
	{
		#region methods

		[Test]
		[TestCase( "yes", TestEnum.Yes )]
		[TestCase( "Yes", TestEnum.Yes )]
		[TestCase( "YES", TestEnum.Yes )]
		public void Test_Parse( string name, TestEnum expectedEnumValue )
		{
			// act
			var enumValue = EnumParser<TestEnum>.Parse( name );

			// assert
			Assert.That( enumValue, Is.EqualTo( expectedEnumValue ) );
		}

		[Test]
		public void Test_Parse_ThrowsException()
		{
			// act/ assert
			Assert.That( () => EnumParser<TestEnum>.Parse( "None" ), Throws.ArgumentException );
		}

		[Test]
		[TestCase( "no", true, TestEnum.No )]
		[TestCase( "No", true, TestEnum.No )]
		[TestCase( "NO", true, TestEnum.No )]
		[TestCase( "None", false, TestEnum.Yes )]
		public void Test_TryParse( string name, bool expectedParseResult, TestEnum expectedEnumValue )
		{
			// act
			var parseResult = EnumParser<TestEnum>.TryParse( name, out var enumValue );

			// assert
			Assert.That( parseResult, Is.EqualTo( expectedParseResult ) );
			Assert.That( enumValue, Is.EqualTo( expectedEnumValue ) );
		}

		#endregion

		#region TestEnum enum

		public enum TestEnum
		{
			Yes,
			No
		}

		#endregion
	}
}