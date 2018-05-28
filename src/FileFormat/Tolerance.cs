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
	using Zeiss.IMT.PiWeb.Formplot.Common;

	#endregion

	/// <summary>
	/// Represents a tolerance.
	/// </summary>
	public class Tolerance : IEquatable<Tolerance>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Tolerance"/> class.
		/// </summary>
		/// <param name="lower">The lower tolerance.</param>
		/// <param name="upper">The upper tolerance.</param>
		public Tolerance( double? lower = null, double? upper = null )
		{
			if( lower.HasValue &&
			    upper.HasValue &&
			    upper.Value < lower.Value )
			{
				Upper = lower;
				Lower = upper;
			}
			else
			{
				Lower = lower;
				Upper = upper;
			}
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the lower.
		/// </summary>
		/// <value>
		/// The lower.
		/// </value>
		public double? Lower { get; set; }


		/// <summary>
		/// Gets or sets the upper.
		/// </summary>
		/// <value>
		/// The upper.
		/// </value>
		public double? Upper { get; set; }


		/// <summary>
		/// Gets or sets the type of the tolerance.
		/// </summary>
		/// <value>
		/// The type of the tolerance.
		/// </value>
		public ToleranceType ToleranceType { get; set; }


		/// <summary>
		/// Gets or sets the radius of a circular tolerance.
		/// </summary>
		public double? CircularToleranceRadius { get; set; }


		/// <summary>
		/// Gets or sets the width of a rectangular tolerance.
		/// </summary>
		public double? RectangleToleranceWidth { get; set; }


		/// <summary>
		/// Gets or sets the height of a rectangular tolerance.
		/// </summary>
		public double? RectangleToleranceHeight { get; set; }

		/// <summary>
		/// Gets or sets the spatial tolerance.
		/// </summary>
		public Vector SpatialTolerance { get; set; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="Tolerance"/> instance has a appropriate values or not.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				switch( ToleranceType )
				{
					case ToleranceType.Circular:
						return !CircularToleranceRadius.HasValue;
					case ToleranceType.Rectangular:
						return !RectangleToleranceHeight.HasValue && !RectangleToleranceWidth.HasValue;
					case ToleranceType.Spatial:
						return SpatialTolerance != null;
					default:
						return !Lower.HasValue && !Upper.HasValue;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Tolerance"/> instance is symmetric.
		/// </summary>
		private bool IsSymmetric => ToleranceType == ToleranceType.Default && Lower.HasValue && Upper.HasValue && Math.Abs( Lower.Value + Upper.Value ) < double.Epsilon;

		#endregion

		#region methods

		/// <summary>
		/// Serializes this instance
		/// </summary>
		internal void Serialize( XmlWriter writer )
		{
			switch( ToleranceType )
			{
				case ToleranceType.Circular when CircularToleranceRadius.HasValue:
					writer.WriteAttributeString( "Type", ToleranceType.ToString() );
					writer.WriteAttributeString( "Radius", XmlConvert.ToString( CircularToleranceRadius.Value ) );
					break;
				case ToleranceType.Rectangular:
					writer.WriteAttributeString( "Type", ToleranceType.ToString() );

					if( RectangleToleranceHeight.HasValue )
						writer.WriteAttributeString( "Height", XmlConvert.ToString( RectangleToleranceHeight.Value ) );
					if( RectangleToleranceWidth.HasValue )
						writer.WriteAttributeString( "Width", XmlConvert.ToString( RectangleToleranceWidth.Value ) );
					break;
				case ToleranceType.Spatial:
					writer.WriteAttributeString( "Type", ToleranceType.ToString() );

					if( SpatialTolerance != null )
					{
						writer.WriteAttributeString( "X", XmlConvert.ToString( SpatialTolerance.X ) );
						writer.WriteAttributeString( "Y", XmlConvert.ToString( SpatialTolerance.Y ) );
						writer.WriteAttributeString( "Z", XmlConvert.ToString( SpatialTolerance.Z ) );
					}

					break;
				case ToleranceType.Default:
					if( IsSymmetric )
					{
						if( Upper != null && Lower != null ) writer.WriteValue( XmlConvert.ToString( Upper.Value - Lower.Value ) );
					}
					else
					{
						if( Lower.HasValue )
							writer.WriteElementString( "Lower", XmlConvert.ToString( Lower.Value ) );

						if( Upper.HasValue )
							writer.WriteElementString( "Upper", XmlConvert.ToString( Upper.Value ) );
					}

					break;
			}
		}

		/// <summary>
		/// Deserializes the information from the specified <paramref name="reader"/>.
		/// </summary>
		internal static Tolerance Deserialize( XmlReader reader )
		{
			var toleranceTypeString = reader.GetAttribute( "Type" );
			var toleranceType = ToleranceType.Default;
			double? radius = null;
			double? height = null;
			double? width = null;
			double? lower = null;
			double? upper = null;
			Vector spatialTolerance = null;

			if ( !string.IsNullOrEmpty( toleranceTypeString ) )
			{
				toleranceType = EnumParser<ToleranceType>.Parse( toleranceTypeString );

				switch( toleranceType )
				{
					case ToleranceType.Circular:
						var radiusText = reader.GetAttribute( "Radius" );
						if( !string.IsNullOrEmpty( radiusText ) )
							radius = XmlConvert.ToDouble( radiusText );
						break;
					case ToleranceType.Rectangular:
						var rectangleHeightText = reader.GetAttribute( "Height" );
						var rectangleWidthText = reader.GetAttribute( "Width" );

						if( !string.IsNullOrEmpty( rectangleHeightText ) )
							height = XmlConvert.ToDouble( rectangleHeightText );
						if( !string.IsNullOrEmpty( rectangleWidthText ) )
							width = XmlConvert.ToDouble( rectangleWidthText );
						break;
					case ToleranceType.Spatial:
						var xText = reader.GetAttribute( "X" );
						var yText = reader.GetAttribute( "Y" );
						var zText = reader.GetAttribute( "Z" );

						double? x = null;
						double? y = null;
						double? z = null;

						if( !string.IsNullOrEmpty( xText ) )
							x = XmlConvert.ToDouble( xText );
						if( !string.IsNullOrEmpty( yText ) )
							y = XmlConvert.ToDouble( yText );
						if( !string.IsNullOrEmpty( zText ) )
							z = XmlConvert.ToDouble( zText );

						if( x.HasValue && y.HasValue && z.HasValue )
							spatialTolerance = new Vector( x.Value, y.Value, z.Value );
						break;
				}
			}
			else
			{
				while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
				{
					if( reader.NodeType == XmlNodeType.Text )
					{
						var toleranceValue = Math.Abs( XmlConvert.ToDouble( reader.ReadString() ) ) / 2.0;

						lower = -toleranceValue;
						upper = toleranceValue;

						break;
					}
					else
					{
						switch( reader.Name )
						{
							case "Lower":
								lower = XmlConvert.ToDouble( reader.ReadString() );
								break;
							case "Upper":
								upper = XmlConvert.ToDouble( reader.ReadString() );
								break;
						}
					}
				}
			}

			return new Tolerance
			{
				Lower = lower,
				Upper = upper,
				ToleranceType = toleranceType,
				CircularToleranceRadius = radius,
				RectangleToleranceHeight = height,
				RectangleToleranceWidth = width,
				SpatialTolerance = spatialTolerance
			};
		}

		/// <summary>
		/// Bestimmt, ob die angegebenen Instanzen gleich sind.
		/// </summary>
		public static bool Equals( Tolerance t1, Tolerance t2 )
		{
			if( ReferenceEquals( t1, t2 ) )
				return true;

			if( t1 != null && t2 != null )
			{
				return Equals( t1.Lower, t2.Lower ) &&
				       Equals( t1.Upper, t2.Upper ) &&
				       Equals( t1.ToleranceType, t2.ToleranceType ) &&
				       Equals( t1.CircularToleranceRadius, t2.CircularToleranceRadius ) &&
				       Equals( t1.RectangleToleranceHeight, t2.RectangleToleranceHeight ) &&
				       Equals( t1.RectangleToleranceWidth, t2.RectangleToleranceWidth );
			}

			return false;
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return Lower.GetHashCode() ^ Upper.GetHashCode();
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals( object obj )
		{
			return Equals( this, obj as Tolerance );
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if( IsEmpty )
				return "Empty";

			if( IsSymmetric )
				return string.Format( CultureInfo.InvariantCulture, "±{0}", Upper );

			if( ToleranceType == ToleranceType.Default )
				return string.Format( CultureInfo.InvariantCulture, "{0}, {1}", Lower, Upper );

			if( ToleranceType == ToleranceType.Circular )
				return string.Format( CultureInfo.InvariantCulture, "Cir {0}", CircularToleranceRadius );

			if( ToleranceType == ToleranceType.Rectangular )
				return string.Format( CultureInfo.InvariantCulture, "Rect {0}, {1}", RectangleToleranceWidth, RectangleToleranceHeight );

			if (ToleranceType == ToleranceType.Spatial)
				return string.Format(CultureInfo.InvariantCulture, "3D {0}, {1}, {2}", SpatialTolerance?.X, SpatialTolerance?.Y, SpatialTolerance?.Z);

			return "";
		}

		#endregion

		#region interface IEquatable<Tolerance>

		/// <summary>
		/// Determines, whether this instance is equal to the <paramref name="other"/> instance.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns></returns>
		public bool Equals( Tolerance other )
		{
			return Equals( this, other );
		}

		#endregion
	}
}