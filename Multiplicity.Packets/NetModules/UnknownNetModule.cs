using System;
using System.IO;

namespace Multiplicity.Packets
{
    public class UnknownNetModule : TerrariaNetModule
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
                PayloadMemory = value ?? System.Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> PayloadMemory { get; set; }

        public UnknownNetModule(BinaryReader br)
            : base(br)
        {
            PayloadMemory = br.ReadRemainingMemory();
        }

        public override short GetLength()
        {
            return (short)PayloadMemory.Length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            base.ToStream(stream, includeHeader);
            if (PayloadMemory.Length > 0)
            {
                stream.Write(PayloadMemory.Span);
            }
        }
    }
}
