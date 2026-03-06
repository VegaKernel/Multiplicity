using System;
using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class NetCreativePowersModule : TerrariaNetModule
    {
        public ushort PowerId { get; set; }

        public CreativePowerId PowerType
        {
            get => (CreativePowerId)PowerId;
            set => PowerId = (ushort)value;
        }

        public CreativePowerPayload PowerData { get; set; }

        public byte[] PowerPayload
        {
            get => PowerData.ToPayloadArray();
            set => PowerData = new RawCreativePowerPayload { Payload = value ?? Array.Empty<byte>() };
        }

        public ReadOnlyMemory<byte> PowerPayloadMemory
        {
            get => PowerData.ToPayloadArray();
            set => PowerData = new RawCreativePowerPayload { Payload = value.ToArray() };
        }

        public NetCreativePowersModule()
            : base(NetworkModuleTypes.NetCreativePowersModule)
        {
            PowerData = new RawCreativePowerPayload();
        }

        public NetCreativePowersModule(BinaryReader br)
            : base(br)
        {
            PowerId = br.ReadUInt16();
            PowerData = CreativePowerPayloadSerializer.Deserialize(PowerType, br.ReadRemainingMemory().Span);
        }

        public override short GetLength()
        {
            return checked((short)(sizeof(ushort) + PowerData.GetPayloadLength()));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(PowerId);
            PowerData.Write(bw);
        }
    }
}
