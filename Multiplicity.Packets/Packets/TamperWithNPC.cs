using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class TamperWithNPC : TerrariaPacket
        {
            private const short BasePayloadLength = 3;
            private const short ImmunityPayloadLength = 9;
    
            public ushort NpcId { get; set; }
    
            public TamperWithNPCActionType ActionType { get; set; }
    
            public int ImmunityTime { get; set; }
    
            public short PlayerId { get; set; }
    
            public TamperWithNPC()
                : base((byte)PacketTypes.TamperWithNPC)
            {
            }
    
            public TamperWithNPC(BinaryReader br)
                : base(br)
            {
                NpcId = br.ReadUInt16();
                ActionType = (TamperWithNPCActionType)br.ReadByte();
    
                if (ActionType == TamperWithNPCActionType.GrantImmunity)
                {
                    ImmunityTime = br.ReadInt32();
                    PlayerId = br.ReadInt16();
                }
            }
    
            public override short GetLength()
            {
                return ActionType == TamperWithNPCActionType.GrantImmunity ? ImmunityPayloadLength : BasePayloadLength;
            }
    
            public override void ToStream(Stream stream, bool includeHeader = true)
            {
                if (includeHeader)
                {
                    base.ToStream(stream, includeHeader);
                }
    
                using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
                {
                    bw.Write(NpcId);
                    bw.Write((byte)ActionType);
    
                    if (ActionType == TamperWithNPCActionType.GrantImmunity)
                    {
                        bw.Write(ImmunityTime);
                        bw.Write(PlayerId);
                    }
                }
            }
        }
}

