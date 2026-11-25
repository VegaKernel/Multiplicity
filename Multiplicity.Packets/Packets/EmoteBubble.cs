using System.IO;
using System.Text;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The EmoteBubble (0x5B) packet.
    /// </summary>
    public class EmoteBubble : TerrariaPacket
    {
        private const short FixedPayloadLength = 5;

        public int BubbleId { get; set; }
        public EmoteBubbleAnchorType AnchorType { get; set; } = EmoteBubbleAnchorType.Removed;
        public ushort AnchorMetaData { get; set; }
        public ProjectileKey ProjectileKey { get; set; }
        public ushort Lifetime { get; set; }
        public byte Emote { get; set; }

        public EmoteBubble()
            : base((byte)PacketTypes.EmoteBubble)
        {
        }

        public EmoteBubble(BinaryReader br)
            : base(br)
        {
            BubbleId = br.ReadInt32();
            AnchorType = (EmoteBubbleAnchorType)br.ReadByte();
            switch (AnchorType)
            {
                case EmoteBubbleAnchorType.Removed: return;
                case EmoteBubbleAnchorType.Npc: AnchorMetaData = br.ReadUInt16(); break;
                case EmoteBubbleAnchorType.Player: AnchorMetaData = br.ReadByte(); break;
                case EmoteBubbleAnchorType.Projectile: ProjectileKey = ProjectileKey.FromPacked(br.ReadInt32()); break;
            }

            Lifetime = br.ReadUInt16();
            Emote = br.ReadByte();
        }

        public bool HasAnchorData => AnchorType is EmoteBubbleAnchorType.Npc or EmoteBubbleAnchorType.Player or EmoteBubbleAnchorType.Projectile;

        public override short GetLength() => AnchorType switch
        {
            EmoteBubbleAnchorType.Removed => FixedPayloadLength,
            EmoteBubbleAnchorType.Npc => FixedPayloadLength + 5,
            EmoteBubbleAnchorType.Player => FixedPayloadLength + 4,
            EmoteBubbleAnchorType.Projectile => FixedPayloadLength + 7,
            _ => FixedPayloadLength + 3
        };

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            byte playerIndex = AnchorType == EmoteBubbleAnchorType.Player ? checked((byte)AnchorMetaData) : (byte)0;
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(BubbleId);
                bw.Write((byte)AnchorType);
                switch (AnchorType)
                {
                    case EmoteBubbleAnchorType.Removed: return;
                    case EmoteBubbleAnchorType.Npc: bw.Write(AnchorMetaData); break;
                    case EmoteBubbleAnchorType.Player: bw.Write(playerIndex); break;
                    case EmoteBubbleAnchorType.Projectile: bw.Write(ProjectileKey.ToInt32()); break;
                }
                bw.Write(Lifetime);
                bw.Write(Emote);
            }
        }

        public override string ToString()
        {
            return $"[EmoteBubble: BubbleId = {BubbleId}, AnchorType = {AnchorType}, Lifetime = {Lifetime}, Emote = {Emote}]";
        }
    }
}
