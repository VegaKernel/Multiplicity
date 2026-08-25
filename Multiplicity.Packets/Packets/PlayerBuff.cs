using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The PlayerBuff (0x32) packet.
    /// Protocol 325 serializes buff ids as UInt16 values. The number of serialized slots is derived
    /// from the packet payload length rather than hard-coded here.
    /// </summary>
    public class PlayerBuff : TerrariaPacket
    {
        public byte PlayerId { get; set; }

        public ushort[] BuffTypes { get; set; } = Array.Empty<ushort>();

        public ReadOnlyMemory<ushort> BuffTypesMemory => BuffTypes;

        [Obsolete("Protocol 325 uses UInt16 buff ids. Use BuffTypes instead. This compatibility property truncates ids above 255.")]
        public byte[] BuffType
        {
            get
            {
                ushort[] source = BuffTypes ?? Array.Empty<ushort>();
                byte[] result = new byte[source.Length];
                for (int i = 0; i < source.Length; i++)
                {
                    result[i] = unchecked((byte)source[i]);
                }

                return result;
            }
            set
            {
                byte[] source = value ?? Array.Empty<byte>();
                ushort[] result = new ushort[source.Length];
                for (int i = 0; i < source.Length; i++)
                {
                    result[i] = source[i];
                }

                BuffTypes = result;
            }
        }

        [Obsolete("Protocol 325 uses UInt16 buff ids. Use BuffTypesMemory instead. This compatibility property truncates ids above 255.")]
        public ReadOnlyMemory<byte> BuffTypeMemory
        {
            get => BuffType;
            set => BuffType = value.ToArray();
        }

        public PlayerBuff()
            : base((byte)PacketTypes.PlayerBuff)
        {
        }

        public PlayerBuff(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();

            long remaining = br.BaseStream.Length - br.BaseStream.Position;
            if (remaining < 0 || (remaining & 1L) != 0 || remaining / sizeof(ushort) > int.MaxValue)
            {
                throw new InvalidDataException($"Invalid PlayerBuff payload tail length {remaining}.");
            }

            int count = checked((int)(remaining / sizeof(ushort)));
            ushort[] buffs = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                buffs[i] = br.ReadUInt16();
            }

            BuffTypes = buffs;
        }

        public override string ToString()
        {
            return $"[PlayerBuff: PlayerId = {PlayerId} BuffCount = {BuffTypes?.Length ?? 0}]";
        }

        public override short GetLength()
        {
            int count = BuffTypes?.Length ?? 0;
            return checked((short)(sizeof(byte) + count * sizeof(ushort)));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter br = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                br.Write(PlayerId);
                ushort[] buffs = BuffTypes ?? Array.Empty<ushort>();
                for (int i = 0; i < buffs.Length; i++)
                {
                    br.Write(buffs[i]);
                }
            }
        }
    }
}
