#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Globalization;
	using System.Xml;

	#endregion

	/// <summary>
	/// Representation of a three dimensional vector.
	/// </summary>
	public class Vector : IEquatable<Vector>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector"/> class.
		/// </summary>
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
		public double X { get; set; }

		/// <summary>
		/// Y coordinate
		/// </summary>
		public double Y { get; set; }

		/// <summary>
		/// Z coordinate.
		/// </summary>
		public double Z { get; set; }

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
				throw new ArgumentNullException( nameof( writer ) );

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
				throw new ArgumentNullException( nameof( reader ) );

			var value = reader.GetAttribute( "X" );
			var x = string.IsNullOrWhiteSpace( value ) ? default ( double ) : XmlConvert.ToDouble( value );
			value = reader.GetAttribute( "Y" );
			var y = string.IsNullOrWhiteSpace( value ) ? default ( double ) : XmlConvert.ToDouble( value );
			value = reader.GetAttribute( "Z" );
			var z = string.IsNullOrWhiteSpace( value ) ? default ( double ) : XmlConvert.ToDouble( value );

			return new Vector { X = x, Y = y, Z = z };
		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator ==( Vector a, Vector b )
		{
			return Equals( a, b );
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="a">a.</param>
		/// <param name="b">The b.</param>
		/// <returns>
		/// The result of the operator.
		/// </returns>
		public static bool operator !=( Vector a, Vector b )
		{
			return !Equals( a, b );
		}

		/// <summary>
		/// Determines whether the two specified instances are equal.
		/// </summary>
		private static bool Equals( Vector a, Vector b )
		{
			if( ReferenceEquals( a, b ) )
			{
				return true;
			}
			else if( a != null && b != null )
			{
				return Math.Abs( a.X - b.X ) < double.Epsilon &&
				       Math.Abs( a.Y - b.Y ) < double.Epsilon &&
				       Math.Abs( a.Z - b.Z ) < double.Epsilon;
			}

			return false;
		}


		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals( object obj )
		{
			return Equals( this, obj as Vector );
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
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