using System;
using System.Buffers.Binary;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TamperWithNPCView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public TamperWithNPCView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.TamperWithNPC);
                PacketReader reader = packet.CreatePayloadReader();
                ushort npcId = reader.ReadUInt16();
                TamperWithNPCActionType actionType = (TamperWithNPCActionType)reader.ReadByte();
                int immunityTime = 0;
                short playerId = 0;
    
                if (actionType == TamperWithNPCActionType.GrantImmunity)
                {
                    immunityTime = reader.ReadInt32();
                    playerId = reader.ReadInt16();
                }
    
                reader.EnsureEnd();
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                NpcId = npcId;
                ActionType = actionType;
                ImmunityTime = immunityTime;
                PlayerId = playerId;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public ushort NpcId { get; }
            public TamperWithNPCActionType ActionType { get; }
            public int ImmunityTime { get; }
            public short PlayerId { get; }
            public bool HasImmunityPayload => ActionType == TamperWithNPCActionType.GrantImmunity;
        }
}

