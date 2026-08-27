using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The SyncExtraValue (0x5C) packet.
    /// </summary>
    public class SyncExtraValue : TerrariaPacket
    {
        public const short PayloadLength = 14;

        public short NpcIndex { get; set; }

        /// <summary>
        /// Gets or sets the integer extra-value delta carried by Terraria on the wire.
        /// </summary>
        public int ExtraValue { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public SyncExtraValue()
            : base((byte)PacketTypes.SyncExtraValue)
        {
        }

        public SyncExtraValue(BinaryReader br)
            : base(br)
        {
            NpcIndex = br.ReadInt16();
            ExtraValue = br.ReadInt32();
            X = br.ReadSingle();
            Y = br.ReadSingle();
        }

        public override string ToString()
        {
            return $"[SyncExtraValue: NpcIndex = {NpcIndex} ExtraValue = {ExtraValue} X = {X} Y = {Y}]";
        }

        public override short GetLength()
        {
            return PayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter br = new BinaryWriter(stream, new System.Text.UTF8Encoding(), leaveOpen: true))
            {
                br.Write(NpcIndex);
                br.Write(ExtraValue);
                br.Write(X);
                br.Write(Y);
            }
        }
    }
}
