using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Models;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public sealed class Protocol329WireTests
    {
        [Fact]
        public void ServerInfo_HandlesEmptyRequestAndBothResponseLayouts()
        {
            byte[] request = [3, 0, 163];
            Assert.True(Assert.IsType<ServerInfo>(PacketDecoder.CommonExtensions.Decode(request)).IsRequest);
            Assert.True(PacketViewParser.ParseExact(request).AsServerInfoView().IsRequest);
            Assert.Equal(request, new ServerInfo().ToArray(includeHeader: true));

            byte[] local = [0x61, 0x1e, 0, 0, 1, 87, 1, 72, 0x68, 0x10, 1, 3, 0, 0, 0, 8, 2, 0, 0];
            byte[] remote = [.. local[..^1], 1, 7, .. "1.2.3.4"u8, 0x62, 0x1e, 0, 0];
            foreach (byte[] payload in new[] { local, remote })
            {
                ServerInfo packet = RoundTrip<ServerInfo>(PacketTypes.ServerInfo, payload);
                ServerInfoView view = ServerInfoView.FromPayload(payload);
                Assert.False(view.IsRequest);
                Assert.Equal(7777, packet.ListenPort);
                Assert.Equal("W"u8.ToArray(), view.WorldNameUtf8.ToArray());
                Assert.Equal("H", packet.HostName);
                Assert.Equal((ushort)4200, view.WorldWidth);
                Assert.True(packet.HasCrimson);
                Assert.Equal(3, packet.GameMode);
                Assert.Equal((byte)8, view.MaxPlayers);
                Assert.Equal((byte)2, view.ActivePlayers);
                Assert.Equal(payload == remote, view.HasRemoteAddress);
            }
            Assert.Equal(7778, ServerInfoView.FromPayload(remote).RemotePort);
            Assert.Equal("1.2.3.4", ServerInfoView.FromPayload(remote).Packet.RemoteAddress);
        }

        [Fact]
        public void PlayerPlatformInfo_OnlyNintendoCarriesPlatformName()
        {
            byte[] nintendo = [7, 5, 2, 0xc3, 0xa9, 1, 78, 1];
            byte[] steam = [8, 6, 1, 83, 0];
            Assert.Equal("é", RoundTrip<PlayerPlatformInfo>(PacketTypes.PlayerPlatformInfo, nintendo).PlatformUserId);
            Assert.Equal("N"u8.ToArray(), PlayerPlatformInfoView.FromPayload(nintendo).PlatformNameUtf8.ToArray());
            Assert.True(PlayerPlatformInfoView.FromPayload(nintendo).IsHost);
            Assert.Equal(string.Empty, RoundTrip<PlayerPlatformInfo>(PacketTypes.PlayerPlatformInfo, steam).PlatformName);
            Assert.True(PlayerPlatformInfoView.FromPayload(steam).PlatformNameUtf8.IsEmpty);
        }

        [Fact]
        public void CustomServerAck_UsesSevenBitEncodedInt()
        {
            byte[] positive = [0xac, 0x02];
            byte[] negative = [255, 255, 255, 255, 15];
            Assert.Equal(300, RoundTrip<CustomServerAck>(PacketTypes.CustomServerAck, positive).Value);
            Assert.Equal(-1, RoundTrip<CustomServerAck>(PacketTypes.CustomServerAck, negative).Value);
            Assert.Equal(-1, CustomServerAckView.FromPayload(negative).Value);
        }

        [Fact]
        public void PlayerAnimation_EndsWithItemAnimationMax()
        {
            byte[] payload = [7, 0, 0, 128, 63, 1, 0, 0x34, 0x12];
            Assert.Equal((short)0x1234, RoundTrip<PlayerAnimation>(PacketTypes.PlayerAnimation, payload).ItemAnimationMax);
            Assert.Equal((short)0x1234, PlayerAnimationView.FromPayload(payload).ItemAnimationMax);
        }

        [Fact]
        public void TweakItem_SkipsRemovedSizeFieldsWithoutMovingOtherBits()
        {
            byte[] payload = [1, 0, 0x80, 0x0f, 0, 0, 128, 63, 0x34, 0x12];
            TweakItem packet = RoundTrip<TweakItem>(PacketTypes.TweakItem, payload);
            Assert.Equal(1f, packet.Scale);
            Assert.Equal((short)0x1234, TweakItemView.FromPayload(payload).Ammo);
        }

        [Fact]
        public void EmoteBubble_UsesAnchorSpecificWidthAndNoExtraEmoteMetadata()
        {
            byte[] npc = [1, 0, 0, 0, 0, 0x34, 0x12, 1, 0, 255];
            byte[] player = [1, 0, 0, 0, 1, 7, 1, 0, 134];
            byte[] projectile = [1, 0, 0, 0, 2, 0x07, 0x34, 0x12, 0xfe, 1, 0, 42];
            byte[] none = [1, 0, 0, 0, 3, 1, 0, 42];
            byte[] removed = [1, 0, 0, 0, 255];

            Assert.Equal((ushort)0x1234, RoundTrip<EmoteBubble>(PacketTypes.EmoteBubble, npc).AnchorMetaData);
            Assert.Equal(byte.MaxValue, EmoteBubbleView.FromPayload(npc).Emote);
            Assert.Equal((ushort)7, RoundTrip<EmoteBubble>(PacketTypes.EmoteBubble, player).AnchorMetaData);
            Assert.Equal(ProjectileKey.FromPacked(0xfe123407u), RoundTrip<EmoteBubble>(PacketTypes.EmoteBubble, projectile).ProjectileKey);
            Assert.Equal(0xfe123407u, EmoteBubbleView.FromPayload(projectile).ProjectileKey.PackedValue);
            Assert.Equal((byte)42, RoundTrip<EmoteBubble>(PacketTypes.EmoteBubble, none).Emote);
            Assert.False(RoundTrip<EmoteBubble>(PacketTypes.EmoteBubble, removed).HasAnchorData);
        }

        [Fact]
        public void NewFlagBits_PreserveNeighboringFlagsAndPayloadSizes()
        {
            WorldInfo world = new WorldInfo { EventInfo11 = 0x91, NoSpidersSeed = true };
            byte[] worldPayload = world.ToPayloadArray();
            WorldInfoView worldView = WorldInfoView.FromPayload(worldPayload);
            Assert.Equal((byte)0xb1, worldView.EventInfo11);
            Assert.True(worldView.NoSpidersSeed);
            world.NoSpidersSeed = false;
            Assert.Equal((byte)0x91, world.EventInfo11);
            Assert.Equal(worldPayload.Length, world.GetLength());

            byte[] hurtPayload = [2, 0, 42, 0, 1, 7, 0];
            PlayerHurtV2 hurt = RoundTrip<PlayerHurtV2>(PacketTypes.PlayerHurtV2, hurtPayload);
            Assert.True(PlayerHurtV2View.FromPayload(hurtPayload).Dodgeable);
            hurt.Dodgeable = false;
            Assert.Equal((byte)3, hurt.Flags);
        }

        private static TPacket RoundTrip<TPacket>(PacketTypes type, byte[] payload) where TPacket : TerrariaPacket
        {
            TPacket packet = Assert.IsType<TPacket>(TerrariaPacket.DeserializePayload(type, payload));
            Assert.Equal(payload, packet.ToPayloadArray());
            return packet;
        }
    }
}
