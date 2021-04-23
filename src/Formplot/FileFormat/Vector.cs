#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013-2021                        */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Globalization;
	using System.Xml;

	#endregion

	/// <summary>
	/// Representation of a three dimensional vector.
	/// </summary>
	public readonly struct Vector : IEquatable<Vector>
	{
		#region constructors

		/// <summary>Constructor.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		public Vector( double x = 0.0, double y = 0.0, double z = 0.0 )
		{
			X = x;
			Y = y;
			Z = z;
		}

		#endregion

		#region properties

		/// <summary>
		/// X coordinate.
		/// </summary>
		public double X { get; }

		/// <summary>
		/// Y coordinate.
		/// </summary>
		public double Y { get; }

		/// <summary>
		/// Z coordinate.
		/// </summary>
		public double Z { get; }

		#endregion

		#region methods

		/// <summary>
		/// Serializes this instances information with the specified writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		internal void Serialize( XmlWriter writer )
		{
			if( writer == null )
				throw new ArgumentNullException( nameof(writer) );

			writer.WriteAttributeString( "X", XmlConvert.ToString( X ) );
			writer.WriteAttributeString( "Y", XmlConvert.ToString( Y ) );
			writer.WriteAttributeString( "Z", XmlConvert.ToString( Z ) );
		}

		/// <summary>
		/// Deserializes information from the specified reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">reader</exception>
		internal static Vector Deserialize( XmlReader reader )
		{
			if( reader == null )
				throw new ArgumentNullException( nameof(reader) );

			var value = reader.GetAttribute( "X" );
			var x = string.IsNullOrWhiteSpace( value ) ? default : XmlConvert.ToDouble( value );
			value = reader.GetAttribute( "Y" );
			var y = string.IsNullOrWhiteSpace( value ) ? default : XmlConvert.ToDouble( value );
			value = reader.GetAttribute( "Z" );
			var z = string.IsNullOrWhiteSpace( value ) ? default : XmlConvert.ToDouble( value );

			return new Vector( x, y, z );
		}

		public static Vector operator +( Vector a, Vector b )
		{
			return new Vector( a.X + b.X, a.Y + b.Y, a.Z + b.Z );
		}

		public static Vector operator -( Vector a, Vector b )
		{
			return new Vector( a.X - b.X, a.Y - b.Y, a.Z - b.Z );
		}

		public static bool operator ==( Vector a, Vector b )
		{
			return Equals( a, b );
		}

		public static bool operator !=( Vector a, Vector b )
		{
			return !Equals( a, b );
		}

		/// <summary>
		/// Determines whether the two specified instances are equal.
		/// </summary>
		private static bool Equals( Vector a, Vector b )
		{
			return Math.Abs( a.X - b.X ) < double.Epsilon &&
			       Math.Abs( a.Y - b.Y ) < double.Epsilon &&
			       Math.Abs( a.Z - b.Z ) < double.Epsilon;
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			return obj is Vector vector && Equals( this, vector );
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "{0}, {1}, {2}", X, Y, Z );
		}

		#endregion

		#region interface IEquatable<Vector>

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
		/// </returns>
		public bool Equals( Vector other )
		{
			return Equals( this, other );
		}

		#endregion
	}
}