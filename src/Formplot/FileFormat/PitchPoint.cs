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
	/// Represents a point of a pitch.
	/// </summary>
	public sealed class PitchPoint : Point<PitchPoint,PitchGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public PitchPoint(  ) { }

		/// <summary>Constructor.</summary>
		/// <param name="deviation">The deviation.</param>
		public PitchPoint( double deviation )
		{
			Deviation = deviation;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <remarks>
		/// The position is not stored in the formplot file. Instead, the array index is used.
		/// </remarks>
		public int Position { get; set; }

		/// <value>
		/// The deviation.
		/// </value>
		public double Deviation { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void WriteToStream( BinaryWriter writer )
		{
			writer.Write( Deviation );
		}

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader )
		{
			Position = Index;
			Deviation = reader.ReadDouble();
		}

		#endregion
	}
}