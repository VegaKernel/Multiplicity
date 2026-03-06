using System;
using System.IO;
using Multiplicity.Packets;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class PacketParsingTests
    {
        [Fact]
        public void DeserializeReadOnlyMemory_PlayerInfo_RoundTripsKeyFields()
        {
            PlayerInfo original = new PlayerInfo
            {
                PlayerId = 7,
                SkinVariant = 3,
                VoiceVariant = 2,
                VoicePitchOffset = 0.15f,
                Hair = 11,
                Name = "Tester",
                HairDye = 5,
                HideVisibleAccessory = 0x1234,
                HideMisc = 0x56,
                HairColor = CreateColor(1, 2, 3),
                SkinColor = CreateColor(4, 5, 6),
                EyeColor = CreateColor(7, 8, 9),
                ShirtColor = CreateColor(10, 11, 12),
                UnderShirtColor = CreateColor(13, 14, 15),
                PantsColor = CreateColor(16, 17, 18),
                ShoeColor = CreateColor(19, 20, 21),
                DifficultyFlags = PlayerDifficultyFlags.Creative | PlayerDifficultyFlags.ExtraAccessory,
                TorchAndCartFlags = PlayerTorchAndCartFlags.EnabledSuperCart,
                ConsumableUnlockFlags = PlayerConsumableUnlockFlags.UsedAegisCrystal | PlayerConsumableUnlockFlags.AteArtisanBread
            };

            byte[] bytes = original.ToArray(includeHeader: true);

            TerrariaPacket parsedPacket = TerrariaPacket.Deserialize((ReadOnlyMemory<byte>)bytes);
            PlayerInfo parsed = Assert.IsType<PlayerInfo>(parsedPacket);

            Assert.Equal(original.PlayerId, parsed.PlayerId);
            Assert.Equal(original.Name, parsed.Name);
            Assert.Equal(original.HideVisibleAccessory, parsed.HideVisibleAccessory);
            Assert.Equal(original.DifficultyFlags, parsed.DifficultyFlags);
            Assert.Equal(original.ConsumableUnlockFlags, parsed.ConsumableUnlockFlags);
            Assert.Equal(bytes, parsed.ToArray(includeHeader: true));
        }

        [Fact]
        public void PacketViewParser_TryParse_ConsumesFirstPacket_AndParsesPlayerUpdateView()
        {
            PlayerUpdate original = new PlayerUpdate
            {
                PlayerId = 9,
                ControlFlags = UpdatePlayerControlFlags.ControlJump | UpdatePlayerControlFlags.DirectionRight,
                MovementFlags = UpdatePlayerMovementFlags.HasVelocity | UpdatePlayerMovementFlags.HasMount,
                MiscFlags1 = UpdatePlayerMiscFlags1.HasPotionOfReturnPositions,
                MiscFlags2 = UpdatePlayerMiscFlags2.HasCameraTarget,
                SelectedItem = 4,
                PositionX = 100.5f,
                PositionY = 200.25f,
                VelocityX = 1.5f,
                VelocityY = -2.5f,
                MountType = 77,
                PotionOfReturnOriginalPositionX = 10f,
                PotionOfReturnOriginalPositionY = 20f,
                PotionOfReturnHomePositionX = 30f,
                PotionOfReturnHomePositionY = 40f,
                CameraTargetX = 55f,
                CameraTargetY = 65f
            };

            byte[] packetBytes = original.ToArray(includeHeader: true);
            byte[] combinedBuffer = new byte[packetBytes.Length + 3];
            Buffer.BlockCopy(packetBytes, 0, combinedBuffer, 0, packetBytes.Length);

            bool result = PacketViewParser.TryParse(combinedBuffer.AsSpan(), out PacketView packetView, out int consumed);

            Assert.True(result);
            Assert.Equal(packetBytes.Length, consumed);

            PlayerUpdateView view = packetView.AsPlayerUpdateView();
            Assert.Equal(original.PlayerId, view.PlayerId);
            Assert.Equal(original.PositionX, view.PositionX);
            Assert.Equal(original.PositionY, view.PositionY);
            Assert.Equal(original.VelocityX, view.VelocityX);
            Assert.Equal(original.VelocityY, view.VelocityY);
            Assert.Equal(original.MountType, view.MountType);
            Assert.True(view.HasVelocity);
            Assert.True(view.HasMount);
            Assert.True(view.HasPotionOfReturnPositions);
            Assert.True(view.HasCameraTarget);
        }

        [Fact]
        public void PacketViewParser_ParseExact_HostToken_UsesStringView()
        {
            HostToken original = new HostToken
            {
                Token = "token-123"
            };

            byte[] bytes = original.ToArray(includeHeader: true);
            PacketView packetView = PacketViewParser.ParseExact(bytes);
            StringPacketView view = packetView.AsStringPacketView();

            Assert.Equal(PacketTypes.HostToken, view.PacketType);
            Assert.Equal("token-123", view.GetValue());
        }

        [Fact]
        public void ModernTypedView_RequestSection_ParsesExpectedFields()
        {
            RequestSection original = new RequestSection
            {
                SectionX = 12,
                SectionY = 34
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            RequestSectionView view = packetView.AsRequestSectionView();

            Assert.Equal((ushort)12, view.SectionX);
            Assert.Equal((ushort)34, view.SectionY);
        }

        [Fact]
        public void GenericTileEntityItemSyncView_ParsesWeaponsRackTryPlacing()
        {
            WeaponsRackTryPlacing original = new WeaponsRackTryPlacing
            {
                TileX = 100,
                TileY = 101,
                ItemType = 2000,
                Prefix = 3,
                Stack = 2
            };

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            TileEntityItemSyncPacketView view = packetView.AsWeaponsRackTryPlacingView();

            Assert.Equal(PacketTypes.WeaponsRackTryPlacing, view.PacketType);
            Assert.Equal((short)100, view.TileX);
            Assert.Equal((short)101, view.TileY);
            Assert.Equal((short)2000, view.ItemType);
            Assert.Equal((byte)3, view.Prefix);
            Assert.Equal((short)2, view.Stack);
        }

        [Fact]
        public void GenericEmptyPayloadView_ParsesPing()
        {
            Ping original = new Ping();

            PacketView packetView = PacketViewParser.ParseExact(original.ToArray(includeHeader: true));
            EmptyPayloadPacketView view = packetView.AsPingView();

            Assert.Equal(PacketTypes.Ping, view.PacketType);
            Assert.Equal(PacketTypes.Ping, view.Header.PacketType);
        }

        [Fact]
        public void PacketViewParser_TryParse_ReturnsFalse_ForIncompleteBuffer()
        {
            ChatMessage original = new ChatMessage
            {
                PlayerId = 1,
                MessageColor = CreateColor(255, 100, 50),
                Message = "hello"
            };

            byte[] bytes = original.ToArray(includeHeader: true);
            ReadOnlySpan<byte> incomplete = bytes.AsSpan(0, bytes.Length - 1);

            bool result = PacketViewParser.TryParse(incomplete, out PacketView _, out int consumed);

            Assert.False(result);
            Assert.Equal(0, consumed);
        }

        [Fact]
        public void DeserializePayload_PlayerInfo_ParsesWithoutPacketHeader()
        {
            PlayerInfo original = new PlayerInfo
            {
                PlayerId = 2,
                Name = "PayloadOnly",
                Hair = 7,
                DifficultyFlags = PlayerDifficultyFlags.Creative
            };

            byte[] packetBytes = original.ToArray(includeHeader: true);
            ReadOnlyMemory<byte> payload = packetBytes.AsMemory(TerrariaPacket.PacketHeaderLength);

            TerrariaPacket parsedPacket = TerrariaPacket.DeserializePayload(PacketTypes.PlayerInfo, payload);
            PlayerInfo parsed = Assert.IsType<PlayerInfo>(parsedPacket);

            Assert.Equal(original.PlayerId, parsed.PlayerId);
            Assert.Equal(original.Name, parsed.Name);
            Assert.Equal(original.Hair, parsed.Hair);
            Assert.Equal(original.DifficultyFlags, parsed.DifficultyFlags);
        }

        [Fact]
        public void PacketViewParser_ParsePayload_UpdatePlayer_ParsesTypedViewWithoutPacketHeader()
        {
            PlayerUpdate original = new PlayerUpdate
            {
                PlayerId = 4,
                ControlFlags = UpdatePlayerControlFlags.ControlUseItem,
                MovementFlags = UpdatePlayerMovementFlags.HasVelocity,
                SelectedItem = 8,
                PositionX = 321.5f,
                PositionY = 654.25f,
                VelocityX = -1.25f,
                VelocityY = 2.75f
            };

            byte[] packetBytes = original.ToArray(includeHeader: true);
            PacketView packetView = PacketViewParser.ParsePayload(
                PacketTypes.PlayerUpdate,
                packetBytes,
                TerrariaPacket.PacketHeaderLength,
                packetBytes.Length - TerrariaPacket.PacketHeaderLength);

            PlayerUpdateView view = packetView.AsPlayerUpdateView();

            Assert.False(packetView.HasPacketSpan);
            Assert.Equal(PacketTypes.PlayerUpdate, view.Header.PacketType);
            Assert.Equal(original.PlayerId, view.PlayerId);
            Assert.Equal(original.PositionX, view.PositionX);
            Assert.Equal(original.PositionY, view.PositionY);
            Assert.Equal(original.VelocityX, view.VelocityX);
            Assert.Equal(original.VelocityY, view.VelocityY);
            Assert.Equal(packetBytes.Length - TerrariaPacket.PacketHeaderLength, view.SourceSpan.Length);

            bool threw = false;
            try
            {
                packetView.GetPacketSpan();
            }
            catch (InvalidDataException)
            {
                threw = true;
            }

            Assert.True(threw);
        }

        [Fact]
        public void PacketViewParser_TryParsePayload_ReturnsFalse_ForTooLargePayload()
        {
            byte[] payload = new byte[ushort.MaxValue];

            bool result = PacketViewParser.TryParsePayload(PacketTypes.Ping, payload, 0, payload.Length, out PacketView _);

            Assert.False(result);
        }

        [Fact]
        public void ProjectileNew_PayloadSerialization_RoundTripsWithoutHeader()
        {
            ProjectileNew original = new ProjectileNew
            {
                Identity = 42,
                PositionX = 14.5f,
                PositionY = 28.25f,
                VelocityX = 1.75f,
                VelocityY = -0.5f,
                Owner = 3,
                Type = 97,
                AI0 = 2.25f,
                Damage = 55
            };

            byte[] payload = original.ToPayloadArray();
            Span<byte> copiedPayload = stackalloc byte[payload.Length];

            original.CopyPayloadTo(copiedPayload);

            Assert.Equal(payload, copiedPayload.ToArray());

            ProjectileNew parsed = Assert.IsType<ProjectileNew>(
                TerrariaPacket.DeserializePayload(PacketTypes.ProjectileNew, payload));
            ProjectileNewView view = ProjectileNewView.FromPayload(payload);

            Assert.Equal(original.Identity, parsed.Identity);
            Assert.Equal(original.Identity, view.Identity);
            Assert.Equal(original.Type, view.Type);
            Assert.Equal(original.Damage, view.Damage);
        }

        [Fact]
        public void StringPacketView_FromPayload_ParsesWithoutPacketHeader()
        {
            HostToken original = new HostToken
            {
                Token = "payload-token"
            };

            byte[] packetBytes = original.ToArray(includeHeader: true);
            ReadOnlySpan<byte> payload = packetBytes.AsSpan(TerrariaPacket.PacketHeaderLength);

            StringPacketView view = StringPacketView.FromPayload(PacketTypes.HostToken, payload);

            Assert.Equal(PacketTypes.HostToken, view.PacketType);
            Assert.Equal("payload-token", view.GetValue());
            Assert.Equal(payload.Length, view.SourceSpan.Length);
        }

        private static ColorStruct CreateColor(byte r, byte g, byte b)
        {
            return new ColorStruct
            {
                R = r,
                G = g,
                B = b
            };
        }
    }
}


