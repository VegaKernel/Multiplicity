using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SyncEmoteBubble (0x5B) packet.
    /// </summary>
    public class SyncEmoteBubble : TerrariaPacket
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

        public SyncEmoteBubble()
            : base((byte)PacketTypes.SyncEmoteBubble)
        {
        }

        public SyncEmoteBubble(BinaryReader br)
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

        public bool HasExtraMetadata => unchecked((sbyte)Emote) < 0;

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
            return $"[SyncEmoteBubble: BubbleId = {BubbleId}, AnchorType = {AnchorType}, AnchorMetaData = {AnchorMetaData}, Lifetime = {Lifetime}, Emote = {Emote}, ExtraMetaData = {ExtraMetaData}]";
        }
    }
}
