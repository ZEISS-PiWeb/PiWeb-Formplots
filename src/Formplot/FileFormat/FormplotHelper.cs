#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.FileFormat
{
	#region usings

	using System;
	using System.Buffers;
	using System.IO;
	using System.IO.Compression;

	#endregion

	internal class FormplotHelper
	{
		#region methods

		public static Version GetFileFormatVersion( FormplotTypes formplotType )
		{
			return formplotType switch
			{
				FormplotTypes.None => new Version( 1, 0 ),
				_                  => new Version( 2, 0 )
			};
		}

		public static void VerifyValidRange( int count, Range range, Property property )
		{
			if( !IsValidRange( count, range ) )
				throw new ArgumentOutOfRangeException( $"Invalid form plot file. Invalid range '{range}' for property '{property}'" );
		}

		public static void VerifyValidRange( int count, Range range, PointState state )
		{
			if( !IsValidRange( count, range ) )
				throw new ArgumentOutOfRangeException( $"Invalid form plot file. Invalid range '{range}' for point state '{state}'" );
		}

		public static void VerifyValidRange( int count, Range range, Segment segment )
		{
			if( !IsValidRange( count, range ) )
				throw new ArgumentOutOfRangeException( $"Invalid form plot file. Invalid range '{range}' for segment '{segment}'" );
		}

		public static void VerifyValidRange( int count, Range range, Tolerance tolerance )
		{
			if( !IsValidRange( count, range ) )
				throw new ArgumentOutOfRangeException( $"Invalid form plot file. Invalid range '{range}' for tolerance '{tolerance}'" );
		}

		private static bool IsValidRange( int count, Range range )
		{
			return range.Start >= 0 && range.Start < count && range.End >= 0 && range.End < count;
		}

		public static Stream ReadAndSanitizeHeaderEntry( ZipArchiveEntry headerEntry )
		{
			using var stream = headerEntry.Open();

			// Ok, so here is the problem: Some applications writes broken XML files which end on ascii zero bytes.
			// The XML loader used in GetFormplotType() does not like this. We need to cut of trailing zeros. However,
			// we cannot do this in little endian UTF-16 encoded files (the last zero would be part of the '>'
			// character). So we compare the first few bytes of the XML header to what Calypso would write to make sure
			// it is not UTF-16.
			var streamLength = (int)headerEntry.Length;
			var content = ArrayPool<byte>.Shared.Rent( streamLength );
			stream.Read( content, 0, streamLength );

			if( !IsTruncationSafe( content ) )
				return new ArrayPoolStream( content, streamLength );

			var endOfContent = FindEndOfContent( content ) + 1;
			return new ArrayPoolStream( content, endOfContent );
		}

		private static bool IsTruncationSafe( byte[] content )
		{
			// check if first few bytes correspond to ascii "<?xml"
			return ( content[ 0 ] == 0x3C )
					&& ( content[ 1 ] == 0x3F )
					&& ( content[ 2 ] == 0x78 )
					&& ( content[ 3 ] == 0x6D )
					&& ( content[ 4 ] == 0x6C );
		}

		private static int FindEndOfContent( byte[] content )
		{
			var i = content.Length - 1;
			while( i > -1 && content[ i ] == 0 )
				--i;

			return i;
		}

		#endregion

		#region class ArrayPoolStream

		private class ArrayPoolStream : Stream
		{
			#region members

			private readonly byte[] _Buffer;
			private readonly Stream _Stream;

			#endregion

			#region constructors

			/// <summary>Constructor.</summary>
			public ArrayPoolStream( byte[] buffer, int length )
			{
				_Buffer = buffer;
				_Stream = new MemoryStream( buffer, 0, length );
			}

			#endregion

			#region properties

			/// <inheritdoc />
			public override bool CanRead => _Stream.CanRead;

			/// <inheritdoc />
			public override bool CanSeek => _Stream.CanSeek;

			/// <inheritdoc />
			public override bool CanWrite => _Stream.CanWrite;

			/// <inheritdoc />
			public override long Length => _Stream.Length;

			/// <inheritdoc />
			public override long Position
			{
				get => _Stream.Position;
				set => _Stream.Position = value;
			}

			#endregion

			#region methods

			/// <inheritdoc />
			protected override void Dispose( bool disposing )
			{
				_Stream.Dispose();
				ArrayPool<byte>.Shared.Return( _Buffer );
			}

			/// <inheritdoc />
			public override void Flush()
			{
				_Stream.Flush();
			}

			/// <inheritdoc />
			public override int Read( byte[] buffer, int offset, int count )
			{
				return _Stream.Read( buffer, offset, count );
			}

			/// <inheritdoc />
			public override long Seek( long offset, SeekOrigin origin )
			{
				return _Stream.Seek( offset, origin );
			}

			/// <inheritdoc />
			public override void SetLength( long value )
			{
				_Stream.SetLength( value );
			}

			/// <inheritdoc />
			public override void Write( byte[] buffer, int offset, int count )
			{
				_Stream.Write( buffer, offset, count );
			}

			#endregion
		}

		#endregion
	}
}