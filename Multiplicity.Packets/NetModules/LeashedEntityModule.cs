using System;
using System.IO;

namespace Multiplicity.Packets
{
    public sealed class LeashedEntityModule : TerrariaNetModule
    {
        private byte[] _entityPayload;

        public LeashedEntityMessageType MessageType { get; set; }

        public int Slot { get; set; }

        public int EntityType { get; set; }

        public short AnchorX { get; set; }

        public short AnchorY { get; set; }

        public byte[] EntityPayload
        {
            get
            {
                return _entityPayload ??= EntityPayloadMemory.ToArray();
            }
            set
            {
                _entityPayload = value;
                EntityPayloadMemory = value ?? Array.Empty<byte>();
            }
        }

        public ReadOnlyMemory<byte> EntityPayloadMemory { get; set; }

        public LeashedEntityModule()
            : base(NetworkModuleTypes.LeashedEntityModule)
        {
            EntityPayloadMemory = ReadOnlyMemory<byte>.Empty;
            _entityPayload = Array.Empty<byte>();
        }

        public LeashedEntityModule(BinaryReader br)
            : base(br)
        {
            MessageType = (LeashedEntityMessageType)br.ReadByte();
            Slot = br.Read7BitEncodedInt();

            switch (MessageType)
            {
                case LeashedEntityMessageType.FullSync:
                    EntityType = br.Read7BitEncodedInt();
                    AnchorX = br.ReadInt16();
                    AnchorY = br.ReadInt16();
                    EntityPayloadMemory = br.ReadRemainingMemory();
                    break;
                case LeashedEntityMessageType.PartialSync:
                    EntityType = br.Read7BitEncodedInt();
                    EntityPayloadMemory = br.ReadRemainingMemory();
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
                    length += EntityPayloadMemory.Length;
                    break;
                case LeashedEntityMessageType.PartialSync:
                    length += PacketSerializationHelper.Get7BitEncodedIntLength(EntityType);
                    length += EntityPayloadMemory.Length;
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
                    if (EntityPayloadMemory.Length > 0)
                    {
                        stream.Write(EntityPayloadMemory.Span);
                    }
                    break;
                case LeashedEntityMessageType.PartialSync:
                    bw.Write7BitEncodedInt(EntityType);
                    if (EntityPayloadMemory.Length > 0)
                    {
                        stream.Write(EntityPayloadMemory.Span);
                    }
                    break;
            }
        }
    }
}