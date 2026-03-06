using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class CrystalInvasionRequestedToSkipWaitTime : TerrariaPacket
        {
            private const short PayloadLength = 0;
    
            public CrystalInvasionRequestedToSkipWaitTime()
                : base((byte)PacketTypes.CrystalInvasionRequestedToSkipWaitTime)
            {
            }
    
            public CrystalInvasionRequestedToSkipWaitTime(BinaryReader br)
                : base(br)
            {
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
            }
        }
}

