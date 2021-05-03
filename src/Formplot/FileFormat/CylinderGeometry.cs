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
	using System.Xml;

	#endregion

	/// <summary>
	/// Represents a cylinder geometry.
	/// </summary>
	public sealed class CylinderGeometry : Geometry
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public CylinderGeometry()
		{
			Radius = 1.0;
			Height = 1.0;
		}

		#endregion

		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.Cylinder;

		/// <summary>
		/// Gets or sets the radius.
		/// </summary>
		public double Radius { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public double Height { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			writer.WriteElementString( "Radius", XmlConvert.ToString( Radius ) );
			writer.WriteElementString( "Height", XmlConvert.ToString( Height ) );
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
				case "Height":
					Height = XmlConvert.ToDouble( reader.ReadString() );
					return true;
			}

			return false;
		}

		#endregion
	}
}