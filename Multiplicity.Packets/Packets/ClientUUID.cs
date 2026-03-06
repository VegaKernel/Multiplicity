using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ClientUUID (0x44) packet.
    /// </summary>
    public class ClientUUID : TerrariaPacket
    {
        public string Uuid { get; set; }

        public ClientUUID()
            : base((byte)PacketTypes.ClientUUID)
        {
            Uuid = string.Empty;
        }

        public ClientUUID(BinaryReader br)
            : base(br)
        {
            Uuid = br.ReadString();
        }

        public override short GetLength()
        {
            return (short)PacketSerializationHelper.GetSerializedStringLength(Uuid);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(Uuid ?? string.Empty);
            }
        }

        public override string ToString()
        {
            return $"[ClientUUID: Uuid = {Uuid}]";
        }
    }
}

