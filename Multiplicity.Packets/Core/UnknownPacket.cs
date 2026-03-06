using System;
using System.IO;

namespace Multiplicity.Packets
{
	public class UnknownPacket : TerrariaPacket
	{
		private byte[] _payload;

		public byte[] Payload
		{
			get
			{
				return _payload ??= PayloadMemory.ToArray();
			}
			set
			{
				_payload = value;
				PayloadMemory = value ?? Array.Empty<byte>();
			}
		}

		public ReadOnlyMemory<byte> PayloadMemory { get; set; }

		public UnknownPacket(BinaryReader br)
			: base(br)
		{
			PayloadMemory = br.ReadMemory(_length - TerrariaPacket.PacketHeaderLength);
		}

        public override short GetLength()
		{
			return (short)(_length - TerrariaPacket.PacketHeaderLength);
		}

		public override void ToStream(Stream stream, bool includeHeader = true)
		{
			base.ToStream(stream, includeHeader);

			if (PayloadMemory.Length > 0)
			{
				stream.Write(PayloadMemory.Span);
			}
		}

		public override string ToString()
		{
			string hex = BitConverter.ToString(Payload).Replace("-", string.Empty);

			return $"[UnknownPacket: Id={Id} Len={GetLength()} Content={hex}]";
		}
	}
}


