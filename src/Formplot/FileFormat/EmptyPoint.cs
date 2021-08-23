#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	using System;
	using System.IO;

	/// <summary>
	/// Point that is used as a dummy for the <see cref="EmptyPlot"/>.
	/// </summary>
	public sealed class EmptyPoint : Point<EmptyPoint, EmptyGeometry>
	{
		#region methods

		/// <inheritdoc />
		internal override void WriteToStream( BinaryWriter writer ) { }

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader, Version version ) { }

		#endregion
	}
}