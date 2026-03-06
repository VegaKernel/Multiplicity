using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The UpdateSign (0x2F) packet.
    /// </summary>
    public class UpdateSign : TerrariaPacket
    {
        private const short FixedPayloadLength = 7;

        public short SignID { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public string Text { get; set; }
        public byte PlayerID { get; set; }

        public UpdateSign()
            : base((byte)PacketTypes.UpdateSign)
        {
            Text = string.Empty;
        }

        public UpdateSign(BinaryReader br)
            : base(br)
        {
            SignID = br.ReadInt16();
            X = br.ReadInt16();
            Y = br.ReadInt16();
            Text = br.ReadString();
            PlayerID = br.ReadByte();
        }

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + PacketSerializationHelper.GetSerializedStringLength(Text));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(SignID);
                bw.Write(X);
                bw.Write(Y);
                bw.Write(Text ?? string.Empty);
                bw.Write(PlayerID);
            }
        }

        public override string ToString()
        {
            return $"[UpdateSign: SignID = {SignID} X = {X} Y = {Y} Text = {Text} PlayerID = {PlayerID}]";
        }
    }
}
