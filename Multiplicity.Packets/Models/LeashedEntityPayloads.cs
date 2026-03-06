using System;
using System.IO;
using System.Numerics;
using Multiplicity.Packets.Views;

namespace Multiplicity.Packets.Models
{
    public abstract class LeashedEntityPayload
    {
        public byte[] ToPayloadArray(bool fullSync)
        {
            using MemoryStream stream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(stream);
            Write(writer, fullSync);
            return stream.ToArray();
        }

        internal abstract short GetPayloadLength(bool fullSync);

        internal abstract void Write(BinaryWriter writer, bool fullSync);
    }

    public sealed class RawLeashedEntityPayload : LeashedEntityPayload
    {
        private byte[] _payload = Array.Empty<byte>();

        public byte[] Payload
        {
            get => _payload;
            set => _payload = value ?? Array.Empty<byte>();
        }

        internal override short GetPayloadLength(bool fullSync)
        {
            return checked((short)_payload.Length);
        }

        internal override void Write(BinaryWriter writer, bool fullSync)
        {
            if (_payload.Length > 0)
            {
                writer.Write(_payload);
            }
        }
    }

    public class LeashedKitePayload : LeashedEntityPayload
    {
        public int ProjectileType { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public byte RotationByte { get; set; }

        public float WindTarget { get; set; }

        public float CloudAlpha { get; set; }

        public float TimeCounter { get; set; }

        public float RotationRadians
        {
            get => RotationByte * (MathF.PI * 2f / 256f);
            set => RotationByte = unchecked((byte)Math.Clamp((int)MathF.Round(value * 256f / (MathF.PI * 2f)), 0, byte.MaxValue));
        }

        internal override short GetPayloadLength(bool fullSync)
        {
            int length = sizeof(float) * 2 + sizeof(uint) + sizeof(byte) + sizeof(float) * 3;
            if (fullSync)
            {
                length += PacketSerializationHelper.Get7BitEncodedIntLength(ProjectileType);
            }

            return checked((short)length);
        }

        internal override void Write(BinaryWriter writer, bool fullSync)
        {
            if (fullSync)
            {
                writer.Write7BitEncodedInt(ProjectileType);
            }

            writer.WriteVector2(Position);
            writer.WritePackedVector2(Velocity);
            writer.Write(RotationByte);
            writer.Write(WindTarget);
            writer.Write(CloudAlpha);
            writer.Write(TimeCounter);
        }
    }

    public class LeashedCritterPayload : LeashedEntityPayload
    {
        public int NpcType { get; set; }

        public Vector2 Size { get; set; }

        public Vector2 PositionOffset { get; set; }

        public bool FacingRight { get; set; }

        public uint RandomState { get; set; }

        public short WaitTime { get; set; }

        public byte State { get; set; }

        public sbyte TargetOffsetX { get; set; }

        public sbyte TargetOffsetY { get; set; }

        public int Direction => FacingRight ? 1 : -1;

        internal override short GetPayloadLength(bool fullSync)
        {
            int length = sizeof(uint) + sizeof(bool) + sizeof(uint) + sizeof(short) + sizeof(byte) + sizeof(sbyte) + sizeof(sbyte);
            if (fullSync)
            {
                length += PacketSerializationHelper.Get7BitEncodedIntLength(NpcType);
                length += sizeof(float) * 2;
            }

            return checked((short)length);
        }

        internal override void Write(BinaryWriter writer, bool fullSync)
        {
            if (fullSync)
            {
                writer.Write7BitEncodedInt(NpcType);
                writer.WriteVector2(Size);
            }

            writer.WritePackedVector2(PositionOffset);
            writer.Write(FacingRight);
            writer.Write(RandomState);
            writer.Write(WaitTime);
            writer.Write(State);
            writer.Write(TargetOffsetX);
            writer.Write(TargetOffsetY);
        }
    }

    public sealed class NormalButterflyLeashedCritterPayload : LeashedCritterPayload
    {
        public byte Variant { get; set; }

        internal override short GetPayloadLength(bool fullSync)
        {
            int length = base.GetPayloadLength(fullSync);
            if (fullSync)
            {
                length += sizeof(byte);
            }

            return checked((short)length);
        }

        internal override void Write(BinaryWriter writer, bool fullSync)
        {
            base.Write(writer, fullSync);
            if (fullSync)
            {
                writer.Write(Variant);
            }
        }
    }

    public sealed class ShimmerFlyLeashedCritterPayload : LeashedCritterPayload
    {
        public byte OldPositionsLength { get; set; }

        internal override short GetPayloadLength(bool fullSync)
        {
            int length = base.GetPayloadLength(fullSync);
            if (fullSync)
            {
                length += sizeof(byte);
            }

            return checked((short)length);
        }

        internal override void Write(BinaryWriter writer, bool fullSync)
        {
            base.Write(writer, fullSync);
            if (fullSync)
            {
                writer.Write(OldPositionsLength);
            }
        }
    }

    internal static class LeashedEntityPayloadSerializer
    {
        public static LeashedEntityPayload Deserialize(LeashedEntityType entityKind, bool fullSync, ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            return entityKind switch
            {
                LeashedEntityType.Kite => DeserializeKite(ref reader, fullSync),
                LeashedEntityType.NormalButterfly => DeserializeNormalButterfly(ref reader, fullSync),
                LeashedEntityType.ShimmerFly => DeserializeShimmerFly(ref reader, fullSync),
                _ when IsCritter(entityKind) => DeserializeCritter(ref reader, fullSync),
                _ => new RawLeashedEntityPayload { Payload = payload.ToArray() }
            };
        }

        private static bool IsCritter(LeashedEntityType entityKind)
        {
            return entityKind is >= LeashedEntityType.WalkerCritter and <= LeashedEntityType.WaterStrider;
        }

        private static LeashedEntityPayload DeserializeKite(ref PacketReader reader, bool fullSync)
        {
            LeashedKitePayload payload = new LeashedKitePayload();
            if (fullSync)
            {
                payload.ProjectileType = reader.Read7BitEncodedInt();
            }

            payload.Position = reader.ReadVector2();
            payload.Velocity = reader.ReadPackedVector2();
            payload.RotationByte = reader.ReadByte();
            payload.WindTarget = reader.ReadSingle();
            payload.CloudAlpha = reader.ReadSingle();
            payload.TimeCounter = reader.ReadSingle();
            reader.EnsureEnd();
            return payload;
        }

        private static LeashedEntityPayload DeserializeCritter(ref PacketReader reader, bool fullSync)
        {
            LeashedCritterPayload payload = new LeashedCritterPayload();
            if (fullSync)
            {
                payload.NpcType = reader.Read7BitEncodedInt();
                payload.Size = reader.ReadVector2();
            }

            payload.PositionOffset = reader.ReadPackedVector2();
            payload.FacingRight = reader.ReadBoolean();
            payload.RandomState = reader.ReadUInt32();
            payload.WaitTime = reader.ReadInt16();
            payload.State = reader.ReadByte();
            payload.TargetOffsetX = reader.ReadSByte();
            payload.TargetOffsetY = reader.ReadSByte();
            reader.EnsureEnd();
            return payload;
        }

        private static LeashedEntityPayload DeserializeNormalButterfly(ref PacketReader reader, bool fullSync)
        {
            NormalButterflyLeashedCritterPayload payload = (NormalButterflyLeashedCritterPayload)DeserializeCritterCore(ref reader, fullSync, new NormalButterflyLeashedCritterPayload());
            if (fullSync)
            {
                payload.Variant = reader.ReadByte();
            }

            reader.EnsureEnd();
            return payload;
        }

        private static LeashedEntityPayload DeserializeShimmerFly(ref PacketReader reader, bool fullSync)
        {
            ShimmerFlyLeashedCritterPayload payload = (ShimmerFlyLeashedCritterPayload)DeserializeCritterCore(ref reader, fullSync, new ShimmerFlyLeashedCritterPayload());
            if (fullSync)
            {
                payload.OldPositionsLength = reader.ReadByte();
            }

            reader.EnsureEnd();
            return payload;
        }

        private static LeashedCritterPayload DeserializeCritterCore<TPayload>(ref PacketReader reader, bool fullSync, TPayload payload)
            where TPayload : LeashedCritterPayload
        {
            if (fullSync)
            {
                payload.NpcType = reader.Read7BitEncodedInt();
                payload.Size = reader.ReadVector2();
            }

            payload.PositionOffset = reader.ReadPackedVector2();
            payload.FacingRight = reader.ReadBoolean();
            payload.RandomState = reader.ReadUInt32();
            payload.WaitTime = reader.ReadInt16();
            payload.State = reader.ReadByte();
            payload.TargetOffsetX = reader.ReadSByte();
            payload.TargetOffsetY = reader.ReadSByte();
            return payload;
        }
    }
}
