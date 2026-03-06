using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetParticlesModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public NetParticlesModuleView(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            ParticleType = reader.ReadByte();
            PositionX = reader.ReadSingle();
            PositionY = reader.ReadSingle();
            MovementX = reader.ReadSingle();
            MovementY = reader.ReadSingle();
            UniqueInfoPiece = reader.ReadInt32();
            InvokingPlayerIndex = reader.ReadByte();
            reader.EnsureEnd();
            _sourceSpan = payload;
        }

        public NetParticlesModuleView(LoadNetModuleView module)
            : this(module.ModulePayloadSpan)
        {
            NetModuleViewHelper.EnsureType(module, NetworkModuleTypes.NetParticlesModule);
        }

        public static NetParticlesModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetParticlesModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte ParticleType { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public float MovementX { get; }
        public float MovementY { get; }
        public int UniqueInfoPiece { get; }
        public byte InvokingPlayerIndex { get; }
    }
}