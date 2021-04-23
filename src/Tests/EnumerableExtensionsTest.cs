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
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using AutoFixture;
	using NUnit.Framework;
	using Zeiss.PiWeb.Formplot.Common;

	#endregion

	[TestFixture]
	public class EnumerableExtensionsTest
	{
		#region members

		private static readonly Fixture Fixture = new Fixture();

		private static readonly IEnumerable FilterNullFilterTestCases = CreateFilterNullFilterTestCases<string>();

		#endregion

		#region methods

		[Test]
		[TestCaseSource( nameof( FilterNullFilterTestCases ) )]
		public void Test_FilterNull( EnumerableFilterTestCase<string> testCase )
		{
			Assert.That( testCase.Enumerable.FilterNull(), Is.EquivalentTo( testCase.ExpectedResult ) );
		}

		private static IEnumerable CreateFilterNullFilterTestCases<T>()
			where T : class
		{
			const int numberOfEntries = 5;
			var multipleValueList = Fixture.CreateMany<T>( numberOfEntries ).ToList();
			yield return new EnumerableFilterTestCase<T> { Enumerable = multipleValueList, ExpectedResult = multipleValueList };

			var singleValueList = Fixture.CreateMany<T>( 1 ).ToList();
			yield return new EnumerableFilterTestCase<T> { Enumerable = singleValueList, ExpectedResult = singleValueList };

			var valueList = Fixture.CreateMany<T>( numberOfEntries ).ToList();
			var mixedValueList = Shuffle( new List<T>( valueList ).Append( null ) );
			yield return new EnumerableFilterTestCase<T> { Enumerable = mixedValueList, ExpectedResult = valueList };

			yield return new EnumerableFilterTestCase<T> { Enumerable = new List<T> { null, null, null }, ExpectedResult = Enumerable.Empty<T>() };
			yield return new EnumerableFilterTestCase<T> { Enumerable = new List<T>(), ExpectedResult = Enumerable.Empty<T>() };
		}


		private static IEnumerable<T> Shuffle<T>( IEnumerable<T> source )
		{
			return source
				.ToDictionary( x => Fixture.Create<Guid>(), x => x )
				.OrderBy( x => x.Key )
				.Select( x => x.Value );
		}

		#endregion

		#region class EnumerableFilterTestCase

		public class EnumerableFilterTestCase<T> : EnumerableFilterTestCase<T, T>
		{ }

		public class EnumerableFilterTestCase<TInput, TResult>
		{
			#region properties

			public IEnumerable<TInput> Enumerable { get; set; }

			public IEnumerable<TResult> ExpectedResult { get; set; }

			#endregion
		}

		#endregion
	}
}