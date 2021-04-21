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
	using System.Xml;

	#endregion

	/// <summary>
	/// Representation of a cylinder geometry.
	/// </summary>
	public sealed class CylinderGeometry : Geometry
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CylinderGeometry"/> class.
		/// </summary>
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

		#region overrides

		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException">writer</exception>
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			writer.WriteElementString( "Radius", XmlConvert.ToString( Radius ) );
			writer.WriteElementString( "Height", XmlConvert.ToString( Height ) );
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
				case "Height":
					Height = XmlConvert.ToDouble( reader.ReadString() );
					return true;
			}

			return false;
		}

		#endregion
	}
}
