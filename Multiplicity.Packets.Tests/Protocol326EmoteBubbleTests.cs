using Multiplicity.Packets;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol326EmoteBubbleTests
    {
        [Fact]
        public void HighPositiveEmote_DoesNotConsumeItemDisplayMetadata()
        {
            EmoteBubble packet = new EmoteBubble
            {
                BubbleId = 123,
                AnchorType = EmoteBubbleAnchorType.Player,
                AnchorMetaData = 7,
                Lifetime = 360,
                Emote = 134,
                ExtraMetaData = 999
            };

            byte[] payload = packet.ToPayloadArray();
            EmoteBubble parsed = Assert.IsType<EmoteBubble>(
                TerrariaPacket.DeserializePayload(PacketTypes.EmoteBubble, payload));
            EmoteBubbleView view = EmoteBubbleView.FromPayload(payload);

            Assert.Equal(10, payload.Length);
            Assert.False(packet.HasExtraMetadata);
            Assert.False(parsed.HasExtraMetadata);
            Assert.Equal((byte)134, parsed.Emote);
            Assert.Equal((byte)134, view.Emote);
            Assert.Equal((short)0, parsed.ExtraMetaData);
        }

        [Fact]
        public void ItemDisplaySentinel_WritesAndReadsExtraMetadata()
        {
            EmoteBubble packet = new EmoteBubble
            {
                BubbleId = 456,
                AnchorType = EmoteBubbleAnchorType.Npc,
                AnchorMetaData = 12,
                Lifetime = 180,
                Emote = byte.MaxValue,
                ExtraMetaData = 321
            };

            byte[] payload = packet.ToPayloadArray();
            EmoteBubble parsed = Assert.IsType<EmoteBubble>(
                TerrariaPacket.DeserializePayload(PacketTypes.EmoteBubble, payload));
            EmoteBubbleView view = EmoteBubbleView.FromPayload(payload);

            Assert.Equal(12, payload.Length);
            Assert.True(packet.HasExtraMetadata);
            Assert.True(parsed.HasExtraMetadata);
            Assert.Equal(byte.MaxValue, parsed.Emote);
            Assert.Equal((short)321, parsed.ExtraMetaData);
            Assert.Equal(byte.MaxValue, view.Emote);
            Assert.Equal((short)321, view.ExtraMetaData);
        }

        [Fact]
        public void RemovedBubble_HasOnlyIdAndAnchorMarker()
        {
            EmoteBubble packet = new EmoteBubble
            {
                BubbleId = 789,
                AnchorType = EmoteBubbleAnchorType.Removed,
                Emote = byte.MaxValue,
                ExtraMetaData = 777
            };

            byte[] payload = packet.ToPayloadArray();
            EmoteBubble parsed = Assert.IsType<EmoteBubble>(
                TerrariaPacket.DeserializePayload(PacketTypes.EmoteBubble, payload));

            Assert.Equal(5, payload.Length);
            Assert.False(parsed.HasAnchorData);
            Assert.Equal((byte)0, parsed.Emote);
            Assert.False(parsed.HasExtraMetadata);
        }
    }
}
