using System;
using System.IO;

namespace Multiplicity.Packets
{
    public abstract class RawPayloadPacket : TerrariaPacket
    {
        private byte[] _payload;
        private ReadOnlyMemory<byte> _payloadMemory;

        public byte[] Payload
        {
            get
            {
                return _payload ??= _payloadMemory.ToArray();
            }
            set
            {
                _payload = value;
                _payloadMemory = value ?? Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> PayloadMemory
        {
            get => _payloadMemory;
            set
            {
                _payloadMemory = value;
                _payload = null;
            }
        }

        protected RawPayloadPacket(byte id)
            : base(id)
        {
            _payloadMemory = ReadOnlyMemory<byte>.Empty;
            _payload = Array.Empty<byte>();
        }

        protected RawPayloadPacket(BinaryReader br)
            : base(br)
        {
            PayloadMemory = br.ReadMemory(_length - PacketHeaderLength);
        }

        public override short GetLength()
        {
            return (short)_payloadMemory.Length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            if (_payloadMemory.Length > 0)
            {
                stream.Write(_payloadMemory.Span);
            }
        }

        public override string ToString()
        {
            return $"[{GetType().Name}: PacketType={PacketType} Length={GetLength()}]";
        }
    }
}
