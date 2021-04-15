#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Globalization;
	using System.Xml;
	using Zeiss.PiWeb.Formplot.Common;

	#endregion

	/// <summary>
	/// Geometry for a <see cref="PitchPlot"/>
	/// </summary>
	public class PitchGeometry : Geometry
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PitchGeometry"/> class.
		/// </summary>
		public PitchGeometry()
		{
			DeviationType = DeviationTypes.None;
		}

		#endregion

		#region properties

		/// <inheritdoc />
		public override GeometryTypes GeometryType { get; } = GeometryTypes.Pitch;

		/// <summary>
		/// Length of the first axis of the plane geometry
		/// </summary>
		public DeviationTypes DeviationType { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter" />.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			writer.WriteElementString( "DeviationType", DeviationType.ToString() );
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
				case "DeviationType":
					DeviationType = EnumParser<DeviationTypes>.Parse( reader.ReadString() );
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "DeviationType={0}, CoordinateSystem={{{1}}}", DeviationType, CoordinateSystem );
		}

		#endregion
	}
}