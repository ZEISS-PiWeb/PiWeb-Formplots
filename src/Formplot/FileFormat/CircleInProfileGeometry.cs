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
	/// Represents a "circle in contour" geometry.
	/// </summary>
	public sealed class CircleInProfileGeometry : Geometry
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public CircleInProfileGeometry()
		{
			Radius = 1.0;
		}

		#endregion

		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.CircleInProfile;

		/// <summary>
		/// Gets or sets the radius.
		/// </summary>
		public double Radius { get; set; }

		/// <summary>
		/// The point with the largest gap.
		/// </summary>
		public CircleInProfilePoint MaxGapPoint { get; set; } = new CircleInProfilePoint();

		/// <summary>
		/// Representation of the first point of contact as <see cref="CirclePoint"/>.
		/// </summary>
		public CircleInProfilePoint FirstTouchingPoint { get; set; } = new CircleInProfilePoint();

		/// <summary>
		/// Representation of the second point of contact as <see cref="CirclePoint"/>.
		/// </summary>
		public CircleInProfilePoint SecondTouchingPoint { get; set; } = new CircleInProfilePoint();

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			writer.WriteElementString( "Radius", XmlConvert.ToString( Radius ) );

			writer.WriteStartElement( "MaxGapPoint" );
			writer.WriteAttributeString( "Angle", XmlConvert.ToString( MaxGapPoint.Angle ) );
			writer.WriteAttributeString( "Deviation", XmlConvert.ToString( MaxGapPoint.Deviation ) );

			if( !MaxGapPoint.Tolerance.IsEmpty )
			{
				writer.WriteStartElement( "Tolerance" );
				MaxGapPoint.Tolerance.Serialize( writer );
				writer.WriteEndElement();
			}

			writer.WriteEndElement();

			writer.WriteStartElement( "FirstTouchingPoint" );
			writer.WriteAttributeString( "Angle", XmlConvert.ToString( FirstTouchingPoint.Angle ) );
			writer.WriteAttributeString( "Deviation", XmlConvert.ToString( FirstTouchingPoint.Deviation ) );

			if( !FirstTouchingPoint.Tolerance.IsEmpty )
			{
				writer.WriteStartElement( "Tolerance" );
				FirstTouchingPoint.Tolerance.Serialize( writer );
				writer.WriteEndElement();
			}

			writer.WriteEndElement();

			writer.WriteStartElement( "SecondTouchingPoint" );
			writer.WriteAttributeString( "Angle", XmlConvert.ToString( SecondTouchingPoint.Angle ) );
			writer.WriteAttributeString( "Deviation", XmlConvert.ToString( SecondTouchingPoint.Deviation ) );

			if( !SecondTouchingPoint.Tolerance.IsEmpty )
			{
				writer.WriteStartElement( "Tolerance" );
				SecondTouchingPoint.Tolerance.Serialize( writer );
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
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
				case "MaxGapPoint":
					return DeserializeMaxGapPoint( reader );
				case "FirstTouchingPoint":
					return DeserializeFirstTouchingPoint( reader );
				case "SecondTouchingPoint":
					return DeserializeSecondTouchingPoint( reader );
			}

			return false;
		}

		private bool DeserializeMaxGapPoint( XmlReader reader )
		{
			var angleString = reader.GetAttribute( "Angle" );
			var deviationString = reader.GetAttribute( "Deviation" );

			Tolerance? tolerance = null;

			if( !reader.IsEmptyElement )
			{
				while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
				{
					if( reader.Name.Equals( "Tolerance" ) )
					{
						tolerance = Tolerance.Deserialize( reader );
					}
				}
			}

			MaxGapPoint = new CircleInProfilePoint
			{
				Angle = Property.ObjectToNullableDouble( angleString, CultureInfo.InvariantCulture ) ?? 0.0,
				Deviation = Property.ObjectToNullableDouble( deviationString, CultureInfo.InvariantCulture ) ?? 0.0
			};

			if( tolerance != null )
				MaxGapPoint.Tolerance = tolerance;

			return true;
		}

		private bool DeserializeFirstTouchingPoint( XmlReader reader )
		{
			var angleString = reader.GetAttribute( "Angle" );
			var deviationString = reader.GetAttribute( "Deviation" );

			Tolerance? tolerance = null;

			if( !reader.IsEmptyElement )
			{
				while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
				{
					if( reader.Name.Equals( "Tolerance" ) )
					{
						tolerance = Tolerance.Deserialize( reader );
					}
				}
			}

			FirstTouchingPoint = new CircleInProfilePoint
			{
				Angle = Property.ObjectToNullableDouble( angleString, CultureInfo.InvariantCulture ) ?? 0.0,
				Deviation = Property.ObjectToNullableDouble( deviationString, CultureInfo.InvariantCulture ) ?? 0.0
			};

			if( tolerance != null )
				FirstTouchingPoint.Tolerance = tolerance;

			return true;
		}

		private bool DeserializeSecondTouchingPoint( XmlReader reader )
		{
			var angleString = reader.GetAttribute( "Angle" );
			var deviationString = reader.GetAttribute( "Deviation" );

			Tolerance? tolerance = null;

			if( !reader.IsEmptyElement )
			{
				while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
				{
					if( reader.Name.Equals( "Tolerance" ) )
					{
						tolerance = Tolerance.Deserialize( reader );
					}
				}
			}

			SecondTouchingPoint = new CircleInProfilePoint
			{
				Angle = Property.ObjectToNullableDouble( angleString, CultureInfo.InvariantCulture ) ?? 0.0,
				Deviation = Property.ObjectToNullableDouble( deviationString, CultureInfo.InvariantCulture ) ?? 0.0
			};

			if( tolerance != null )
				SecondTouchingPoint.Tolerance = tolerance;

			return true;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "Radius={0}, CoordinateSystem={{{1}}}", Radius, CoordinateSystem );
		}

		#endregion
	}
}