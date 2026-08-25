using System;
using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class LandGolfBallInCup : TerrariaPacket
    {
        private const short PayloadLength = 9;

        public byte PlayerId { get; set; }

        public ushort CupX { get; set; }

        public ushort CupY { get; set; }

        public ushort Hits { get; set; }

        public ushort ProjectileType { get; set; }

        [Obsolete("Protocol 325 uses this field as hit count. Use Hits.")]
        public ushort ShotX
        {
            get => Hits;
            set => Hits = value;
        }

        [Obsolete("Protocol 325 uses this field as projectile type. Use ProjectileType.")]
        public ushort ShotY
        {
            get => ProjectileType;
            set => ProjectileType = value;
        }

        public LandGolfBallInCup()
            : base((byte)PacketTypes.LandGolfBallInCup)
        {
        }

        public LandGolfBallInCup(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            CupX = br.ReadUInt16();
            CupY = br.ReadUInt16();
            Hits = br.ReadUInt16();
            ProjectileType = br.ReadUInt16();
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

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(PlayerId);
                bw.Write(CupX);
                bw.Write(CupY);
                bw.Write(Hits);
                bw.Write(ProjectileType);
            }
        }
    }
}
