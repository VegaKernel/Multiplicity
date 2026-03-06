using System;
using System.IO;

namespace Multiplicity.Packets
{
    public abstract class RawPayloadPacket : TerrariaPacket
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

        protected RawPayloadPacket(byte id)
            : base(id)
        {
            PayloadMemory = ReadOnlyMemory<byte>.Empty;
            _payload = Array.Empty<byte>();
        }

        protected RawPayloadPacket(BinaryReader br)
            : base(br)
        {
            PayloadMemory = br.ReadMemory(_length - PACKET_HEADER_LEN);
        }

        public override short GetLength()
        {
            return (short)PayloadMemory.Length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            if (PayloadMemory.Length > 0)
            {
                stream.Write(PayloadMemory.Span);
            }
        }

        public override string ToString()
        {
            return $"[{GetType().Name}: PacketType={PacketType} Length={GetLength()}]";
        }
    }
}
