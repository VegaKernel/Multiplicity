using System;
using System.Buffers.Binary;

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
                ushort shotX = reader.ReadUInt16();
                ushort shotY = reader.ReadUInt16();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                PlayerId = playerId;
                CupX = cupX;
                CupY = cupY;
                ShotX = shotX;
                ShotY = shotY;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte PlayerId { get; }
            public ushort CupX { get; }
            public ushort CupY { get; }
            public ushort ShotX { get; }
            public ushort ShotY { get; }
        }
}

