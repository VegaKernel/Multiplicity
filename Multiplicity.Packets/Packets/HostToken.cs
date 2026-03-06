using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class HostToken : TerrariaPacket
        {
            public string Token { get; set; } = string.Empty;
    
            public HostToken()
                : base((byte)PacketTypes.HostToken)
            {
            }
    
            public HostToken(BinaryReader br)
                : base(br)
            {
                Token = br.ReadString();
            }
    
            public override short GetLength()
            {
                return (short)PacketSerializationHelper.GetSerializedStringLength(Token);
            }
    
            public override void ToStream(Stream stream, bool includeHeader = true)
            {
                if (includeHeader)
                {
                    base.ToStream(stream, includeHeader);
                }
    
                using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
                {
                    bw.Write(Token ?? string.Empty);
                }
            }
        }
}

