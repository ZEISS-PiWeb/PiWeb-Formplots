﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.Common
{
	#region usings

	using System;
	using System.Collections.Generic;

	#endregion

	/// <summary>
	/// Responsible for parsing enums.
	/// </summary>
	internal static class EnumParser<T>
		where T : struct
	{
		#region members

		private static readonly Dictionary<string, T> Items = new Dictionary<string, T>( StringComparer.OrdinalIgnoreCase );

		#endregion

		#region methods

		/// <summary>
		/// Equivalent to <see cref="Enum.Parse(System.Type,string)"/>.
		/// </summary>
		internal static T Parse( string value )
		{
			lock( Items )
			{
				if( Items.TryGetValue( value, out var result ) )
					return result;

				return Items[ value ] = (T)Enum.Parse( typeof( T ), value, true );
			}
		}

		/// <summary>
		/// Equivalent to <see cref="System.Enum.TryParse{TEnum}(string?,bool,out TEnum)"/>.
		/// </summary>
		internal static bool TryParse( string value, out T enumValue )
		{
			lock( Items )
			{
				if( Items.TryGetValue( value, out enumValue ) )
					return true;

				if( !Enum.TryParse( value, true, out enumValue ) )
					return false;

				Items[ value ] = enumValue;

				return true;
			}
		}

		#endregion
	}
}