using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The Zones (0x24) packet.
    /// </summary>
    public class Zones : TerrariaPacket
    {
        public const short PayloadLength = 7;

        public byte PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the Zone1 flags.
        /// </summary>
        public byte Zone1 { get; set; }

        /// <summary>
        /// Gets or sets the Zone2 flags.
        /// </summary>
        public byte Zone2 { get; set; }

        /// <summary>
        /// Gets or sets the Zone3 flags.
        /// </summary>
        public byte Zone3 { get; set; }

        /// <summary>
        /// Gets or sets the Zone4 flags.
        /// </summary>
        public byte Zone4 { get; set; }

        /// <summary>
        /// Gets or sets the Zone5 flags introduced by the current Terraria protocol.
        /// </summary>
        public byte Zone5 { get; set; }

        /// <summary>
        /// Gets or sets the number of nearby town NPCs reported by the client.
        /// </summary>
        public byte TownNpcCount { get; set; }

        public Zones()
            : base((byte)PacketTypes.Zones)
        {
        }

        public Zones(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            Zone1 = br.ReadByte();
            Zone2 = br.ReadByte();
            Zone3 = br.ReadByte();
            Zone4 = br.ReadByte();
            Zone5 = br.ReadByte();
            TownNpcCount = br.ReadByte();
        }

        public override string ToString()
        {
            return $"[Zones: PlayerId = {PlayerId} Zone1 = {Zone1} Zone2 = {Zone2} Zone3 = {Zone3} Zone4 = {Zone4} Zone5 = {Zone5} TownNpcCount = {TownNpcCount}]";
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
                br.Write(PlayerId);
                br.Write(Zone1);
                br.Write(Zone2);
                br.Write(Zone3);
                br.Write(Zone4);
                br.Write(Zone5);
                br.Write(TownNpcCount);
            }
        }
    }
}
