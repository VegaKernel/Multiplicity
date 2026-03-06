using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class UpdatePlayerLuckFactors : TerrariaPacket
        {
            private const short PayloadLength = 21;
    
            public byte PlayerId { get; set; }
    
            public int LadyBugLuckTimeLeft { get; set; }
    
            public float TorchLuck { get; set; }
    
            public byte LuckPotion { get; set; }
    
            public bool HasGardenGnomeNearby { get; set; }
    
            public bool BrokenMirrorBadLuck { get; set; }
    
            public float EquipmentBasedLuckBonus { get; set; }
    
            public float CoinLuck { get; set; }
    
            public byte KiteLuckLevel { get; set; }
    
            public UpdatePlayerLuckFactors()
                : base((byte)PacketTypes.UpdatePlayerLuckFactors)
            {
            }
    
            public UpdatePlayerLuckFactors(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
                LadyBugLuckTimeLeft = br.ReadInt32();
                TorchLuck = br.ReadSingle();
                LuckPotion = br.ReadByte();
                HasGardenGnomeNearby = br.ReadBoolean();
                BrokenMirrorBadLuck = br.ReadBoolean();
                EquipmentBasedLuckBonus = br.ReadSingle();
                CoinLuck = br.ReadSingle();
                KiteLuckLevel = br.ReadByte();
            }
    
            public override short GetLength()
            {
                return PayloadLength;
            }
    
            public override void ToStream(Stream stream, bool includeHeader = true)
            {
                if (includeHeader)
                {
                    base.ToStream(stream, includeHeader);
                }
    
                using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
                {
                    bw.Write(PlayerId);
                    bw.Write(LadyBugLuckTimeLeft);
                    bw.Write(TorchLuck);
                    bw.Write(LuckPotion);
                    bw.Write(HasGardenGnomeNearby);
                    bw.Write(BrokenMirrorBadLuck);
                    bw.Write(EquipmentBasedLuckBonus);
                    bw.Write(CoinLuck);
                    bw.Write(KiteLuckLevel);
                }
            }
        }
}

