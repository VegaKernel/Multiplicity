using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The UpdateNPCName (0x38) packet.
    /// </summary>
    public class UpdateNPCName : TerrariaPacket
    {
        private const short FixedPayloadLength = 2;

        public short NpcId { get; set; }
        public string Name { get; set; }

        public UpdateNPCName()
            : base((byte)PacketTypes.UpdateNPCName)
        {
            Name = string.Empty;
        }

        public UpdateNPCName(BinaryReader br)
            : base(br)
        {
            NpcId = br.ReadInt16();
            Name = br.BaseStream.Length > br.BaseStream.Position ? br.ReadString() : string.Empty;
        }

        public override short GetLength()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return FixedPayloadLength;
            }

            return (short)(FixedPayloadLength + PacketSerializationHelper.GetSerializedStringLength(Name));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(NpcId);
                if (!string.IsNullOrEmpty(Name))
                {
                    bw.Write(Name);
                }
            }
        }

        public override string ToString()
        {
            return $"[UpdateNPCName: NpcId = {NpcId} Name = {Name}]";
        }
    }
}

