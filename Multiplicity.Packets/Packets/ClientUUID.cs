using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ClientUUID (0x44) packet.
    /// </summary>
    public class ClientUUID : TerrariaPacket
    {
        public string UUID { get; set; }

        public ClientUUID()
            : base((byte)PacketTypes.ClientUUID)
        {
            UUID = string.Empty;
        }

        public ClientUUID(BinaryReader br)
            : base(br)
        {
            UUID = br.ReadString();
        }

        public override short GetLength()
        {
            return (short)PacketSerializationHelper.GetSerializedStringLength(UUID);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(UUID ?? string.Empty);
            }
        }

        public override string ToString()
        {
            return $"[ClientUUID: UUID = {UUID}]";
        }
    }
}
