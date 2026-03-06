using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetCreativePowersModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NetCreativePowersModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            PowerId = reader.ReadUInt16();
            PowerPayloadSpan = reader.ReadRemainingSpan();
            _sourceSpan = payload;
        }

        public NetCreativePowersModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.NetCreativePowersModule);
        }

        public static NetCreativePowersModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetCreativePowersModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public ushort PowerId { get; }
        public ReadOnlySpan<byte> PowerPayloadSpan { get; }
        public PacketReader CreatePowerPayloadReader() => new PacketReader(PowerPayloadSpan);
    }
}