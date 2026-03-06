using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiplicity.Packets
{
	public abstract class TerrariaNetworkObject
	{
		/// <summary>
		/// Gets the object length in bytes.
		/// </summary>
		public abstract short GetLength();

		/// <summary>
		/// Gets or sets the CRC32 hash for this TerrariaNetworkObject.
		/// </summary>
		/// <value>The CRC value.</value>
		public uint Crc { get; internal set; }

		/// <summary>
		/// Serializes this TerrariaNetworkObject instance into the provided stream.
		/// </summary>
		/// <param name="stream">
		/// A reference to a valid, open, and writable stream object in which to serialize this
		/// instance to.
		/// </param>
		public abstract void ToStream(Stream stream, bool includeHeader = true);

		/// <summary>
		/// Serializes only the payload portion of this object into the provided stream.
		/// </summary>
		/// <param name="stream">
		/// A reference to a valid, open, and writable stream object in which to serialize the payload.
		/// </param>
		public virtual void WritePayload(Stream stream)
		{
			ToStream(stream, includeHeader: false);
		}

		/// <summary>
		/// Returns a byte array with the binary contents of this TerrariaPacket instance.
		/// </summary>
		public virtual byte[] ToArray(bool includeHeader = true)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				ToStream(ms, includeHeader);
				return ms.ToArray();
			}
		}

		/// <summary>
		/// Returns a byte array containing only the payload bytes of this object.
		/// </summary>
		public virtual byte[] ToPayloadArray()
		{
			return ToArray(includeHeader: false);
		}

		/// <summary>
		/// Copies the payload bytes into the destination span.
		/// Throws if the destination is smaller than the payload length.
		/// </summary>
		public virtual void CopyPayloadTo(Span<byte> destination)
		{
			if (!TryCopyPayloadTo(destination, out int bytesWritten))
			{
				throw new ArgumentException(
					$"Destination span length {destination.Length} is smaller than payload length {GetLength()}.",
					nameof(destination));
			}

			if (bytesWritten != GetLength())
			{
				throw new InvalidOperationException(
					$"Payload serialization produced {bytesWritten} bytes, expected {GetLength()}.");
			}
		}

		/// <summary>
		/// Attempts to copy the payload bytes into the destination span.
		/// </summary>
		public virtual bool TryCopyPayloadTo(Span<byte> destination, out int bytesWritten)
		{
			byte[] payload = ToPayloadArray();
			bytesWritten = payload.Length;

			if (destination.Length < payload.Length)
			{
				return false;
			}

			payload.CopyTo(destination);
			return true;
		}
	}
}

