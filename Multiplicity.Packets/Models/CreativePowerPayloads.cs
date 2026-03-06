using System;
using System.IO;
using Multiplicity.Packets.Views;

namespace Multiplicity.Packets.Models
{
    public abstract class CreativePowerPayload
    {
        public abstract CreativePowerPayloadKind Kind { get; }

        public byte[] ToPayloadArray()
        {
            using MemoryStream stream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(stream);
            Write(writer);
            return stream.ToArray();
        }

        internal abstract short GetPayloadLength();

        internal abstract void Write(BinaryWriter writer);
    }

    public sealed class RawCreativePowerPayload : CreativePowerPayload
    {
        private byte[] _payload = Array.Empty<byte>();

        public override CreativePowerPayloadKind Kind => CreativePowerPayloadKind.Raw;

        public byte[] Payload
        {
            get => _payload;
            set => _payload = value ?? Array.Empty<byte>();
        }

        internal override short GetPayloadLength() => checked((short)_payload.Length);

        internal override void Write(BinaryWriter writer)
        {
            if (_payload.Length > 0)
            {
                writer.Write(_payload);
            }
        }
    }

    public sealed class CreativePowerSharedButtonPayload : CreativePowerPayload
    {
        public override CreativePowerPayloadKind Kind => CreativePowerPayloadKind.SharedButton;

        internal override short GetPayloadLength() => 0;

        internal override void Write(BinaryWriter writer)
        {
        }
    }

    public sealed class CreativePowerSharedTogglePayload : CreativePowerPayload
    {
        public override CreativePowerPayloadKind Kind => CreativePowerPayloadKind.SharedToggle;

        public bool Enabled { get; set; }

        internal override short GetPayloadLength() => sizeof(bool);

        internal override void Write(BinaryWriter writer)
        {
            writer.Write(Enabled);
        }
    }

    public sealed class CreativePowerSharedSliderPayload : CreativePowerPayload
    {
        public override CreativePowerPayloadKind Kind => CreativePowerPayloadKind.SharedSlider;

        public float Value { get; set; }

        internal override short GetPayloadLength() => sizeof(float);

        internal override void Write(BinaryWriter writer)
        {
            writer.Write(Value);
        }
    }

    public sealed class CreativePowerPerPlayerSliderPayload : CreativePowerPayload
    {
        public override CreativePowerPayloadKind Kind => CreativePowerPayloadKind.PerPlayerSlider;

        public byte PlayerId { get; set; }

        public float Value { get; set; }

        internal override short GetPayloadLength() => checked((short)(sizeof(byte) + sizeof(float)));

        internal override void Write(BinaryWriter writer)
        {
            writer.Write(PlayerId);
            writer.Write(Value);
        }
    }

    public sealed class CreativePowerPerPlayerTogglePayload : CreativePowerPayload
    {
        public const int MaxPlayerCount = 255;
        public const int SyncEveryoneBitsetByteCount = 32;

        private byte[] _playerStateBytes = new byte[SyncEveryoneBitsetByteCount];

        public override CreativePowerPayloadKind Kind => CreativePowerPayloadKind.PerPlayerToggle;

        public CreativePowerPerPlayerToggleMessageType MessageType { get; set; }

        public byte PlayerId { get; set; }

        public bool Enabled { get; set; }

        public byte[] PlayerStateBytes
        {
            get => _playerStateBytes;
            set => _playerStateBytes = PacketSerializationHelper.NormalizeFixedBuffer(value, SyncEveryoneBitsetByteCount);
        }

        public bool GetEnabledState(int playerId)
        {
            if ((uint)playerId >= MaxPlayerCount)
            {
                throw new ArgumentOutOfRangeException(nameof(playerId));
            }

            int byteIndex = playerId / 8;
            int bitIndex = playerId % 8;
            return (_playerStateBytes[byteIndex] & (1 << bitIndex)) != 0;
        }

        internal override short GetPayloadLength()
        {
            return MessageType switch
            {
                CreativePowerPerPlayerToggleMessageType.SyncEveryone => checked((short)(sizeof(byte) + SyncEveryoneBitsetByteCount)),
                CreativePowerPerPlayerToggleMessageType.SyncOnePlayer => checked((short)(sizeof(byte) + sizeof(byte) + sizeof(bool))),
                _ => sizeof(byte)
            };
        }

        internal override void Write(BinaryWriter writer)
        {
            writer.Write((byte)MessageType);
            switch (MessageType)
            {
                case CreativePowerPerPlayerToggleMessageType.SyncEveryone:
                    writer.Write(_playerStateBytes);
                    break;
                case CreativePowerPerPlayerToggleMessageType.SyncOnePlayer:
                    writer.Write(PlayerId);
                    writer.Write(Enabled);
                    break;
            }
        }
    }

    internal static class CreativePowerPayloadSerializer
    {
        public static CreativePowerPayload Deserialize(CreativePowerId powerType, ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            return GetPayloadKind(powerType) switch
            {
                CreativePowerPayloadKind.SharedButton => DeserializeSharedButton(ref reader),
                CreativePowerPayloadKind.SharedToggle => DeserializeSharedToggle(ref reader),
                CreativePowerPayloadKind.SharedSlider => DeserializeSharedSlider(ref reader),
                CreativePowerPayloadKind.PerPlayerSlider => DeserializePerPlayerSlider(ref reader),
                CreativePowerPayloadKind.PerPlayerToggle => DeserializePerPlayerToggle(ref reader),
                _ => new RawCreativePowerPayload { Payload = payload.ToArray() }
            };
        }

        public static CreativePowerPayloadKind GetPayloadKind(CreativePowerId powerType)
        {
            return powerType switch
            {
                CreativePowerId.StartDayImmediately => CreativePowerPayloadKind.SharedButton,
                CreativePowerId.StartNoonImmediately => CreativePowerPayloadKind.SharedButton,
                CreativePowerId.StartNightImmediately => CreativePowerPayloadKind.SharedButton,
                CreativePowerId.StartMidnightImmediately => CreativePowerPayloadKind.SharedButton,
                CreativePowerId.FreezeTime => CreativePowerPayloadKind.SharedToggle,
                CreativePowerId.FreezeRainPower => CreativePowerPayloadKind.SharedToggle,
                CreativePowerId.FreezeWindDirectionAndStrength => CreativePowerPayloadKind.SharedToggle,
                CreativePowerId.StopBiomeSpreadPower => CreativePowerPayloadKind.SharedToggle,
                CreativePowerId.ModifyWindDirectionAndStrength => CreativePowerPayloadKind.SharedSlider,
                CreativePowerId.ModifyRainPower => CreativePowerPayloadKind.SharedSlider,
                CreativePowerId.ModifyTimeRate => CreativePowerPayloadKind.SharedSlider,
                CreativePowerId.DifficultySliderPower => CreativePowerPayloadKind.SharedSlider,
                CreativePowerId.SpawnRateSliderPerPlayerPower => CreativePowerPayloadKind.PerPlayerSlider,
                CreativePowerId.GodmodePower => CreativePowerPayloadKind.PerPlayerToggle,
                CreativePowerId.FarPlacementRangePower => CreativePowerPayloadKind.PerPlayerToggle,
                _ => CreativePowerPayloadKind.Raw
            };
        }

        private static CreativePowerPayload DeserializeSharedButton(ref PacketReader reader)
        {
            reader.EnsureEnd();
            return new CreativePowerSharedButtonPayload();
        }

        private static CreativePowerPayload DeserializeSharedToggle(ref PacketReader reader)
        {
            bool enabled = reader.ReadBoolean();
            reader.EnsureEnd();
            return new CreativePowerSharedTogglePayload
            {
                Enabled = enabled
            };
        }

        private static CreativePowerPayload DeserializeSharedSlider(ref PacketReader reader)
        {
            float value = reader.ReadSingle();
            reader.EnsureEnd();
            return new CreativePowerSharedSliderPayload
            {
                Value = value
            };
        }

        private static CreativePowerPayload DeserializePerPlayerSlider(ref PacketReader reader)
        {
            byte playerId = reader.ReadByte();
            float value = reader.ReadSingle();
            reader.EnsureEnd();
            return new CreativePowerPerPlayerSliderPayload
            {
                PlayerId = playerId,
                Value = value
            };
        }

        private static CreativePowerPayload DeserializePerPlayerToggle(ref PacketReader reader)
        {
            CreativePowerPerPlayerToggleMessageType messageType = (CreativePowerPerPlayerToggleMessageType)reader.ReadByte();
            CreativePowerPerPlayerTogglePayload payload = new CreativePowerPerPlayerTogglePayload
            {
                MessageType = messageType
            };

            switch (messageType)
            {
                case CreativePowerPerPlayerToggleMessageType.SyncEveryone:
                    payload.PlayerStateBytes = reader.ReadSpan(CreativePowerPerPlayerTogglePayload.SyncEveryoneBitsetByteCount).ToArray();
                    break;
                case CreativePowerPerPlayerToggleMessageType.SyncOnePlayer:
                    payload.PlayerId = reader.ReadByte();
                    payload.Enabled = reader.ReadBoolean();
                    break;
                default:
                    return new RawCreativePowerPayload { Payload = payload.ToPayloadArray() };
            }

            reader.EnsureEnd();
            return payload;
        }
    }
}
