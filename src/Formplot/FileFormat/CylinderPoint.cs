﻿#region copyright

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
	/// Represents a cylinder point.
	/// </summary>
	public sealed class CylinderPoint : Point<CylinderPoint, CylinderGeometry>
	{
		#region constructors

		/// <inheritdoc/>
		public CylinderPoint() { }

		/// <summary>Constructor.</summary>
		/// <param name="angle">The angle.</param>
		/// <param name="height">The height.</param>
		/// <param name="deviation">The deviation.</param>
		public CylinderPoint( double angle, double height, double deviation )
		{
			Angle = angle;
			Height = height;
			Deviation = deviation;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the angle in radians.
		/// </summary>
		public double Angle { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public double Height { get; set; }

		/// <summary>
		/// Gets or sets the deviation of this point.
		/// </summary>
		public double Deviation { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		internal override void WriteToStream( BinaryWriter writer )
		{
			writer.Write( Angle );
			writer.Write( Height );
			writer.Write( Deviation );
		}

		/// <inheritdoc />
		internal override void ReadFromStream( BinaryReader reader, Version version )
		{
			Angle = reader.ReadDouble();
			Height = reader.ReadDouble();
			Deviation = reader.ReadDouble();
		}

		#endregion
	}
}