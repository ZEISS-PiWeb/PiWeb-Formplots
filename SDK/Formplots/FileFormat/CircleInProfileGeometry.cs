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
	/// Stellt eine "Kreis in Kontur" Geometrie dar.
	/// </summary>
	public class CircleInProfileGeometry : Geometry
	{
		#region members

		private CircleInProfilePoint _FirstTouchingPoint = new CircleInProfilePoint();
		private CircleInProfilePoint _SecondTouchingPoint = new CircleInProfilePoint();
		private CircleInProfilePoint _MaxGapPoint = new CircleInProfilePoint();

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
		public CircleInProfilePoint MaxGapPoint
		{
			get { return _MaxGapPoint; }
			set { _MaxGapPoint = value ?? new CircleInProfilePoint(); }
		}

		/// <summary>
		/// Representation of the first point of contact as <see cref="CirclePoint"/>.
		/// </summary>
		public CircleInProfilePoint FirstTouchingPoint
		{
			get { return _FirstTouchingPoint; }
			set { _FirstTouchingPoint = value ?? new CircleInProfilePoint(); }
		}

		/// <summary>
		/// Representation of the second point of contact as <see cref="CirclePoint"/>.
		/// </summary>
		public CircleInProfilePoint SecondTouchingPoint
		{
			get { return _SecondTouchingPoint; }
			set { _SecondTouchingPoint = value ?? new CircleInProfilePoint(); }
		}

		#endregion

		#region methods

		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException">writer</exception>
		public override void Serialize( XmlWriter writer )
		{
			if( writer == null )
			{
				throw new ArgumentNullException( nameof( writer ) );
			}

			writer.WriteStartElement( "CoordinateSystem" );
			CoordinateSystem.Serialize( writer );
			writer.WriteEndElement();

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
		/// <exception cref="System.ArgumentNullException">reader</exception>
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
					case "Radius":
						Radius = XmlConvert.ToDouble( reader.ReadString() );
						break;
					case "MaxGapPoint":
					{
						var angleString = reader.GetAttribute( "Angle" );
						var deviationString = reader.GetAttribute( "Deviation" );

						Tolerance tolerance = null;

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
							Deviation = Property.ObjectToNullableDouble( deviationString, CultureInfo.InvariantCulture ) ?? 0.0,
							Tolerance = tolerance
						};
						break;
					}
					case "FirstTouchingPoint":
					{
						var angleString = reader.GetAttribute( "Angle" );
						var deviationString = reader.GetAttribute( "Deviation" );

						Tolerance tolerance = null;

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
							Deviation = Property.ObjectToNullableDouble( deviationString, CultureInfo.InvariantCulture ) ?? 0.0,
							Tolerance = tolerance
						};
						break;
					}
					case "SecondTouchingPoint":
					{
						var angleString = reader.GetAttribute( "Angle" );
						var deviationString = reader.GetAttribute( "Deviation" );

						Tolerance tolerance = null;

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
							Deviation = Property.ObjectToNullableDouble( deviationString, CultureInfo.InvariantCulture ) ?? 0.0,
							Tolerance = tolerance
						};
						break;
					}
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
			return string.Format( CultureInfo.InvariantCulture, "Radius={0}, CoordinateSystem={{{1}}}", Radius, CoordinateSystem );
		}

		#endregion
	}
}