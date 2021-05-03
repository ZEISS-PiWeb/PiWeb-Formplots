#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
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

		/// <summary>Constructor.</summary>
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

		/// <inheritdoc />
		internal override void Serialize( XmlWriter writer )
		{
			base.Serialize( writer );

			writer.WriteElementString( "DeviationType", DeviationType.ToString() );
		}


		/// <inheritdoc />
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

		/// <inheritdoc />
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "DeviationType={0}, CoordinateSystem={{{1}}}", DeviationType, CoordinateSystem );
		}

		#endregion
	}
}