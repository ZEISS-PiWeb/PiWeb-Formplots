#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	#region usings

	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	#endregion

	/// <summary>
	/// Represents a list of ranges
	/// </summary>
	public class RangeList : List<Range>
	{
		#region methods

		/// <summary>
		/// Enumerates all <paramref name="rangeLists"/> keys whose values contain the <paramref name="value"/>.
		/// </summary>
		internal static IEnumerable<T> GetKeysInRange<T>( Dictionary<T, RangeList> rangeLists, int value )
		{
			return from kvp in rangeLists where kvp.Value.Any( range => range.IsValueInRange( value ) ) select kvp.Key;
		}

		/// <summary>
		/// Tries to parse a new <see cref="RangeList"/> instance from the <paramref name="rangesString"/>.
		/// <remarks>In case <paramref name="rangesString"/> doesn't define a valid range, an empty <see cref="RangeList"/> instance is returned.</remarks>
		/// </summary>
		internal static RangeList TryParseOrEmpty( string rangesString )
		{
			var returnValue = new RangeList();
			var rangeStrings = rangesString.Split( ';' );

			returnValue.AddRange( rangeStrings.Select( Range.TryParseOrNull ).Where( range => range != null ) );

			return returnValue;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			var rangeString = new StringBuilder();

			foreach( var range in this )
			{
				if( rangeString.Length > 0 )
				{
					rangeString.Append( ";" );
				}

				rangeString.Append( range );
			}

			return rangeString.ToString();
		}

		#endregion
	}
}