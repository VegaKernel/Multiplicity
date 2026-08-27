using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The OpenSignResponse / SignNew (47) packet.
    /// </summary>
    public class SignNew : TerrariaPacket
    {
        private const short FixedPayloadLength = 8;

        public short SignId { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public string Text { get; set; }
        public byte PlayerId { get; set; }
        public byte Flags { get; set; }

        public bool SuppressOpen
        {
            get => (Flags & 1) != 0;
            set => Flags = value ? (byte)(Flags | 1) : (byte)(Flags & ~1);
        }

        public SignNew()
            : base((byte)PacketTypes.SignNew)
        {
            Text = string.Empty;
        }

        public SignNew(BinaryReader br)
            : base(br)
        {
            SignId = br.ReadInt16();
            X = br.ReadInt16();
            Y = br.ReadInt16();
            Text = br.ReadString();
            PlayerId = br.ReadByte();
            Flags = br.ReadByte();
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
                bw.Write(SignId);
                bw.Write(X);
                bw.Write(Y);
                bw.Write(Text ?? string.Empty);
                bw.Write(PlayerId);
                bw.Write(Flags);
            }
        }

        public override string ToString()
        {
            return $"[SignNew: SignId = {SignId} X = {X} Y = {Y} Text = {Text} PlayerId = {PlayerId} Flags = {Flags}]";
        }
    }
}
