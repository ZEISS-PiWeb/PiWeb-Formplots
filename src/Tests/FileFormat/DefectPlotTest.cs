#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.Tests.FileFormat
{
	#region usings

	using System;
	using System.IO;
	using NUnit.Framework;
	using Zeiss.PiWeb.Formplot.FileFormat;

	#endregion

	[TestFixture]
	public class DefectPlotTests
	{
		#region members

		private static readonly string TestDataDirectory = Path.Combine( TestContext.CurrentContext.TestDirectory, "TestData" );

		#endregion

		#region methods

		[Test]
		public void Test_Deserialize_LegacyFormat()
		{
			var file = Path.Combine( TestDataDirectory, "pltx", "DefectPlot_2_0.pltx" );
			using var stream = File.OpenRead( file );
			var deserialized = Formplot.ReadFrom<DefectPlot>( stream );

			Assert.That( deserialized, Is.Not.Null );
			Assert.That( deserialized.Segments, Has.Count.EqualTo( 1 ) );
			Assert.That( deserialized.Segments[ 0 ].Points, Has.Count.EqualTo( 1 ) );
			Assert.That( deserialized.Segments[ 0 ].Points[ 0 ].Position, Is.EqualTo( new Vector( 0 ) ) );
			Assert.That( deserialized.Segments[ 0 ].Points[ 0 ].Size, Is.EqualTo( new Vector( 1, 1, 1 ) ) );
			Assert.That( deserialized.Segments[ 0 ].Points[ 0 ].Voxels, Is.Not.Null );
			Assert.That( deserialized.Segments[ 0 ].Points[ 0 ].Voxels, Has.Length.EqualTo( 1 ) );
			Assert.That( deserialized.Segments[ 0 ].Points[ 0 ].Voxels[ 0 ].Position, Is.EqualTo( new Vector( 0 ) ) );
			Assert.That( deserialized.Segments[ 0 ].Points[ 0 ].Voxels[ 0 ].Size, Is.EqualTo( new Vector( 1, 1, 1 ) ) );
		}

		[Test]
		public void Test_Voxels_OutOfBounds_Throws_FormatException()
		{
			var writer = new BinaryWriter( new MemoryStream() );

			var defect = new Defect( new Vector( 1, 1, 1 ), new Vector( 2, 2, 2 ), new[]
			{
				new Voxel( new Vector( 1.5, 1.5, 1.5 ), new Vector( 2, 2, 2 ) )
			} );

			Assert.That( () => defect.WriteToStream( writer ), Throws.InstanceOf<FormatException>() );
		}

		[Test]
		public void Test_Voxels_WithEmptyArray_Throws_Nothing()
		{
			var writer = new BinaryWriter( new MemoryStream() );
			var defect = new Defect( new Vector( 1, 1, 1 ), new Vector( 2, 2, 2 ), Array.Empty<Voxel>() );

			Assert.That( () => defect.WriteToStream( writer ), Throws.Nothing );
		}

		[Test]
		public void Test_Mesh_Throws_FormatException_WithInvalidNumberOfIndices()
		{
			var writer = new BinaryWriter( new MemoryStream() );

			var defect = new Defect( new Vector( 0 ), new Vector( 1, 1, 1 ), new Mesh(
				new[] { 0, 1 },
				new[] { 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f }
			) );

			Assert.That( () => defect.WriteToStream( writer ), Throws.InstanceOf<FormatException>() );
		}

		[Test]
		public void Test_Mesh_Throws_FormatException_WithInvalidIndices()
		{
			var writer = new BinaryWriter( new MemoryStream() );

			var defect = new Defect( new Vector( 0 ), new Vector( 1, 1, 1 ), new Mesh(
				new[] { 0, 0, 1 },
				new[] { 0.0f, 0.0f, 0.0f }
			) );

			Assert.That( () => defect.WriteToStream( writer ), Throws.InstanceOf<FormatException>() );
		}

		[Test]
		public void Test_Mesh_Throws_FormatException_WithInvalidNumberOfVertices()
		{
			var writer = new BinaryWriter( new MemoryStream() );

			var defect = new Defect( new Vector( 0 ), new Vector( 1, 1, 1 ), new Mesh(
				new[] { 0, 0, 0 },
				new[] { 0.0f, 0.0f, 0.0f, 0.0f }
			) );

			Assert.That( () => defect.WriteToStream( writer ), Throws.InstanceOf<FormatException>() );
		}

		[Test]
		public void Test_Mesh_Throws_FormatException_WithVerticesOutOfBounds()
		{
			var writer = new BinaryWriter( new MemoryStream() );

			var defect = new Defect( new Vector( 0 ), new Vector( 1, 1, 1 ), new Mesh(
				new[] { 0, 0, 0 },
				new[] { 0.0f, 0.0f, 2.0f }
			) );

			Assert.That( () => defect.WriteToStream( writer ), Throws.InstanceOf<FormatException>() );
		}

		[Test]
		public void Test_Mesh_Serialization_HappyPath()
		{
			var stream = new MemoryStream();
			var writer = new BinaryWriter( stream );
			var reader = new BinaryReader( stream );
			var defect = new Defect( new Vector( 1, 1, 1 ), new Vector( 3, 3, 3 ), new Mesh(
				new[] { 0, 1, 2 },
				new[] { 1.0f, 1.0f, 1.0f, 3.0f, 3.0f, 3.0f, 1.0f, 2.0f, 3.0f }
			) );

			Assert.That( defect.Voxels, Is.Null );
			Assert.That( defect.Shape, Is.Not.Null );

			var clone = new Defect();

			Assert.That( () => defect.WriteToStream( writer ), Throws.Nothing );
			stream.Seek( 0, SeekOrigin.Begin );
			Assert.That( () => clone.ReadFromStream( reader, new Version( 3, 0 ) ), Throws.Nothing );

			Assert.That( clone.Position, Is.EqualTo( defect.Position ) );
			Assert.That( clone.Size, Is.EqualTo( defect.Size ) );
			Assert.That( clone.Voxels, Is.Null );
			Assert.That( clone.Shape, Is.Not.Null );

			Assert.That( clone.Shape.Value.Indices, Is.EquivalentTo( defect.Shape.Value.Indices) );
			Assert.That( clone.Shape.Value.Vertices, Is.EquivalentTo( defect.Shape.Value.Vertices) );
		}

		#endregion
	}
}