#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Collections;
	using System.Collections.Generic;

	#endregion

	/// <summary>
	/// A collection of key-value pairs that can be used to store additional metadata.
	/// </summary>
	public sealed class PropertyCollection : ICollection<Property>
	{
		#region members

		private readonly Dictionary<string, Property> _Map = new Dictionary<string, Property>();

		#endregion

		#region properties

		/// <summary>
		/// Gibt das Property mit dem Namen <code>name</code> zurück. Der Zugriff
		/// auf das entsprechende Property geschieht über eine Hash-Tabelle und ist
		/// somit sehr effizient.
		/// </summary>
		public Property? this[ string name ] => _Map.TryGetValue( name, out var result ) ? result : null;

		#endregion

		#region methods

		/// <summary>
		/// Fügt die angegebenen Properties zu dieser Collection hinzu.
		/// </summary>
		/// <param name="properties">Die hinzuzufügenden Properties.</param>
		public void AddRange( IEnumerable<Property> properties )
		{
			foreach( var prop in properties )
			{
				Add( prop );
			}
		}

		/// <summary>
		/// Leert die Liste und fügt ihr die angegebene Property hinzu,
		/// </summary>
		/// <param name="prop"></param>
		public void Set( Property prop )
		{
			Clear();
			Add( prop );
		}

		/// <summary>
		/// Leert die Liste und fügt ihr die angegebene Properties hinzu,
		/// </summary>
		/// <param name="prop"></param>
		public void Set( IEnumerable<Property> prop )
		{
			Clear();
			AddRange( prop );
		}

		#endregion

		#region interface ICollection<Property>

		/// <inheritdoc />
		public void Add( Property item )
		{
			if( item == null )
				throw new ArgumentNullException( nameof( item ) );

			// Avoid duplicates
			_Map[ item.Name ] = item;
		}

		/// <inheritdoc />
		public void Clear()
		{
			_Map.Clear();
		}

		/// <inheritdoc />
		public bool Contains( Property item )
		{
			if( item == null )
				throw new ArgumentNullException( nameof( item ) );

			return _Map.ContainsKey( item.Name );
		}

		/// <inheritdoc />
		public void CopyTo( Property[] array, int arrayIndex )
		{
			_Map.Values.CopyTo( array, arrayIndex );
		}

		/// <inheritdoc />
		public int Count => _Map.Count;

		/// <inheritdoc />
		public bool IsReadOnly => false;

		/// <inheritdoc />
		public bool Remove( Property prop )
		{
			if( prop == null )
				throw new ArgumentNullException( nameof( prop ) );

			return _Map.Remove( prop.Name );
		}

		/// <inheritdoc />
		public IEnumerator<Property> GetEnumerator()
		{
			return _Map.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}