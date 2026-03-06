using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SendPassword (0x26) packet.
    /// </summary>
    public class SendPassword : TerrariaPacket
    {
        public string Password { get; set; }

        public SendPassword()
            : base((byte)PacketTypes.SendPassword)
        {
            Password = string.Empty;
        }

        public SendPassword(BinaryReader br)
            : base(br)
        {
            Password = br.ReadString();
        }

        public override short GetLength()
        {
            return (short)PacketSerializationHelper.GetSerializedStringLength(Password);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(Password ?? string.Empty);
            }
        }

        public override string ToString()
        {
            return $"[SendPassword: Password = {Password}]";
        }
    }
}
