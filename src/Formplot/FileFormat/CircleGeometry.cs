#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2013                             */
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
	/// Describes a circle geometry
	/// </summary>
	public sealed class CircleGeometry : Geometry
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public CircleGeometry()
		{
			Radius = 1.0;
		}

		#endregion

		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.Circle;

		/// <summary>
		/// Gets or sets the radius.
		/// </summary>
		public double Radius { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			writer.WriteElementString( "Radius", XmlConvert.ToString( Radius ) );
		}

		/// <inheritdoc />
		protected override bool DeserializeItem( XmlReader reader, Version version )
		{
			if( base.DeserializeItem( reader, version ) )
				return true;

			switch( reader.Name )
			{
				case "Radius":
					Radius = XmlConvert.ToDouble( reader.ReadString() );
					return true;
			}

			return false;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "Radius={0}, CoordinateSystem={{{1}}}", Radius, CoordinateSystem );
		}

		#endregion
	}
}