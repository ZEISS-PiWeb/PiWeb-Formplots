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
	using System.IO;

	#endregion

	/// <summary>
	/// Represents a CurveDistance point.
	/// </summary>
	public sealed class CurveDistancePoint : Point<CurveDistancePoint, CurveDistanceGeometry>
	{
		#region constructors

		/// <inheritdoc />
		public CurveDistancePoint() { }


		/// <summary>Constructor.</summary>
		/// <param name="firstPosition">The nominal position of the first point.</param>
		/// <param name="firstDirection">The direction of the first point.</param>
		/// <param name="firstDeviation">The deviation of the first point.</param>
		/// <param name="secondPosition">The nominal position of the second point.</param>
		/// <param name="secondDirection">The direction of the second point.</param>
		/// <param name="secondDeviation">The deviation position of the second point.</param>
		/// <param name="distance">The calculated distance between the actual points.</param>
		public CurveDistancePoint(
			Vector firstPosition,
			Vector firstDirection,
			double firstDeviation,
			Vector secondPosition,
			Vector secondDirection,
			double secondDeviation,
			double distance )
		{
			FirstPosition = firstPosition;
			FirstDirection = firstDirection;
			FirstDeviation = firstDeviation;
			SecondPosition = secondPosition;
			SecondDirection = secondDirection;
			SecondDeviation = secondDeviation;
			Distance = distance;
		}

		#endregion

		#region properties

		/// <summary>
		/// The nominal position of the first point.
		/// </summary>
		public Vector FirstPosition { get; set; }

		/// <summary>
		/// The direction of the first point.
		/// </summary>
		public Vector FirstDirection { get; set; }

		/// <summary>
		/// The deviation of the first point.
		/// </summary>
		public double FirstDeviation { get; set; }

		/// <summary>
		/// The nominal position of the second point.
		/// </summary>
		public Vector SecondPosition { get; set; }

		/// <summary>
		/// The direction of the second point.
		/// </summary>
		public Vector SecondDirection { get; set; }

		/// <summary>
		/// The deviation position of the second point.
		/// </summary>
		public double SecondDeviation { get; set; }

		/// <summary>
		/// The calculated distance between the actual points.
		/// </summary>
		public double Distance { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void WriteToStream( BinaryWriter writer )
		{
			writer.Write( FirstPosition.X );
			writer.Write( FirstPosition.Y );
			writer.Write( FirstPosition.Z );
			writer.Write( FirstDirection.X );
			writer.Write( FirstDirection.Y );
			writer.Write( FirstDirection.Z );
			writer.Write( FirstDeviation );

			writer.Write( SecondPosition.X );
			writer.Write( SecondPosition.Y );
			writer.Write( SecondPosition.Z );
			writer.Write( SecondDirection.X );
			writer.Write( SecondDirection.Y );
			writer.Write( SecondDirection.Z );
			writer.Write( SecondDeviation );

			writer.Write( Distance );
		}

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader, Version version )
		{
			FirstPosition = new Vector( reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble() );
			FirstDirection = new Vector( reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble() );
			FirstDeviation = reader.ReadDouble();

			SecondPosition = new Vector( reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble() );
			SecondDirection = new Vector( reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble() );
			SecondDeviation = reader.ReadDouble();

			Distance = reader.ReadDouble();
		}

		#endregion
	}
}