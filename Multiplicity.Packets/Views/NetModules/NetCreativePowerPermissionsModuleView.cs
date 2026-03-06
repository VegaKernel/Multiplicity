using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetCreativePowerPermissionsModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NetCreativePowerPermissionsModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            MessageType = (NetCreativePowerPermissionsMessageType)reader.ReadByte();
            PowerId = reader.ReadUInt16();
            PermissionLevel = reader.ReadByte();
            reader.EnsureEnd();
            _sourceSpan = payload;
        }

        public NetCreativePowerPermissionsModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.NetCreativePowerPermissionsModule);
        }

        public static NetCreativePowerPermissionsModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetCreativePowerPermissionsModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public NetCreativePowerPermissionsMessageType MessageType { get; }
        public ushort PowerId { get; }
        public byte PermissionLevel { get; }
    }
}