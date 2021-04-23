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
	using System.Diagnostics;
	using System.Globalization;
	using System.Text;
	using System.Xml;
	using Zeiss.PiWeb.Formplot.Common;

	#endregion

	/// <summary>
	/// Class to encapsulate metadata with a name, a datatype and a value
	/// </summary>
	public sealed class Property
	{
		#region constructors

		/// <summary>Constructor.</summary>
		/// <param name="name">The name.</param>
		/// <param name="datatype">The datatype.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <param name="unit">The unit.</param>
		/// <exception cref="System.ArgumentException">name must not be null or empty</exception>
		private Property( string name, DataTypeId datatype, object value, string? description, string? unit )
		{
			if( string.IsNullOrWhiteSpace( name ) )
				throw new ArgumentException( "name must not be null or empty", nameof( name ) );


			Name = name;
			DataType = datatype;
			Value = value;
			Description = description;
			Unit = unit;
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
		public object? Value { get; }

		/// <summary>
		/// Gets a culture invariant description.
		/// </summary>
		public string? Description { get; }

		/// <summary>
		/// Gets the unit of the specified value.
		/// </summary>
		public string? Unit { get; }

		#endregion

		#region methods

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="long"/> value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <param name="unit">The unit.</param>
		/// <returns></returns>
		public static Property Create( string name, long value, string? description = null, string? unit = null )
		{
			return new Property( name, DataTypeId.Integer, value, description, unit );
		}

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="double"/> value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <param name="unit">The unit.</param>
		/// <returns></returns>
		public static Property Create( string name, double value, string? description = null, string? unit = null )
		{
			return new Property( name, DataTypeId.Double, value, description, unit );
		}

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="string"/> value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		public static Property Create( string name, string value, string? description = null )
		{
			return new Property( name, DataTypeId.String, value, description, null );
		}

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="DateTime"/> value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		public static Property Create( string name, DateTime value, string? description = null )
		{
			return new Property( name, DataTypeId.DateTime, value, description, null );
		}

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="TimeSpan"/> value.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
		public static Property Create( string name, TimeSpan value, string? description = null )
		{
			return new Property( name, DataTypeId.TimeSpan, value, description, null );
		}

		/// <summary>
		/// Creates a <see cref="Property"/> instance from a <see cref="string"/> value and tries to detect the real datatype.
		/// <para><remarks>Order: <see cref="DateTime"/> -> <see cref="Int64"/> -> <see cref="Double"/> -> <see cref="TimeSpan"/></remarks></para>
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		/// <param name="description">The description.</param>
		/// <param name="unit">The unit.</param>
		/// <returns></returns>
		public static Property TryToDetectTypeAndCreate( string name, string value, string? description, string? unit = null )
		{
			if( string.IsNullOrWhiteSpace( value ) ) return new Property( name, DataTypeId.String, value, description, null );

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
				return Create( name, longValue.Value, description, unit );
			}

			var doubleValue = ObjectToNullableDouble( value, CultureInfo.InvariantCulture ) ?? ObjectToNullableDouble( value );

			if( doubleValue.HasValue )
			{
				return Create( name, doubleValue.Value, description, unit );
			}

			var timeSpanValue = ObjectToNullableTimeSpan( value, CultureInfo.InvariantCulture ) ?? ObjectToNullableTimeSpan( value );

			return timeSpanValue.HasValue ? Create( name, timeSpanValue.Value, description ) : new Property( name, DataTypeId.String, value, description, null );
		}

		/// <summary>
		/// Returns the properties value as string.
		/// </summary>
		public string? GetStringValue()
		{
			switch( DataType )
			{
				case DataTypeId.Integer:
					return XmlConvert.ToString( (long)( Value ?? 0 ) );
				case DataTypeId.Double:
					return XmlConvert.ToString( (double)( Value ?? 0.0 ) );
				case DataTypeId.String:
					return (string?)Value;
				case DataTypeId.DateTime:
					return XmlConvert.ToString( (DateTime)( Value ?? new DateTime() ), XmlDateTimeSerializationMode.RoundtripKind );
				case DataTypeId.TimeSpan:
					return XmlConvert.ToString( (TimeSpan)( Value ?? new TimeSpan() ) );
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

			if( !string.IsNullOrWhiteSpace( Unit ) )
			{
				writer.WriteAttributeString( "Unit", Unit );
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
			var dataType = EnumParser<DataTypeId>.TryParse( reader.GetAttribute( "Type" ), out var parsedDataType )
				? parsedDataType
				: DataTypeId.String; // Fall back to DataType = String for very old form plot data that might not have a data type identifier
			var description = reader.GetAttribute( "Description" );
			var unit = reader.GetAttribute( "Unit" );
			var stringValue = reader.ReadString();

			switch( dataType )
			{
				case DataTypeId.DateTime:
					return Create( name, XmlConvert.ToDateTime( stringValue, XmlDateTimeSerializationMode.RoundtripKind ), description );
				case DataTypeId.Double:
					return Create( name, XmlConvert.ToDouble( stringValue ), description, unit );
				case DataTypeId.Integer:
					return Create( name, XmlConvert.ToInt64( stringValue ), description, unit );
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
		private static bool Equals( Property? m1, Property? m2 )
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

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ DataType.GetHashCode() ^ ( Value?.GetHashCode() ?? 0 );
		}

		/// <inheritdoc />
		public override bool Equals( object? obj )
		{
			return Equals( this, obj as Property );
		}

		/// <inheritdoc />
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append( Name );
			if( !string.IsNullOrEmpty( Description ) )
				sb.Append( " [" ).Append( Description ).Append( "] " );

			return sb.ToString();
		}

		private static DateTime? ObjectToNullableDateTime( string stringValue, IFormatProvider? provider = null, DateTimeStyles style = DateTimeStyles.RoundtripKind )
		{
			return DateTime.TryParse( stringValue, provider ?? CultureInfo.CurrentCulture, style, out var result ) ? (DateTime?)result : null;
		}

		private static long? ObjectToNullableInt64( string stringValue, IFormatProvider? provider = null, NumberStyles style = NumberStyles.Integer )
		{
			return long.TryParse( stringValue, style, provider, out var result ) ? (long?)result : null;
		}

		internal static double? ObjectToNullableDouble( string stringValue, IFormatProvider? provider = null, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands )
		{
			return double.TryParse( stringValue, style, provider, out var result ) ? (double?)result : null;
		}

		private static TimeSpan? ObjectToNullableTimeSpan( string stringValue, IFormatProvider? provider = null )
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
				result = default;
				return false;
			}
		}

		#endregion
	}
}