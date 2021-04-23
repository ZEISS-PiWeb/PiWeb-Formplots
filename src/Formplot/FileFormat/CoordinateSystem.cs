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
	using System.Xml.Serialization;

	#endregion

	/// <summary>
	/// Describes a coordinate system, composed of a position vector and 3 direction vectors.
	/// </summary>
	public sealed class CoordinateSystem
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public CoordinateSystem()
		{
			Origin = new Vector( 0, 0, 0 );
			Axis1 = new Vector( 1, 0, 0 );
			Axis2 = new Vector( 0, 1, 0 );
			Axis3 = new Vector( 0, 0, 1 );
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the position vector.
		/// </summary>
		public Vector Origin { get; set; }

		/// <summary>
		/// Gets or sets the first direction vector.
		/// </summary>
		public Vector Axis1 { get; set; }

		/// <summary>
		/// Gets or sets the second direction vector.
		/// </summary>
		public Vector Axis2 { get; set; }

		/// <summary>
		/// Gets or sets the third direction vector.
		/// </summary>
		public Vector Axis3 { get; set; }

		#endregion

		#region methods

		/// <inheritdoc cref="IXmlSerializable.WriteXml" />
		internal void Serialize( XmlWriter writer )
		{
			if( writer == null )
			{
				throw new ArgumentNullException( nameof( writer ) );
			}

			writer.WriteStartElement( "Origin" );
			Origin.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteStartElement( "Axis1" );
			Axis1.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteStartElement( "Axis2" );
			Axis2.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteStartElement( "Axis3" );
			Axis3.Serialize( writer );
			writer.WriteEndElement();
		}

		/// <inheritdoc cref="IXmlSerializable.ReadXml" />
		public static CoordinateSystem Deserialize( XmlReader reader )
		{
			if( reader == null )
			{
				throw new ArgumentNullException( nameof( reader ) );
			}

			var result = new CoordinateSystem();

			while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
			{
				switch( reader.Name )
				{
					case "Origin":
						result.Origin = Vector.Deserialize( reader );
						break;
					case "Axis1":
						result.Axis1 = Vector.Deserialize( reader );
						break;
					case "Axis2":
						result.Axis2 = Vector.Deserialize( reader );
						break;
					case "Axis3":
						result.Axis3 = Vector.Deserialize( reader );
						break;
				}
			}

			return result;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return string.Format(
				CultureInfo.InvariantCulture,
				"Origin={{{0}}}; Axis1={{{1}}}; Axis2={{{2}}}; Axis3={{{3}}}",
				Origin, Axis1, Axis2, Axis3 );
		}

		#endregion
	}
}