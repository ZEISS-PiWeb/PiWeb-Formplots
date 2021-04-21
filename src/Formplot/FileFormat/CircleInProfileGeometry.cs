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
	/// Stellt eine "Kreis in Kontur" Geometrie dar.
	/// </summary>
	public sealed class CircleInProfileGeometry : Geometry
	{
		#region members

		#endregion

		#region constructors

		/// <summary>
		/// Konstruktor
		/// </summary>
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
		/// <value>
		/// The radius.
		/// </value>
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

		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException">writer</exception>
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
				case "Radius":
						Radius = XmlConvert.ToDouble( reader.ReadString() );
						return true;
					case "MaxGapPoint":
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
					case "FirstTouchingPoint":
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
					case "SecondTouchingPoint":
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
			}

			return false;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "Radius={0}, CoordinateSystem={{{1}}}", Radius, CoordinateSystem );
		}

		#endregion
	}
}