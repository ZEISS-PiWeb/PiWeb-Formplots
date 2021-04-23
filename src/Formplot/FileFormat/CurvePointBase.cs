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

	using System.IO;

	#endregion

	/// <summary>
	/// Represents a curve point.
	/// </summary>
	public abstract class CurvePointBase<TPoint, TGeometry> : Point<TPoint, TGeometry>
		where TPoint : Point<TPoint, TGeometry>, new()
		where TGeometry : Geometry, new()
	{
		#region constructors

		/// <inheritdoc/>
		internal CurvePointBase() { }

		/// <summary>Constructor.</summary>
		/// <param name="position">The position.</param>
		/// <param name="direction">The direction.</param>
		/// <param name="deviation">The deviation.</param>
		internal CurvePointBase( Vector position, Vector direction, double deviation )
		{
			Position = position;
			Direction = direction;
			Deviation = deviation;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		public Vector Position { get; set; }

		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		public Vector Direction { get; set; }

		/// <summary>
		/// Gets or sets the deviation of this point.
		/// </summary>
		public double Deviation { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void WriteToStream( BinaryWriter writer )
		{
			writer.Write( Position.X );
			writer.Write( Position.Y );
			writer.Write( Position.Z );

			writer.Write( Direction.X );
			writer.Write( Direction.Y );
			writer.Write( Direction.Z );

			writer.Write( Deviation );
		}

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader )
		{
			Position = new Vector( reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble() );
			Direction = new Vector( reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble() );
			Deviation = reader.ReadDouble();
		}

		#endregion
	}
}