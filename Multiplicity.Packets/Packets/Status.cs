using System.IO;
using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The Status (0x09) packet.
    /// </summary>
    public class Status : TerrariaPacket
    {
        private const short FixedPayloadLength = 5;

        public int StatusMax { get; set; }
        public NetworkText StatusText { get; set; }
        public StatusSpecialFlags SpecialFlags { get; set; }

        public Status()
            : base((byte)PacketTypes.Status)
        {
            StatusText = new NetworkText();
        }

        public Status(BinaryReader br)
            : base(br)
        {
            StatusMax = br.ReadInt32();
            StatusText = br.ReadNetworkText();
            SpecialFlags = (StatusSpecialFlags)br.ReadByte();
        }

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + (StatusText?.GetLength() ?? 0));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(StatusMax);
                bw.Write(StatusText ?? new NetworkText());
                bw.Write((byte)SpecialFlags);
            }
        }

        public override string ToString()
        {
            return $"[Status: StatusMax = {StatusMax} StatusText = {StatusText?.Text} SpecialFlags = {SpecialFlags}]";
        }
    }
}
