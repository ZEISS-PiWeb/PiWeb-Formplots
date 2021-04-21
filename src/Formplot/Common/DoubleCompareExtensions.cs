#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.Common
{
	#region usings

	using System;

	#endregion

	/// <summary>
	/// Class containing helper methods for comparing doubles.
	/// (see https://msdn.microsoft.com/en-us/library/ya2zha7s(v=vs.110).aspx, Technique 1).
	///
	/// Comparing Nullables is done like in .NET Framework:
	/// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/nullable-types/using-nullable-types
	///
	/// Comparing nullables returns:
	/// 
	/// double? num1 = 10;
	/// double? num2 = null;
	/// double? num3 = null;
	/// 
	/// num1 == num2 = False
	/// num1 != num2 = True
	/// num1 >  num2 = False
	/// num1 >= num2 = False
	/// num1 &lt;  num2 = False
	/// num1 &lt;= num2 = False
	/// 
	/// num3 == num2 = True
	/// num3 != num2 = False
	/// num3 >  num2 = False
	/// num3 >= num2 = False
	/// num3 &lt;  num2 = False
	/// num3 &lt;= num2 = False
	/// </summary>
	internal static class DoubleCompareExtensions
	{
		#region constants

		internal const double DefaultPrecision = 1e-15;

		#endregion

		#region methods

		/// <summary>
		/// Checks if the given doubles can be considered "equal" under the given tolerance.
		/// </summary>
		/// <param name="x">First value.</param>
		/// <param name="y">Second value.</param>
		/// <param name="tolerance">Equality tolerance.</param>
		/// <returns>
		/// True if the distance between both values is smaller than the given tolerance. Otherwise false.
		/// </returns>
		internal static bool IsCloseTo( this double x, double y, double tolerance = DefaultPrecision )
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			return x == y || Math.Abs( x - y ) <= tolerance;
		}
		
		/// <summary>
		/// Checks if the given doubles can be considered "equal" under the given tolerance.
		/// </summary>
		/// <param name="x">First value.</param>
		/// <param name="y">Second value.</param>
		/// <param name="tolerance">Equality tolerance.</param>
		/// <returns>
		/// True if both are null or the distance between them is smaller than the given tolerance. Otherwise false.
		/// </returns>
		internal static bool IsCloseTo( this double? x, double? y, double tolerance = DefaultPrecision )
		{
			if( !x.HasValue && !y.HasValue )
				return true;
			
			if( !x.HasValue || !y.HasValue ) 
				return false;

			return IsCloseTo( x.Value, y.Value, tolerance );
		}

		#endregion
	}
}