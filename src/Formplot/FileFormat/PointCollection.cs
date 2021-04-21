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
	/// A list of points that automatically manages the hierarchy between points and segments.
	/// </summary>
	public sealed class PointCollection<TPoint, TGeometry> : IReadOnlyList<TPoint>, IList<TPoint>
		where TPoint : Point<TPoint, TGeometry>, new()
		where TGeometry : Geometry, new()
	{
		#region members

		private readonly List<TPoint> _Points = new List<TPoint>();
		private readonly Segment<TPoint, TGeometry> _Segment;

		#endregion

		#region constructors

		/// <summary>
		/// </summary>
		/// <param name="segment">The segment to which this point collection belongs.</param>
		public PointCollection( Segment<TPoint, TGeometry> segment )
		{
			_Segment = segment;
		}

		#endregion

		#region interface IList<TPoint>

		/// <inheritdoc/>>
		public void Clear()
		{
			foreach( var point in _Points )
				point.Segment = null;

			_Points.Clear();
		}

		/// <inheritdoc/>>
		public bool Contains( TPoint point )
		{
			return _Points.Contains( point );
		}

		/// <inheritdoc/>>
		public void CopyTo( TPoint[] array, int arrayIndex )
		{
			_Points.CopyTo( array, arrayIndex );
		}

		/// <inheritdoc/>>
		public bool Remove( TPoint point )
		{
			_Points.Remove( point );
			point.Segment = null;

			return true;
		}

		/// <inheritdoc/>>
		public bool IsReadOnly => false;

		/// <inheritdoc/>>
		public void Add( TPoint point )
		{
			point.Segment?.Points.Remove( point );

			_Points.Add( point );
			point.Segment = _Segment;
		}

		/// <inheritdoc />
		public int IndexOf( TPoint point )
		{
			return _Points.IndexOf( point );
		}

		/// <inheritdoc />
		public void Insert( int index, TPoint point )
		{
			point.Segment?.Points.Remove( point );

			_Points.Insert( index, point );
			point.Segment = _Segment;
		}

		/// <inheritdoc />
		public void RemoveAt( int index )
		{
			var point = this[ index ];
			point.Segment = null;

			_Points.RemoveAt( index );
		}

		#endregion

		#region interface IReadOnlyList<TPoint>

		/// <inheritdoc cref="IReadOnlyList{T}" />>
		public int Count => _Points.Count;

		/// <inheritdoc/>>
		public IEnumerator<TPoint> GetEnumerator()
		{
			return _Points.GetEnumerator();
		}

		/// <inheritdoc/>>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc cref="IList{T}" />
		public TPoint this[ int index ]
		{
			get => _Points[ index ];
			set
			{
				if( value == null )
					throw new ArgumentNullException( nameof( value ) );

				_Points[ index ].Segment = null;
				value.Segment = _Segment;

				_Points[ index ] = value;
			}
		}

		#endregion
	}
}