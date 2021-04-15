#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	using System.IO;

	/// <summary>
	/// Point that is used as a dummy for the <see cref="EmptyPlot"/>.
	/// </summary>
	public sealed class EmptyPoint : Point<EmptyPoint,EmptyGeometry>
	{
		internal override void WriteToStream( BinaryWriter writer ){ }

		internal override void ReadFromStream( BinaryReader reader ){ }
	}
}



