using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SyncItemDespawn : TerrariaPacket
        {
            private const short PayloadLength = 2;
    
            public short ItemId { get; set; }
    
            public SyncItemDespawn()
                : base((byte)PacketTypes.SyncItemDespawn)
            {
            }
    
            public SyncItemDespawn(BinaryReader br)
                : base(br)
            {
                ItemId = br.ReadInt16();
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
                    bw.Write(ItemId);
                }
            }
        }
}

