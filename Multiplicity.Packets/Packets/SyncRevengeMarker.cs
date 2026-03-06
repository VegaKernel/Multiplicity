using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SyncRevengeMarker : TerrariaPacket
        {
            public RevengeMarkerData Marker { get; set; } = new RevengeMarkerData();
    
            public SyncRevengeMarker()
                : base((byte)PacketTypes.SyncRevengeMarker)
            {
            }
    
            public SyncRevengeMarker(BinaryReader br)
                : base(br)
            {
                Marker = new RevengeMarkerData(br);
            }
    
            public override short GetLength()
            {
                return RevengeMarkerData.PayloadLength;
            }
    
            public override void ToStream(Stream stream, bool includeHeader = true)
            {
                if (includeHeader)
                {
                    base.ToStream(stream, includeHeader);
                }
    
                using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
                {
                    (Marker ?? new RevengeMarkerData()).ToStream(bw);
                }
            }
        }
}

