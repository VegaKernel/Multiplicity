using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetCreativeUnlocksPlayerReportModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NetCreativeUnlocksPlayerReportModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            UserId = reader.ReadByte();
            ItemId = reader.ReadUInt16();
            Amount = reader.ReadUInt16();
            reader.EnsureEnd();
            _sourceSpan = payload;
        }

        public NetCreativeUnlocksPlayerReportModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.NetCreativeUnlocksPlayerReportModule);
        }

        public static NetCreativeUnlocksPlayerReportModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetCreativeUnlocksPlayerReportModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte UserId { get; }
        public ushort ItemId { get; }
        public ushort Amount { get; }
    }
}