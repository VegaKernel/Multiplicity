using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class FinishedConnectingToServer : TerrariaPacket
        {
            private const short PayloadLength = 0;
    
            public FinishedConnectingToServer()
                : base((byte)PacketTypes.FinishedConnectingToServer)
            {
            }
    
            public FinishedConnectingToServer(BinaryReader br)
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

