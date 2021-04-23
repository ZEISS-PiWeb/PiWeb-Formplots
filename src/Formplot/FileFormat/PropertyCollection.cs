#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
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
		/// Gets the property with the given <c>name</c>. Accessing the property is done efficiently via hash table.
		/// </summary>
		public Property? this[ string name ] => _Map.TryGetValue( name, out var result ) ? result : null;

		#endregion

		#region methods

		/// <summary>
		/// Adds the given properties to this collection.
		/// </summary>
		/// <param name="properties">Will be added.</param>
		public void AddRange( IEnumerable<Property> properties )
		{
			foreach( var prop in properties )
			{
				Add( prop );
			}
		}

		/// <summary>
		/// Clears this collection and adds the given property.
		/// </summary>
		/// <param name="prop">Replaces all other entries.</param>
		public void Set( Property prop )
		{
			Clear();
			Add( prop );
		}

		/// <summary>
		/// Cle
		/// Clears this collection and adds the given properties.
		/// </summary>
		/// <param name="prop">Replace all other entries.</param>
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