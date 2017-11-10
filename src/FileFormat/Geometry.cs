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
			get { return _CoordinateSystem; }
			set { _CoordinateSystem = value ?? new CoordinateSystem(); }
		}

		#endregion

		#region methods

		/// <summary>
		/// Creates a geometry of the specified geometry type.
		/// </summary>
		internal static Geometry Create( GeometryTypes geometryType )
		{
			switch( geometryType )
			{
				case GeometryTypes.Circle:
					return new CircleGeometry();
				case GeometryTypes.Plane:
					return new PlaneGeometry();
				case GeometryTypes.Curve:
					return new CurveGeometry();
				case GeometryTypes.Line:
					return new LineGeometry();
				case GeometryTypes.Cylinder:
					return new CylinderGeometry();
				case GeometryTypes.CircleInProfile:
					return new CircleInProfileGeometry();

				default:
					return null;
			}
		}

		/// <summary>
		/// Returns the geometry type value for a specified formplot type.
		/// </summary>
		public static GeometryTypes GetGeometryTypeFromFormplotType( FormplotTypes formplotType )
		{
			switch( formplotType )
			{
				case FormplotTypes.None:
				case FormplotTypes.Fourier: return GeometryTypes.None;
				case FormplotTypes.Roundness: return GeometryTypes.Circle;
				case FormplotTypes.Flatness: return GeometryTypes.Plane;
				case FormplotTypes.CurveProfile: return GeometryTypes.Curve;
				case FormplotTypes.Straightness: return GeometryTypes.Line;
				case FormplotTypes.Cylindricity: return GeometryTypes.Cylinder;
				case FormplotTypes.Pitch: return GeometryTypes.None;
				case FormplotTypes.BorePattern: return GeometryTypes.Curve;
				case FormplotTypes.CircleInProfile: return GeometryTypes.CircleInProfile;
				default:
					throw new ArgumentOutOfRangeException( nameof( formplotType ), formplotType, null );
			}
		}

		/// <summary>
		/// Writes the geometry information to the specified <see cref="XmlWriter"/>.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public abstract void Serialize( XmlWriter writer );

		/// <summary>
		/// Reads the geometry information from the specified <see cref="XmlReader"/>.
		/// </summary>
		/// <param name="reader">The reader.</param>
		protected abstract void Deserialize( XmlReader reader );

		/// <summary>
		/// Reads the geometry information from the specified <see cref="XmlReader"/>.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="geometryType">Type of the geometry.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		internal static Geometry DeserializeGeometry( XmlReader reader, GeometryTypes geometryType )
		{
			if( reader == null )
			{
				throw new ArgumentNullException( nameof( reader ) );
			}

			var geometry = Create( geometryType );
			geometry.Deserialize( reader );
			return geometry;
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