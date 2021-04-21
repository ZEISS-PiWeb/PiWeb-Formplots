#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 20120-2021                        */
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