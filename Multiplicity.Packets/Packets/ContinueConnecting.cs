using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ContinueConnecting (0x03) packet for Terraria 1.4.5.x.
    /// </summary>
    public class ContinueConnecting : TerrariaPacket
    {
        public const short PayloadLength = 2;

        public byte PlayerId { get; set; }

        /// <summary>
        /// Gets or sets Terraria's third server-special flag, transmitted as the second byte
        /// of packet 3 during connection bootstrap.
        /// </summary>
        public bool ServerSpecialFlag2 { get; set; }

        public ContinueConnecting()
            : base((byte)PacketTypes.ContinueConnecting)
        {
        }

        public ContinueConnecting(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            ServerSpecialFlag2 = br.ReadBoolean();
        }

        public override string ToString()
        {
            return $"[ContinueConnecting: PlayerId = {PlayerId} ServerSpecialFlag2 = {ServerSpecialFlag2}]";
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

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(PlayerId);
            bw.Write(ServerSpecialFlag2);
        }
    }
}
