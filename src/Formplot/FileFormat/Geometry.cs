#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
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
	/// Base class for geometry descriptions.
	/// </summary>
	public abstract class Geometry
	{
		#region members

		private CoordinateSystem _CoordinateSystem = new CoordinateSystem();

		#endregion

		#region properties

		/// <summary>
		/// The <see cref="CoordinateSystem"/> of the geometry.
		/// </summary>
		public CoordinateSystem CoordinateSystem
		{
			get => _CoordinateSystem;
			set => _CoordinateSystem = value ?? new CoordinateSystem();
		}

		/// <summary>
		/// Gets the type of this geometry
		/// </summary>
		public abstract GeometryTypes GeometryType { get; }

		#endregion

		#region methods

		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter"/>.
		/// </summary>
		/// <param name="writer">The writer.</param>
		internal virtual void Serialize( XmlWriter writer )
		{
			if( writer == null )
			{
				throw new ArgumentNullException( nameof( writer ) );
			}

			writer.WriteStartElement( "CoordinateSystem" );
			CoordinateSystem.Serialize( writer );
			writer.WriteEndElement();
		}

		/// <summary>
		/// Reads the information from the current item of the specified <see cref="XmlReader"/>.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="version">The version of the formplot file.</param>
		protected virtual bool DeserializeItem( XmlReader reader, Version version )
		{
			switch( reader.Name )
			{
				case "ElementSystem":
				case "CoordinateSystem":
					CoordinateSystem = CoordinateSystem.Deserialize( reader );
					return true;
			}

			return false;
		}

		/// <summary>
		/// Reads the geometry information from the specified <see cref="XmlReader"/>.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="version">The version of the formplot file.</param>
		internal void Deserialize( XmlReader reader, Version version )
		{
			if( reader == null )
			{
				throw new ArgumentNullException( nameof( reader ) );
			}

			while( reader.Read() && reader.NodeType != XmlNodeType.EndElement )
			{
				DeserializeItem( reader, version );
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
			return string.Format( CultureInfo.InvariantCulture, "CoordinateSystem={{{0}}}", CoordinateSystem );
		}

		#endregion
	}
}