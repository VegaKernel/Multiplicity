using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetPingModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NetPingModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            PositionX = reader.ReadSingle();
            PositionY = reader.ReadSingle();
            reader.EnsureEnd();
            _sourceSpan = payload;
        }

        public NetPingModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.NetPingModule);
        }

        public static NetPingModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetPingModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public float PositionX { get; }
        public float PositionY { get; }
    }
}