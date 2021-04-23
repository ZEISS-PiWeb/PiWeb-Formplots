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

	using System.IO;

	#endregion

	/// <summary>
	/// Represents a point of a line
	/// </summary>
	public sealed class LinePoint : Point<LinePoint,LineGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public LinePoint( ) { }

		/// <summary>Constructor.</summary>
		/// <param name="position">The position.</param>
		/// <param name="deviation">The deviation.</param>
		public LinePoint( double position, double deviation )
		{
			Position = position;
			Deviation = deviation;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public double Position { get; set; }

		/// <summary>
		/// Gets or sets the deviation.
		/// </summary>
		public double Deviation { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void WriteToStream( BinaryWriter writer )
		{
			writer.Write( Position );
			writer.Write( Deviation );
		}

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader )
		{
			Position = reader.ReadDouble();
			Deviation = reader.ReadDouble();
		}

		#endregion
	}
}