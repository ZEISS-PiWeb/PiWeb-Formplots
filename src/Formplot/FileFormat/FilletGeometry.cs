#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019-2021                        */
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
	/// Contains the Fillet entry- and exit points as well as the gap points of two Fillets.
	/// </summary>
	public sealed class FilletGeometry : CurveGeometry
	{
		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.Fillet;

		/// <summary>
		/// Gets or sets the inlet point.
		/// </summary>
		public FilletCircle? Circle { get; set; }

		/// <summary>
		/// Gets or sets the inlet point.
		/// </summary>
		public FilletPoint? Inlet { get; set; }

		/// <summary>
		/// Gets or sets the outlet point.
		/// </summary>
		public FilletPoint? Outlet { get; set; }

		#endregion

		#region methods

		private static void SerializePoint( XmlWriter writer, FilletPoint point, string name )
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

		private static FilletPoint DeserializePoint( XmlReader reader )
		{
			var deviation = Property.ObjectToNullableDouble( reader.GetAttribute( "Deviation" ), CultureInfo.InvariantCulture ) ?? 0.0;
			var tolerance = default( Tolerance );
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

			var point = new FilletPoint
			{
				Position = position,
				Deviation = deviation,
				Direction = normal
			};

			if( tolerance != null )
				point.Tolerance = tolerance;

			return point;
		}

		private static void SerializeCircle( XmlWriter writer, FilletCircle circle, string name )
		{
			writer.WriteStartElement( name );

			writer.WriteAttributeString( "Radius", XmlConvert.ToString( circle.Radius ) );

			if( circle.Center != null )
				SerializePoint( writer, circle.Center, "Center" );

			writer.WriteEndElement();
		}

		private static FilletCircle DeserializeCircle( XmlReader reader )
		{
			var radius = 0.0;
			var center = new FilletPoint();

			while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
			{
				switch( reader.Name )
				{
					case "Radius":
						radius = Property.ObjectToNullableDouble( reader.ReadString(), CultureInfo.InvariantCulture ) ?? 0.0;
						break;
					case "Center":
						center = DeserializePoint( reader );
						break;
				}
			}

			return new FilletCircle
			{
				Radius = radius,
				Center = center
			};
		}

		/// <inheritdoc />
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			if( Circle != null )
				SerializeCircle( writer, Circle, "Circle" );

			if( Inlet != null )
				SerializePoint( writer, Inlet, "Inlet" );

			if( Outlet != null )
				SerializePoint( writer, Outlet, "Outlet" );
		}

		/// <inheritdoc />
		protected override bool DeserializeItem( XmlReader reader, Version version )
		{
			if( base.DeserializeItem( reader, version ) )
				return true;

			switch( reader.Name )
			{
				case "Inlet":
					Inlet = DeserializePoint( reader );
					return true;
				case "Outlet":
					Outlet = DeserializePoint( reader );
					return true;
				case "Circle":
					Circle = DeserializeCircle( reader );
					return true;
			}

			return false;
		}

		#endregion
	}
}