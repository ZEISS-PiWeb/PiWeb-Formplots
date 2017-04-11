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
	/// Describes a coordinate system, composed of a position vector and 3 direction vectors.
	/// </summary>
	public class CoordinateSystem
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CoordinateSystem"/> class.
		/// </summary>
		public CoordinateSystem()
		{
			Origin = new Vector { X = 0, Y = 0, Z = 0 };
			Axis1 = new Vector { X = 1, Y = 0, Z = 0 };
			Axis2 = new Vector { X = 0, Y = 1, Z = 0 };
			Axis3 = new Vector { X = 0, Y = 0, Z = 1 };
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

		/// <summary>
		/// Writes the coordinate system information in the specified <see cref="XmlWriter"/>.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException">writer</exception>
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

		/// <summary>
		/// Reads the coordinate system information from the specified <see cref="XmlReader"/>.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">reader</exception>
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

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "Orgin={{{0}}}; Axis1={{{1}}}; Axis2={{{2}}}; Axis3={{{3}}}", Origin, Axis1, Axis2, Axis3 );
		}

		#endregion
	}
}