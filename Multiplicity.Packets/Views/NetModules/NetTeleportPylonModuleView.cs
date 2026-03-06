using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetTeleportPylonModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NetTeleportPylonModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            MessageType = (NetTeleportPylonMessageType)reader.ReadByte();
            TileX = reader.ReadInt16();
            TileY = reader.ReadInt16();
            PylonType = reader.ReadByte();
            reader.EnsureEnd();
            _sourceSpan = payload;
        }

        public NetTeleportPylonModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.NetTeleportPylonModule);
        }

        public static NetTeleportPylonModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetTeleportPylonModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public NetTeleportPylonMessageType MessageType { get; }
        public short TileX { get; }
        public short TileY { get; }
        public byte PylonType { get; }
    }
}