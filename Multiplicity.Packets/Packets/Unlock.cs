using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The Unlock (0x34) packet.
    /// </summary>
    public class Unlock : TerrariaPacket
    {
        private const short PayloadLength = 5;

        public UnlockAction Action { get; set; }
        public short X { get; set; }
        public short Y { get; set; }

        public Unlock()
            : base((byte)PacketTypes.Unlock)
        {
        }

        public Unlock(BinaryReader br)
            : base(br)
        {
            Action = (UnlockAction)br.ReadByte();
            X = br.ReadInt16();
            Y = br.ReadInt16();
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
                bw.Write((byte)Action);
                bw.Write(X);
                bw.Write(Y);
            }
        }

        public override string ToString()
        {
            return $"[Unlock: Action = {Action}, X = {X}, Y = {Y}]";
        }
    }
}
