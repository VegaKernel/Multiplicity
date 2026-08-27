using System;
using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Packet 112 (SpecialFX). EffectType 1 carries TreeGrowFX data; effect type 2 carries fairy FX.
    /// The wire shape is always Byte + Int32 + Int32 + Byte + Int16 + Byte.
    /// </summary>
    public class TreeGrowFX : TerrariaPacket
    {
        private const short PayloadLength = 13;

        public byte EffectType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public byte Style { get; set; }
        public short Extra { get; set; }
        public bool ExtraFlag { get; set; }

        [Obsolete("Use EffectType. Packet 112 is the generic SpecialFX packet.")]
        public bool GrowEffect
        {
            get => EffectType != 0;
            set => EffectType = value ? (byte)1 : (byte)0;
        }

        [Obsolete("Use Style.")]
        public byte Height
        {
            get => Style;
            set => Style = value;
        }

        [Obsolete("Use Extra.")]
        public short TreeGore
        {
            get => Extra;
            set => Extra = value;
        }

        public TreeGrowFX()
            : base((byte)PacketTypes.TreeGrowFX)
        {
        }

        public TreeGrowFX(BinaryReader br)
            : base(br)
        {
            EffectType = br.ReadByte();
            X = br.ReadInt32();
            Y = br.ReadInt32();
            Style = br.ReadByte();
            Extra = br.ReadInt16();
            ExtraFlag = br.ReadByte() == 1;
        }

        public override short GetLength() => PayloadLength;

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(EffectType);
            bw.Write(X);
            bw.Write(Y);
            bw.Write(Style);
            bw.Write(Extra);
            bw.Write((byte)(ExtraFlag ? 1 : 0));
        }

        public override string ToString()
        {
            return $"[SpecialFX: EffectType = {EffectType} X = {X} Y = {Y} Style = {Style} Extra = {Extra} ExtraFlag = {ExtraFlag}]";
        }
    }
}
