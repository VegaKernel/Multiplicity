using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetBestiaryModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NetBestiaryModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            UnlockType = (NetBestiaryUnlockType)reader.ReadByte();
            NpcNetId = reader.ReadInt16();
            KillCount = UnlockType == NetBestiaryUnlockType.Kill ? reader.Read7BitEncodedInt() : 0;
            reader.EnsureEnd();
            _sourceSpan = payload;
        }

        public NetBestiaryModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.NetBestiaryModule);
        }

        public static NetBestiaryModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetBestiaryModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public NetBestiaryUnlockType UnlockType { get; }
        public short NpcNetId { get; }
        public int KillCount { get; }
    }
}