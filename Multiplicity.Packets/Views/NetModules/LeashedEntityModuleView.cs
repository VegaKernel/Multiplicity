using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct LeashedEntityModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public LeashedEntityModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            MessageType = (LeashedEntityMessageType)reader.ReadByte();
            Slot = reader.Read7BitEncodedInt();
            EntityType = 0;
            AnchorX = 0;
            AnchorY = 0;
            EntityPayloadSpan = ReadOnlySpan<byte>.Empty;

            switch (MessageType)
            {
                case LeashedEntityMessageType.Remove:
                    reader.EnsureEnd();
                    break;
                case LeashedEntityMessageType.FullSync:
                    EntityType = reader.Read7BitEncodedInt();
                    AnchorX = reader.ReadInt16();
                    AnchorY = reader.ReadInt16();
                    EntityPayloadSpan = reader.ReadRemainingSpan();
                    break;
                case LeashedEntityMessageType.PartialSync:
                    EntityType = reader.Read7BitEncodedInt();
                    EntityPayloadSpan = reader.ReadRemainingSpan();
                    break;
                default:
                    reader.EnsureEnd();
                    break;
            }

            _sourceSpan = payload;
        }

        public LeashedEntityModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.LeashedEntityModule);
        }

        public static LeashedEntityModuleView FromPayload(ReadOnlySpan<byte> payload) => new LeashedEntityModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public LeashedEntityMessageType MessageType { get; }
        public int Slot { get; }
        public int EntityType { get; }
        public LeashedEntityType EntityKind => (LeashedEntityType)EntityType;
        public short AnchorX { get; }
        public short AnchorY { get; }
        public ReadOnlySpan<byte> EntityPayloadSpan { get; }
        public bool HasEntityPayload => MessageType != LeashedEntityMessageType.Remove;
        public PacketReader CreateEntityPayloadReader() => new PacketReader(EntityPayloadSpan);

        public LeashedEntityPayload GetPayload() => HasEntityPayload
            ? LeashedEntityPayloadSerializer.Deserialize(EntityKind, MessageType == LeashedEntityMessageType.FullSync, EntityPayloadSpan)
            : throw new InvalidOperationException("LeashedEntityModuleView does not contain an entity payload.");
    }
}
