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
	/// Represents a plane geometry.
	/// </summary>
	public class PlaneGeometry : Geometry
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PlaneGeometry"/> class.
		/// </summary>
		public PlaneGeometry()
		{
			Length1 = 1.0;
			Length2 = 1.0;
		}

		#endregion

		#region properties
		
		/// <summary>
		/// Length of the first axis of the plane geometry
		/// </summary>
		public double Length1 { get; set; }


		/// <summary>
		/// Length of the second axis of the plane geometry.
		/// </summary>
		public double Length2 { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public override void Serialize( XmlWriter writer )
		{
			if( writer == null )
			{
				throw new ArgumentNullException( nameof( writer ) );
			}

			writer.WriteStartElement( "CoordinateSystem" );
			CoordinateSystem.Serialize( writer );
			writer.WriteEndElement();

			writer.WriteElementString( "Length1", XmlConvert.ToString( Length1 ) );
			writer.WriteElementString( "Length2", XmlConvert.ToString( Length2 ) );
		}


		/// <summary>
		/// Reads the geometry information from the specified <see cref="XmlReader" />.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		protected override void Deserialize( XmlReader reader )
		{
			if( reader == null )
			{
				throw new ArgumentNullException( nameof( reader ) );
			}

			while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
			{
				switch( reader.Name )
				{
					case "CoordinateSystem":
						CoordinateSystem = CoordinateSystem.Deserialize( reader );
						break;
					case "Length1":
						Length1 = XmlConvert.ToDouble( reader.ReadString() );
						break;
					case "Length2":
						Length2 = XmlConvert.ToDouble( reader.ReadString() );
						break;
				}
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "Length1={0}, Length2={1}, CoordinateSystem={{{2}}}", Length1, Length2, CoordinateSystem );
		}

		#endregion
	}
}