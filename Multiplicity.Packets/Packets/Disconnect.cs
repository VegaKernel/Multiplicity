using System.IO;
using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The Disconnect (0x02) packet.
    /// </summary>
    public class Disconnect : TerrariaPacket
    {
        public NetworkText Reason { get; set; }

        public Disconnect()
            : base((byte)PacketTypes.Disconnect)
        {
            Reason = new NetworkText();
        }

        public Disconnect(BinaryReader br)
            : base(br)
        {
            Reason = br.ReadNetworkText();
        }

        public override short GetLength()
        {
            return (short)(Reason?.GetLength() ?? 0);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(Reason ?? new NetworkText());
            }
        }

        public override string ToString()
        {
            return $"[Disconnect: Reason = {Reason?.Text}]";
        }
    }
}
