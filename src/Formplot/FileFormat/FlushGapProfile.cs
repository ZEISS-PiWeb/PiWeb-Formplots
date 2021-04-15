#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
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
	/// Holds Information about the profiles in Flush- Gapplot
	/// </summary>
	public sealed class FlushGapProfile
	{
		#region members

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the inlet point.
		/// </summary>
		public FlushGapPoint? Inlet { get; set; }

		/// <summary>
		/// Gets or sets the outlet point.
		/// </summary>
		public FlushGapPoint? Outlet { get; set; }

		/// <summary>
		/// Gets or sets the flush point.
		/// </summary>
		public FlushGapPoint? Flush { get; set; }

		/// <summary>
		/// Gets or sets the gap point.
		/// </summary>
		public FlushGapPoint? Gap { get; set; }

		private static void SerializePoint( XmlWriter writer, FlushGapPoint point, string name )
		{
			writer.WriteStartElement( name );

			writer.WriteAttributeString( "Deviation", XmlConvert.ToString( point.Deviation ) );

			writer.WriteStartElement( "Position" );
			point.Position.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteStartElement( "Normal" );
			point.Direction.Serialize( writer );
			writer.WriteEndElement();

			if( !point.Tolerance.IsEmpty )
			{
				writer.WriteStartElement( "Tolerance" );
				point.Tolerance.Serialize( writer );
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		private static FlushGapPoint DeserializePoint( XmlReader reader )
		{
			var deviation = Property.ObjectToNullableDouble( reader.GetAttribute( "Deviation" ), CultureInfo.InvariantCulture ) ?? 0.0;
			var tolerance = new Tolerance();
			var position = new Vector();
			var normal = new Vector();

			while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
			{
				switch( reader.Name )
				{
					case "Tolerance":
						tolerance = Tolerance.Deserialize( reader );
						break;
					case "Position":
						position = Vector.Deserialize( reader );
						break;
					case "Normal":
						normal = Vector.Deserialize( reader );
						break;
				}
			}

			return new FlushGapPoint
			{
				Position = position,
				Deviation = deviation,
				Direction = normal,
				Tolerance = tolerance
			};
		}

		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException">writer</exception>
		public void Serialize( XmlWriter writer )
		{
			if( writer == null )
			{
				throw new ArgumentNullException( nameof( writer ) );
			}

			if( Inlet != null )
				SerializePoint( writer, Inlet, "Inlet" );

			if( Outlet != null )
				SerializePoint( writer, Outlet, "Outlet" );

			if( Flush != null )
				SerializePoint( writer, Flush, "Flush" );

			if( Gap != null )
				SerializePoint( writer, Gap, "Gap" );
		}


		/// <summary>
		/// Reads the geometry information to the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns>FlushGapProfile</returns>
		public static FlushGapProfile Deserialize( XmlReader reader )
		{
			var result = new FlushGapProfile();

			if( reader.IsEmptyElement )
				return result;

			while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
			{
				switch( reader.Name )
				{
					case "Inlet":
						result.Inlet = DeserializePoint( reader );
						break;
					case "Outlet":
						result.Outlet = DeserializePoint( reader );
						break;
					case "Edge":
						DeserializePoint( reader ); //Not used anymore
						break;
					case "Flush":
						result.Flush = DeserializePoint( reader );
						break;
					case "Gap":
						result.Gap = DeserializePoint( reader );
						break;
				}
			}

			return result;
		}

		#endregion
	}
}