using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The EmoteBubble (0x5B) packet.
    /// </summary>
    public class EmoteBubble : TerrariaPacket
    {
        private const short FixedPayloadLength = 5;
        private const short AnchorPayloadLength = 5;
        private const short EmoteMetadataPayloadLength = 2;

        public int BubbleId { get; set; }
        public EmoteBubbleAnchorType AnchorType { get; set; } = EmoteBubbleAnchorType.Removed;
        public ushort AnchorMetaData { get; set; }
        public ushort Lifetime { get; set; }
        public byte Emote { get; set; }
        public short ExtraMetaData { get; set; }

        public EmoteBubble()
            : base((byte)PacketTypes.EmoteBubble)
        {
        }

        public EmoteBubble(BinaryReader br)
            : base(br)
        {
            BubbleId = br.ReadInt32();
            AnchorType = (EmoteBubbleAnchorType)br.ReadByte();
            if (HasAnchorData)
            {
                AnchorMetaData = br.ReadUInt16();
                Lifetime = br.ReadUInt16();
                Emote = br.ReadByte();
                if (HasExtraMetadata)
                {
                    ExtraMetaData = br.ReadInt16();
                }
            }
        }

        public bool HasAnchorData => AnchorType != EmoteBubbleAnchorType.Removed;

        /// <summary>
        /// Terraria uses emote -1 (ItemDisplay) as the metadata-bearing sentinel. It is serialized
        /// through BinaryWriter.Write(byte) as 0xFF. Ordinary emote ids include values above 127,
        /// so interpreting every high-bit byte as a negative signed value would consume two bytes
        /// that are not present for emotes 128..150.
        /// </summary>
        public bool HasExtraMetadata => Emote == byte.MaxValue;

        public override short GetLength()
        {
            short length = FixedPayloadLength;

            if (HasAnchorData)
            {
                length += AnchorPayloadLength;
                if (HasExtraMetadata)
                {
                    length += EmoteMetadataPayloadLength;
                }
            }

            return length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(BubbleId);
                bw.Write((byte)AnchorType);
                if (HasAnchorData)
                {
                    bw.Write(AnchorMetaData);
                    bw.Write(Lifetime);
                    bw.Write(Emote);
                    if (HasExtraMetadata)
                    {
                        bw.Write(ExtraMetaData);
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"[EmoteBubble: BubbleId = {BubbleId}, AnchorType = {AnchorType}, AnchorMetaData = {AnchorMetaData}, Lifetime = {Lifetime}, Emote = {Emote}, ExtraMetaData = {ExtraMetaData}]";
        }
    }
}
