#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.Common
{
	#region usings

	using System;
	using System.Collections.Generic;

	#endregion

	/// <summary>
	/// Helferklasse zum Parsen von Enums.
	/// </summary>
	internal static class EnumParser<T> where T : struct
	{
		#region members

		private static readonly Dictionary<string, T> Items = new Dictionary<string, T>( StringComparer.OrdinalIgnoreCase );

		#endregion

		#region methods

		/// <summary>
		/// Alternative zu <see cref="System.Enum.Parse(System.Type,string)"/>. Spart das doppelte Schreiben des Klassennamens.
		/// </summary>
		internal static T Parse( string value )
		{
			T result;
			lock( Items )
			{
				if( !Items.TryGetValue( value, out result ) )
					result = Items[ value ] = ( T ) Enum.Parse( typeof( T ), value, true );
			}
			return result;
		}

		/// <summary>
		/// Alternative zu <see cref="System.Enum.Parse(System.Type,string)"/>. Spart das doppelte Schreiben des Klassennamens.
		/// </summary>
		internal static bool TryParse( string value, out T enumValue )
		{
			var result = false;

			lock( Items )
			{
				if( !Items.TryGetValue( value, out enumValue ) )
				{
					if( Enum.TryParse( value, true, out enumValue ) )
					{
						Items[ value ] = enumValue;
						result = true;
					}
				}
				else
				{
					result = true;
				}
			}

			return result;
		}

		#endregion
	}
}