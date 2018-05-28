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
	using System.Diagnostics;
	using System.Globalization;
	using System.Text;
	using System.Xml;
	using Zeiss.IMT.PiWeb.Formplot.Common;

	#endregion

	/// <summary>
	/// Class to encapsulate metadata with a name, a datatype and a value
	/// </summary>
	public class Property
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Property"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="datatype">The datatype.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <exception cref="System.ArgumentException">name must not be null or empty</exception>
		private Property( string name, DataTypeId datatype, object value, string description )
		{
			if( string.IsNullOrWhiteSpace( name ) )
				throw new ArgumentException( "name must not be null or empty", nameof( name ) );


			Name = name;
			DataType = datatype;
			Value = value;
			Description = description;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the culture invariant name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the datatype.
		/// </summary>
		private DataTypeId DataType { get; }

		/// <summary>
		/// Gets the value.
		/// </summary>
		public object Value { get; }

		/// <summary>
		/// Gets a culture invariant description.
		/// </summary>
		public string Description { get; }

		#endregion

		#region methods

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="long"/> value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		public static Property Create( string name, long value, string description = null )
		{
			return new Property( name, DataTypeId.Integer, value, description );
		}

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="double"/> value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		public static Property Create( string name, double value, string description = null )
		{
			return new Property( name, DataTypeId.Double, value, description );
		}

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="string"/> value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		public static Property Create( string name, string value, string description = null )
		{
			return new Property( name, DataTypeId.String, value, description );
		}

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="DateTime"/> value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		public static Property Create( string name, DateTime value, string description = null )
		{
			return new Property( name, DataTypeId.DateTime, value, description );
		}

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="TimeSpan"/> value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		public static Property Create( string name, TimeSpan value, string description = null )
		{
			return new Property( name, DataTypeId.TimeSpan, value, description );
		}

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="string"/> value and tries to detect the real datatype.
		/// <para><remarks>Order: <see cref="DateTime"/> -> <see cref="Int64"/> -> <see cref="Double"/> -> <see cref="TimeSpan"/></remarks></para>
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		public static Property TryToDetectTypeAndCreate( string name, string value, string description )
		{
			if( string.IsNullOrWhiteSpace( value ) ) return new Property( name, DataTypeId.String, value, description );

			var dateTimeValue = ObjectToNullableDateTime( value, CultureInfo.InvariantCulture ) ?? ObjectToNullableDateTime( value );

			if( dateTimeValue.HasValue )
			{
				if( dateTimeValue.Value.Kind == DateTimeKind.Unspecified )
					dateTimeValue = DateTime.SpecifyKind( dateTimeValue.Value, DateTimeKind.Local );

				if( dateTimeValue.Value > new DateTime( 1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc ) && dateTimeValue < DateTime.UtcNow + TimeSpan.FromDays( 100 * 365 ) )
					return Create( name, dateTimeValue.Value, description );
			}

			var longValue = ObjectToNullableInt64( value, CultureInfo.InvariantCulture ) ?? ObjectToNullableInt64( value );

			if( longValue.HasValue )
			{
				return Create( name, longValue.Value, description );
			}

			var doubleValue = ObjectToNullableDouble( value, CultureInfo.InvariantCulture ) ?? ObjectToNullableDouble( value );

			if( doubleValue.HasValue )
			{
				return Create( name, doubleValue.Value, description );
			}

			var timeSpanValue = ObjectToNullableTimeSpan( value, CultureInfo.InvariantCulture ) ?? ObjectToNullableTimeSpan( value );

			return timeSpanValue.HasValue ? Create( name, timeSpanValue.Value, description ) : new Property( name, DataTypeId.String, value, description );
		}

		/// <summary>
		/// Returns the properties value as string.
		/// </summary>
		public string GetStringValue()
		{
			switch( DataType )
			{
				case DataTypeId.Integer:
					return XmlConvert.ToString( ( long ) Value );
				case DataTypeId.Double:
					return XmlConvert.ToString( ( double ) Value );
				case DataTypeId.String:
					return ( string ) Value;
				case DataTypeId.DateTime:
					return XmlConvert.ToString( ( DateTime ) Value, XmlDateTimeSerializationMode.RoundtripKind );
				case DataTypeId.TimeSpan:
					return XmlConvert.ToString( ( TimeSpan ) Value );
				default:
					throw new NotSupportedException( $"type \"{DataType}\" is not supported" );
			}
		}

		/// <summary>
		/// Serializes the property.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException">writer</exception>
		internal void Serialize( XmlWriter writer )
		{
			if( writer == null )
				throw new ArgumentNullException( nameof( writer ) );

			var value = GetStringValue();

			writer.WriteAttributeString( "Name", Name );
			writer.WriteAttributeString( "Type", DataType.ToString() );

			if( !string.IsNullOrWhiteSpace( Description ) )
			{
				writer.WriteAttributeString( "Description", Description );
			}

			writer.WriteValue( value );
		}

		/// <summary>
		/// Deserializes the property data.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">reader</exception>
		/// <exception cref="System.NotSupportedException"></exception>
		internal static Property Deserialize( XmlReader reader )
		{
			if( reader == null )
				throw new ArgumentNullException( nameof( reader ) );

			var name = reader.GetAttribute( "Name" );
			var dataType = EnumParser<DataTypeId>.Parse( reader.GetAttribute( "Type" ) );
			var description = reader.GetAttribute( "Description" );
			var stringValue = reader.ReadString();

			switch( dataType )
			{
				case DataTypeId.DateTime:
					return Create( name, XmlConvert.ToDateTime( stringValue, XmlDateTimeSerializationMode.RoundtripKind ), description );
				case DataTypeId.Double:
					return Create( name, XmlConvert.ToDouble( stringValue ), description );
				case DataTypeId.Integer:
					return Create( name, XmlConvert.ToInt64( stringValue ), description );
				case DataTypeId.String:
					return Create( name, stringValue, description );
				case DataTypeId.TimeSpan:
					return Create( name, XmlConvert.ToTimeSpan( stringValue ), description );
				default:
					throw new NotSupportedException( $"DataTypeId \"{dataType}\" is not supported" );
			}
		}

		/// <summary>
		/// Determines, whether the two specified instances are equal.
		/// </summary>
		private static bool Equals( Property m1, Property m2 )
		{
			if( ReferenceEquals( m1, m2 ) )
			{
				return true;
			}

			if( m1 != null && m2 != null )
			{
				return m1.Name == m2.Name &&
				       m1.DataType == m2.DataType &&
				       Equals( m1.Value, m2.Value );
			}
			return false;
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ DataType.GetHashCode() ^ ( Value?.GetHashCode() ?? 0 );
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
			return Equals( this, obj as Property );
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append( Name );
			if( !string.IsNullOrEmpty( Description ) )
				sb.Append( " [" ).Append( Description ).Append( "] " );

			return sb.ToString();
		}

		private static DateTime? ObjectToNullableDateTime( string stringValue, IFormatProvider provider = null, DateTimeStyles style = DateTimeStyles.RoundtripKind )
		{
			return DateTime.TryParse( stringValue, provider ?? CultureInfo.CurrentCulture, style, out var result ) ? ( DateTime? ) result : null;
		}

		private static long? ObjectToNullableInt64( string stringValue, IFormatProvider provider = null, NumberStyles style = NumberStyles.Integer )
		{
			return long.TryParse( stringValue, style, provider, out var result ) ? ( long? ) result : null;
		}

		internal static double? ObjectToNullableDouble( string stringValue, IFormatProvider provider = null, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands )
		{
			return double.TryParse( stringValue, style, provider, out var result ) ? ( double? ) result : null;
		}

		private static TimeSpan? ObjectToNullableTimeSpan( string stringValue, IFormatProvider provider = null )
		{
			if( TimeSpan.TryParse( stringValue, provider, out var result ) ) return result;
			if( TryXmlConvertToTimeSpan( stringValue, out result ) ) return result;
			return null;
		}

		[DebuggerStepThrough]
		private static bool TryXmlConvertToTimeSpan( string value, out TimeSpan result )
		{
			try
			{
				result = XmlConvert.ToTimeSpan( value );
				return true;
			}
			catch( Exception )
			{
				result = default ( TimeSpan );
				return false;
			}
		}

		#endregion
	}
}