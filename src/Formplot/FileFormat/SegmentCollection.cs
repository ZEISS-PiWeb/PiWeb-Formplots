#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019-2021                        */
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
	/// A list of segments that automatically manages the hierarchy between segments and plots.
	/// </summary>
	public sealed class SegmentCollection<TPoint, TGeometry> : IReadOnlyList<Segment<TPoint, TGeometry>>, IList<Segment<TPoint, TGeometry>>
		where TPoint : Point<TPoint, TGeometry>, new()
		where TGeometry : Geometry, new()
	{
		#region members

		private readonly List<Segment<TPoint, TGeometry>> _Segments = new List<Segment<TPoint, TGeometry>>();
		private readonly Formplot<TPoint, TGeometry> _Formplot;

		#endregion

		#region constructors

		/// <summary>Constructor.</summary>
		/// <param name="formplot">The formplot to which this collection belongs.</param>
		public SegmentCollection( Formplot<TPoint, TGeometry> formplot )
		{
			_Formplot = formplot;
		}

		#endregion

		#region methods

		/// <summary>
		/// Creates a segment with the specified <paramref name="name"/> and <paramref name="type"/> and adds it to the collection.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="type">Type.</param>
		/// <returns>New instance.</returns>
		public Segment<TPoint, TGeometry> Create( string name, SegmentTypes type )
		{
			var segment = new Segment<TPoint, TGeometry>( name, type );
			Add( segment );

			return segment;
		}

		#endregion

		#region interface IList<Segment<TPoint,TGeometry>>

		/// <inheritdoc />
		public void Clear()
		{
			foreach( var segment in _Segments )
				segment.Formplot = null;

			_Segments.Clear();
		}

		/// <inheritdoc />
		public bool Contains( Segment<TPoint, TGeometry> segment )
		{
			return _Segments.Contains( segment );
		}

		/// <inheritdoc />
		public void CopyTo( Segment<TPoint, TGeometry>[] array, int arrayIndex )
		{
			_Segments.CopyTo( array, arrayIndex );
		}

		/// <inheritdoc />
		public bool Remove( Segment<TPoint, TGeometry>? segment )
		{
			if( segment == null )
				return false;

			_Segments.Remove( segment );
			segment.Formplot = null;

			return true;
		}

		/// <inheritdoc />
		public bool IsReadOnly => false;

		/// <inheritdoc />
		public void Add( Segment<TPoint, TGeometry>? segment )
		{
			if( segment == null )
				throw new ArgumentNullException( nameof( segment ) );

			segment.Formplot?.Segments.Remove( segment );
			segment.Formplot = _Formplot;

			_Segments.Add( segment );
		}

		/// <inheritdoc />
		public int IndexOf( Segment<TPoint, TGeometry> item )
		{
			return _Segments.IndexOf( item );
		}

		/// <inheritdoc />
		public void Insert( int index, Segment<TPoint, TGeometry> segment )
		{
			if( segment == null )
				throw new ArgumentNullException( nameof( segment ) );

			segment.Formplot?.Segments.Remove( segment );
			segment.Formplot = _Formplot;

			_Segments.Insert( index, segment );
		}

		/// <inheritdoc />
		public void RemoveAt( int index )
		{
			var segment = this[ index ];
			segment.Formplot = null;

			_Segments.RemoveAt( index );
		}

		#endregion

		#region interface IReadOnlyList<Segment<TPoint,TGeometry>>

		/// <inheritdoc cref="IReadOnlyCollection{T}" />
		public int Count => _Segments.Count;

		/// <inheritdoc />
		public IEnumerator<Segment<TPoint, TGeometry>> GetEnumerator()
		{
			return _Segments.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc cref="IReadOnlyCollection{T}" />
		public Segment<TPoint, TGeometry> this[ int index ]
		{
			get => _Segments[ index ];
			set
			{
				if( value == null )
					throw new ArgumentNullException( nameof( value ) );

				_Segments[ index ].Formplot = null;

				value.Formplot?.Segments.Remove( value );
				value.Formplot = _Formplot;

				_Segments[ index ] = value;
			}
		}

		#endregion
	}
}