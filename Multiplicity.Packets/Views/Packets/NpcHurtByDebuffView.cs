using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NpcHurtByDebuffView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public NpcHurtByDebuffView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.NpcHurtByDebuff);
                PacketReader reader = packet.CreatePayloadReader();
                byte npcId = reader.ReadByte();
                short damage = reader.ReadInt16();
                reader.EnsureEnd();
    
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                NpcId = npcId;
                Damage = damage;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public byte NpcId { get; }
            public short Damage { get; }
        }
}

