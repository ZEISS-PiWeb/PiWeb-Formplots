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
	/// Represents a plane geometry.
	/// </summary>
	public sealed class PlaneGeometry : Geometry
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

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.Plane;

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
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			writer.WriteElementString( "Length1", XmlConvert.ToString( Length1 ) );
			writer.WriteElementString( "Length2", XmlConvert.ToString( Length2 ) );
		}


		/// <summary>
		/// Reads the geometry information from the specified <see cref="XmlReader" />.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="version">The version of the formplot file.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		protected override bool DeserializeItem( XmlReader reader, Version version )
		{
			if( base.DeserializeItem( reader, version ) )
				return true;

			switch( reader.Name )
			{
				case "Length1":
					Length1 = XmlConvert.ToDouble( reader.ReadString() );
					return true;
				case "Length2":
					Length2 = XmlConvert.ToDouble( reader.ReadString() );
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "Length1={0}, Length2={1}, CoordinateSystem={{{2}}}", Length1, Length2, CoordinateSystem );
		}

		#endregion
	}
}