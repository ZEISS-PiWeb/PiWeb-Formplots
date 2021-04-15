#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.Common
{
	using System.Collections.Generic;

	internal static class EnumerableExtensions
	{
		internal static IEnumerable<T> FilterNull<T>( this IEnumerable<T?> items ) where T : class
		{
			foreach( var item in items )
			{
				if( item != null )
					yield return item;
			}
		}
	}
}