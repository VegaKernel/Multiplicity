using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class TileEntityDisplayDollItemSync : TerrariaPacket
        {
            private const short BasePayloadLength = 7;
            private const short PosePayloadLength = 1;
    
            public byte PlayerId { get; set; }
    
            public int TileEntityId { get; set; }
    
            public byte ItemIndex { get; set; }
    
            public DisplayDollSyncCommand Command { get; set; }
    
            public byte Pose { get; set; }
    
            public ItemSyncData ItemData { get; set; } = new ItemSyncData();
    
            public TileEntityDisplayDollItemSync()
                : base((byte)PacketTypes.TileEntityDisplayDollItemSync)
            {
            }
    
            public TileEntityDisplayDollItemSync(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
                TileEntityId = br.ReadInt32();
                ItemIndex = br.ReadByte();
                Command = (DisplayDollSyncCommand)br.ReadByte();
    
                if (Command == DisplayDollSyncCommand.Pose)
                {
                    Pose = br.ReadByte();
                    return;
                }
    
                ItemData = new ItemSyncData(br);
            }
    
            public override short GetLength()
            {
                return (short)(BasePayloadLength + (Command == DisplayDollSyncCommand.Pose ? PosePayloadLength : ItemSyncData.PayloadLength));
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
                    bw.Write(ItemIndex);
                    bw.Write((byte)Command);
    
                    if (Command == DisplayDollSyncCommand.Pose)
                    {
                        bw.Write(Pose);
                        return;
                    }
    
                    (ItemData ?? new ItemSyncData()).ToStream(bw);
                }
            }
        }
}

