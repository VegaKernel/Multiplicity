using System;
using System.IO;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    [CollectionDefinition("Deserializer map", DisableParallelization = true)]
    public sealed class DeserializerMapCollection { }

    [Collection("Deserializer map")]
    public sealed class PlayerUpdateViewParserTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Standard_view_parser_matches_model_for_every_optional_field_combination(bool trailingBytes)
        {
            for (int flags = 0; flags < 16; flags++)
            {
                var original = new PlayerUpdate
                {
                    PlayerId = 7,
                    ControlFlags = UpdatePlayerControlFlags.ControlUseItem,
                    SelectedItem = 1,
                    PositionX = 320,
                    PositionY = 480,
                    VelocityX = 1.25f,
                    VelocityY = -0.5f,
                    MountType = 1,
                    PotionOfReturnOriginalPositionX = 300,
                    PotionOfReturnOriginalPositionY = 450,
                    PotionOfReturnHomePositionX = 100,
                    PotionOfReturnHomePositionY = 150,
                    CameraTargetX = 320,
                    CameraTargetY = 480,
                    MovementFlags = ((flags & 1) != 0 ? UpdatePlayerMovementFlags.HasVelocity : 0) |
                        ((flags & 2) != 0 ? UpdatePlayerMovementFlags.HasMount : 0),
                    MiscFlags1 = (flags & 4) != 0 ? UpdatePlayerMiscFlags1.HasPotionOfReturnPositions : 0,
                    MiscFlags2 = (flags & 8) != 0 ? UpdatePlayerMiscFlags2.HasCameraTarget : 0
                };

                byte[] payload = original.ToArray(includeHeader: false);
                var window = new byte[payload.Length + (trailingBytes ? 4 : 0)];
                payload.CopyTo(window, 0);
                using var reader = new PacketBinaryReader(PacketTypes.PlayerUpdate, window);
                var model = Assert.IsType<PlayerUpdate>(TerrariaPacket.Deserialize(reader, (byte)PacketTypes.PlayerUpdate));
                int modelConsumed = (int)reader.BaseStream.Position - TerrariaPacket.PacketHeaderLength;
                Assert.True(PacketViewParser.TryParsePayload(PacketTypes.PlayerUpdate, window.AsMemory(),
                    out var packet, out int consumed));
                Assert.Equal(modelConsumed, consumed);
                Assert.True(payload.AsSpan().SequenceEqual(packet.PayloadSpan));
                var view = packet.AsPlayerUpdateView();
                Assert.Equal(model.PlayerId, view.PlayerId);
                Assert.Equal(model.ControlFlags, view.ControlFlags);
                Assert.Equal(model.MovementFlags, view.MovementFlags);
                Assert.Equal(model.MiscFlags1, view.MiscFlags1);
                Assert.Equal(model.MiscFlags2, view.MiscFlags2);
                Assert.Equal(model.SelectedItem, view.SelectedItem);
                Assert.Equal(model.PositionX, view.PositionX);
                Assert.Equal(model.PositionY, view.PositionY);
                Assert.Equal(model.VelocityX, view.VelocityX);
                Assert.Equal(model.VelocityY, view.VelocityY);
                Assert.Equal(model.MountType, view.MountType);
                Assert.Equal(model.PotionOfReturnOriginalPositionX, view.PotionOfReturnOriginalPositionX);
                Assert.Equal(model.PotionOfReturnOriginalPositionY, view.PotionOfReturnOriginalPositionY);
                Assert.Equal(model.PotionOfReturnHomePositionX, view.PotionOfReturnHomePositionX);
                Assert.Equal(model.PotionOfReturnHomePositionY, view.PotionOfReturnHomePositionY);
                Assert.Equal(model.CameraTargetX, view.CameraTargetX);
                Assert.Equal(model.CameraTargetY, view.CameraTargetY);
                Assert.Equal(!trailingBytes, PacketViewParser.TryParsePayload(PacketTypes.PlayerUpdate, window.AsSpan(), out _));
            }
        }

        [Fact]
        public void Window_length_bound_matches_general_reader()
        {
            byte[] payload = new PlayerUpdate { PlayerId = 7 }.ToArray(includeHeader: false);
            var maximum = new byte[short.MaxValue - TerrariaPacket.PacketHeaderLength];
            payload.CopyTo(maximum, 0);
            Assert.True(PacketViewParser.TryParsePayload(PacketTypes.PlayerUpdate, maximum.AsMemory(), out _, out int consumed));
            Assert.Equal(payload.Length, consumed);
            var beyondMaximum = new byte[maximum.Length + 1];
            payload.CopyTo(beyondMaximum, 0);
            Assert.False(PacketViewParser.TryParsePayload(PacketTypes.PlayerUpdate, beyondMaximum.AsMemory(), out _, out _));
        }

        [Fact]
        public void Incomplete_serialized_update_is_rejected()
        {
            var update = new PlayerUpdate
            {
                PlayerId = 7,
                MovementFlags = UpdatePlayerMovementFlags.HasVelocity,
                VelocityX = 1.25f,
                VelocityY = -0.5f
            };
            byte[] payload = update.ToArray(includeHeader: false);
            Assert.False(PacketViewParser.TryParsePayload(PacketTypes.PlayerUpdate,
                payload.AsMemory(0, payload.Length - 1), out _, out int consumed));
            Assert.Equal(0, consumed);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Replaced_deserializer_keeps_general_parser(bool reject)
        {
            Func<BinaryReader, TerrariaPacket> original = TerrariaPacket.DeserializerMap[PacketTypes.PlayerUpdate];
            bool called = false;
            try
            {
                TerrariaPacket.DeserializerMap[PacketTypes.PlayerUpdate] = reader =>
                {
                    called = true;
                    if (reject)
                    {
                        throw new InvalidDataException("Custom validation rejected this packet.");
                    }

                    return original(reader);
                };
                byte[] payload = new PlayerUpdate { PlayerId = 7 }.ToArray(includeHeader: false);
                Assert.Equal(!reject, PacketViewParser.TryParsePayload(PacketTypes.PlayerUpdate, payload.AsMemory(), out _, out int consumed));
                Assert.Equal(reject ? 0 : payload.Length, consumed);
                Assert.True(called);
            }
            finally
            {
                TerrariaPacket.DeserializerMap[PacketTypes.PlayerUpdate] = original;
            }
        }
    }
}
