using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct LandGolfBallInCupView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public LandGolfBallInCupView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.LandGolfBallInCup);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            ushort cupX = reader.ReadUInt16();
            ushort cupY = reader.ReadUInt16();
            ushort hits = reader.ReadUInt16();
            ushort projectileType = reader.ReadUInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            CupX = cupX;
            CupY = cupY;
            Hits = hits;
            ProjectileType = projectileType;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public ushort CupX { get; }
        public ushort CupY { get; }
        public ushort Hits { get; }
        public ushort ProjectileType { get; }

        [Obsolete("Protocol 325 uses this field as hit count. Use Hits.")]
        public ushort ShotX => Hits;

        [Obsolete("Protocol 325 uses this field as projectile type. Use ProjectileType.")]
        public ushort ShotY => ProjectileType;
    }
}
