using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class TileEntityHatRackItemSync : TerrariaPacket
        {
            private const short PayloadLength = 11;
    
            public byte PlayerId { get; set; }
    
            public int TileEntityId { get; set; }
    
            public byte EncodedSlot { get; set; }
    
            public ItemSyncData ItemData { get; set; } = new ItemSyncData();
    
            public TileEntityHatRackItemSync()
                : base((byte)PacketTypes.TileEntityHatRackItemSync)
            {
            }
    
            public TileEntityHatRackItemSync(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
                TileEntityId = br.ReadInt32();
                EncodedSlot = br.ReadByte();
                ItemData = new ItemSyncData(br);
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
                    bw.Write(TileEntityId);
                    bw.Write(EncodedSlot);
                    (ItemData ?? new ItemSyncData()).ToStream(bw);
                }
            }
        }
}

