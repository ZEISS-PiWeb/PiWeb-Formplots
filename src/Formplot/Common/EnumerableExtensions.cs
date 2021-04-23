#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2012                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.Common
{
	using System.Collections.Generic;

	internal static class EnumerableExtensions
	{
		#region methods

		/// <summary>
		/// Removes all null values from a given <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <param name="items">Will be filtered.</param>
		/// <typeparam name="T">Type of the contained items.</typeparam>
		/// <returns>An enumerable containing all non-null items.</returns>
		internal static IEnumerable<T> FilterNull<T>( this IEnumerable<T?> items )
			where T : class
		{
			foreach( var item in items )
			{
				if( item != null )
					yield return item;
			}
		}

		#endregion
	}
}