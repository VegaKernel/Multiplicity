using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetAmbienceModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NetAmbienceModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            PlayerId = reader.ReadByte();
            Seed = reader.ReadInt32();
            SkyEntityType = reader.ReadByte();
            reader.EnsureEnd();
            _sourceSpan = payload;
        }

        public NetAmbienceModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.NetAmbienceModule);
        }

        public static NetAmbienceModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetAmbienceModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public int Seed { get; }
        public byte SkyEntityType { get; }
    }
}