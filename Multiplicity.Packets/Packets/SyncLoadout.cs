using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SyncLoadout : TerrariaPacket
        {
            private const short PayloadLength = 4;
    
            public byte PlayerId { get; set; }
    
            public byte LoadoutIndex { get; set; }
    
            public ushort HiddenAccessoryVisibilityMask { get; set; }
    
            public SyncLoadout()
                : base((byte)PacketTypes.SyncLoadout)
            {
            }
    
            public SyncLoadout(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
                LoadoutIndex = br.ReadByte();
                HiddenAccessoryVisibilityMask = br.ReadUInt16();
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
                    bw.Write(LoadoutIndex);
                    bw.Write(HiddenAccessoryVisibilityMask);
                }
            }
        }
}

