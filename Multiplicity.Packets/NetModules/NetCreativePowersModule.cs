using System;
using System.IO;

namespace Multiplicity.Packets
{
    public sealed class NetCreativePowersModule : TerrariaNetModule
    {
        private byte[] _powerPayload;

        public ushort PowerId { get; set; }

        public byte[] PowerPayload
        {
            get
            {
                return _powerPayload ??= PowerPayloadMemory.ToArray();
            }
            set
            {
                _powerPayload = value;
                PowerPayloadMemory = value ?? Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> PowerPayloadMemory { get; set; }

        public NetCreativePowersModule()
            : base(NetworkModuleTypes.NetCreativePowersModule)
        {
            PowerPayloadMemory = ReadOnlyMemory<byte>.Empty;
            _powerPayload = Array.Empty<byte>();
        }

        public NetCreativePowersModule(BinaryReader br)
            : base(br)
        {
            PowerId = br.ReadUInt16();
            PowerPayloadMemory = br.ReadRemainingMemory();
        }

        public override short GetLength()
        {
            return (short)(sizeof(ushort) + PowerPayloadMemory.Length);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(PowerId);
            if (PowerPayloadMemory.Length > 0)
            {
                stream.Write(PowerPayloadMemory.Span);
            }
        }
    }
}