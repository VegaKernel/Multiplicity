using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct UnbreakableWallScanModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public UnbreakableWallScanModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            PlayerId = reader.ReadByte();
            InsideUnbreakableWalls = reader.ReadBoolean();
            reader.EnsureEnd();
            _sourceSpan = payload;
        }

        public UnbreakableWallScanModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.UnbreakableWallScanModule);
        }

        public static UnbreakableWallScanModuleView FromPayload(ReadOnlySpan<byte> payload) => new UnbreakableWallScanModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public bool InsideUnbreakableWalls { get; }
    }
}