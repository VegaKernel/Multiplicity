using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class TEDeadCellsDisplayJar : TileEntityItemSyncPacketBase
        {
            public TEDeadCellsDisplayJar()
                : base((byte)PacketTypes.TEDeadCellsDisplayJar)
            {
            }
    
            public TEDeadCellsDisplayJar(BinaryReader br)
                : base(br)
            {
            }
        }
}

