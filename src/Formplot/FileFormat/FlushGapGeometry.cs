#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
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
	/// Contains the curve entry- and exit points as well as the gap points of two curves.
	/// </summary>
	public sealed class FlushGapGeometry : CurveGeometry
	{
		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.FlushGap;

		/// <summary>
		/// Get or set the reference profile for geometry of Flush- and Gapplot
		/// </summary>
		public FlushGapProfile ReferenceProfile { get; private set; } = new FlushGapProfile();

		/// <summary>
		/// Get or set the measure profile  for geometry of Flush- and Gapplot
		/// </summary>
		public FlushGapProfile MeasureProfile { get; private set; } = new FlushGapProfile();

		/// <summary>
		/// Get or set the value for the Flush of Flush- and Gapplot
		/// </summary>
		public double FlushValue { get; set; }

		/// <summary>
		/// Get or set the type of connection between the Flush points of reference and measure profile
		/// </summary>
		public FlushPointConnectionType FlushConnectionType { get; set; } = FlushPointConnectionType.Orthogonal;

		/// <summary>
		/// Get or set the value for the Gap of Flush- and Gapplot
		/// </summary>
		public double GapValue { get; set; }

		/// <summary>
		/// Get or set the type of connection between the Gap points of reference and measure profile
		/// </summary>
		public FlushPointConnectionType GapConnectionType { get; set; } = FlushPointConnectionType.Orthogonal;

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			if( ReferenceProfile != null )
			{
				writer.WriteStartElement( "ReferenceProfile" );
				ReferenceProfile.Serialize( writer );
				writer.WriteEndElement();
			}

			if( MeasureProfile != null )
			{
				writer.WriteStartElement( "MeasureProfile" );
				MeasureProfile.Serialize( writer );
				writer.WriteEndElement();
			}

			writer.WriteStartElement( "Flush" );
			writer.WriteAttributeString( "ConnectionType", FlushConnectionType.ToString() );
			writer.WriteString( XmlConvert.ToString( FlushValue ) );
			writer.WriteEndElement();

			writer.WriteStartElement( "Gap" );
			writer.WriteAttributeString( "ConnectionType", GapConnectionType.ToString() );
			writer.WriteString( XmlConvert.ToString( GapValue ) );
			writer.WriteEndElement();
		}

		/// <inheritdoc />
		protected override bool DeserializeItem( XmlReader reader, Version version )
		{
			if( base.DeserializeItem( reader, version ) )
				return true;

			switch( reader.Name )
			{
				case "ReferenceProfile":
					ReferenceProfile = FlushGapProfile.Deserialize( reader );
					return true;
				case "MeasureProfile":
					MeasureProfile = FlushGapProfile.Deserialize( reader );
					return true;
				case "Flush":
				{
					if( Enum.TryParse<FlushPointConnectionType>( reader.GetAttribute( "ConnectionType" ), out var connectionType ) )
						FlushConnectionType = connectionType;

					FlushValue = Property.ObjectToNullableDouble( reader.ReadString(), CultureInfo.InvariantCulture ) ?? 0.0;

					return true;
				}
				case "Gap":
				{
					if( Enum.TryParse<FlushPointConnectionType>( reader.GetAttribute( "ConnectionType" ), out var connectionType ) )
						GapConnectionType = connectionType;

					GapValue = Property.ObjectToNullableDouble( reader.ReadString(), CultureInfo.InvariantCulture ) ?? 0.0;

					return true;
				}
			}

			return false;
		}

		#endregion
	}
}