using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ChestName (0x45) packet.
    /// </summary>
    public class ChestName : TerrariaPacket
    {
        private const short FixedPayloadLength = 6;

        public short ChestId { get; set; }

        public short ChestX { get; set; }

        public short ChestY { get; set; }

        /// <summary>
        /// Gets or sets whether the payload includes the trailing chest name string.
        /// Client-to-server lookup requests contain only the fixed six-byte prefix,
        /// while server-to-client responses append the string.
        /// </summary>
        public bool HasName { get; set; } = true;

        public string Name { get; set; }

        public ChestName()
            : base((byte)PacketTypes.ChestName)
        {
            Name = string.Empty;
        }

        public ChestName(BinaryReader br)
            : base(br)
        {
            ChestId = br.ReadInt16();
            ChestX = br.ReadInt16();
            ChestY = br.ReadInt16();

            HasName = br.BaseStream.Position < br.BaseStream.Length;
            Name = HasName ? br.ReadString() : string.Empty;
        }

        public override string ToString()
        {
            return $"[ChestName: ChestId = {ChestId} ChestX = {ChestX} ChestY = {ChestY} HasName = {HasName} Name = {Name}]";
        }

        public override short GetLength()
        {
            if (!HasName)
            {
                return FixedPayloadLength;
            }

            return checked((short)(FixedPayloadLength + PacketSerializationHelper.GetSerializedStringLength(Name)));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(ChestId);
            bw.Write(ChestX);
            bw.Write(ChestY);
            if (HasName)
            {
                bw.Write(Name ?? string.Empty);
            }
        }
    }
}
