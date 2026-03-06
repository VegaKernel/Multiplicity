using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PasswordSend (0x26) packet.
    /// </summary>
    public class PasswordSend : TerrariaPacket
    {
        public string Password { get; set; }

        public PasswordSend()
            : base((byte)PacketTypes.PasswordSend)
        {
            Password = string.Empty;
        }

        public PasswordSend(BinaryReader br)
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
            return $"[PasswordSend: Password = {Password}]";
        }
    }
}
