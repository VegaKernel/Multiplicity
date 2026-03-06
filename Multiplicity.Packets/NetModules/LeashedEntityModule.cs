using System;
using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class LeashedEntityModule : TerrariaNetModule
    {
        public LeashedEntityMessageType MessageType { get; set; }

        public int Slot { get; set; }

        public int EntityType { get; set; }

        public LeashedEntityType EntityKind
        {
            get => (LeashedEntityType)EntityType;
            set => EntityType = (int)value;
        }

        public short AnchorX { get; set; }

        public short AnchorY { get; set; }

        public LeashedEntityPayload EntityData { get; set; }

        public byte[] EntityPayload
        {
            get => EntityData.ToPayloadArray(MessageType == LeashedEntityMessageType.FullSync);
            set => EntityData = new RawLeashedEntityPayload { Payload = value ?? Array.Empty<byte>() };
        }

        public ReadOnlyMemory<byte> EntityPayloadMemory
        {
            get => EntityData.ToPayloadArray(MessageType == LeashedEntityMessageType.FullSync);
            set => EntityData = new RawLeashedEntityPayload { Payload = value.ToArray() };
        }

        public LeashedEntityModule()
            : base(NetworkModuleTypes.LeashedEntityModule)
        {
            EntityData = new RawLeashedEntityPayload();
        }

        public LeashedEntityModule(BinaryReader br)
            : base(br)
        {
            MessageType = (LeashedEntityMessageType)br.ReadByte();
            Slot = br.Read7BitEncodedInt();
            EntityData = new RawLeashedEntityPayload();

            switch (MessageType)
            {
                case LeashedEntityMessageType.FullSync:
                    EntityType = br.Read7BitEncodedInt();
                    AnchorX = br.ReadInt16();
                    AnchorY = br.ReadInt16();
                    EntityData = LeashedEntityPayloadSerializer.Deserialize(EntityKind, fullSync: true, br.ReadRemainingMemory().Span);
                    break;
                case LeashedEntityMessageType.PartialSync:
                    EntityType = br.Read7BitEncodedInt();
                    EntityData = LeashedEntityPayloadSerializer.Deserialize(EntityKind, fullSync: false, br.ReadRemainingMemory().Span);
                    break;
            }
        }

        public override short GetLength()
        {
            int length = sizeof(byte) + PacketSerializationHelper.Get7BitEncodedIntLength(Slot);
            switch (MessageType)
            {
                case LeashedEntityMessageType.FullSync:
                    length += PacketSerializationHelper.Get7BitEncodedIntLength(EntityType);
                    length += sizeof(short) * 2;
                    length += EntityData.GetPayloadLength(fullSync: true);
                    break;
                case LeashedEntityMessageType.PartialSync:
                    length += PacketSerializationHelper.Get7BitEncodedIntLength(EntityType);
                    length += EntityData.GetPayloadLength(fullSync: false);
                    break;
            }

            return (short)length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write((byte)MessageType);
            bw.Write7BitEncodedInt(Slot);

            switch (MessageType)
            {
                case LeashedEntityMessageType.FullSync:
                    bw.Write7BitEncodedInt(EntityType);
                    bw.Write(AnchorX);
                    bw.Write(AnchorY);
                    EntityData.Write(bw, fullSync: true);
                    break;
                case LeashedEntityMessageType.PartialSync:
                    bw.Write7BitEncodedInt(EntityType);
                    EntityData.Write(bw, fullSync: false);
                    break;
            }
        }
    }
}
