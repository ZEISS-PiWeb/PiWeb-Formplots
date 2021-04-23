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

		/// <summary>Constructor.</summary>
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

		/// <inheritdoc />
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			writer.WriteElementString( "Length1", XmlConvert.ToString( Length1 ) );
			writer.WriteElementString( "Length2", XmlConvert.ToString( Length2 ) );
		}


		/// <inheritdoc />
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

		/// <inheritdoc />
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "Length1={0}, Length2={1}, CoordinateSystem={{{2}}}", Length1, Length2, CoordinateSystem );
		}

		#endregion
	}
}