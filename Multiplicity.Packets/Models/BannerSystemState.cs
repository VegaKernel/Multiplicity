using System;
using System.IO;
using Multiplicity.Packets.Views;

namespace Multiplicity.Packets.Models
{
    public sealed class BannerSystemState
    {
        public int[] KillCounts { get; set; } = Array.Empty<int>();

        public ushort[] ClaimableBanners { get; set; } = Array.Empty<ushort>();

        public BannerSystemState()
        {
        }

        public BannerSystemState(BinaryReader reader)
        {
            short killCountLength = reader.ReadInt16();
            KillCounts = new int[killCountLength];
            for (int i = 0; i < KillCounts.Length; i++)
            {
                KillCounts[i] = reader.ReadInt32();
            }

            short claimableLength = reader.ReadInt16();
            ClaimableBanners = new ushort[claimableLength];
            for (int i = 0; i < ClaimableBanners.Length; i++)
            {
                ClaimableBanners[i] = reader.ReadUInt16();
            }
        }

        public static BannerSystemState Parse(ReadOnlySpan<byte> payload)
        {
            PacketReader reader = new PacketReader(payload);
            short killCountLength = reader.ReadInt16();
            int[] killCounts = new int[killCountLength];
            for (int i = 0; i < killCounts.Length; i++)
            {
                killCounts[i] = reader.ReadInt32();
            }

            short claimableLength = reader.ReadInt16();
            ushort[] claimableBanners = new ushort[claimableLength];
            for (int i = 0; i < claimableBanners.Length; i++)
            {
                claimableBanners[i] = reader.ReadUInt16();
            }

            reader.EnsureEnd();

            return new BannerSystemState
            {
                KillCounts = killCounts,
                ClaimableBanners = claimableBanners
            };
        }

        public short GetPayloadLength()
        {
            return checked((short)(
                sizeof(short) +
                (KillCounts?.Length ?? 0) * sizeof(int) +
                sizeof(short) +
                (ClaimableBanners?.Length ?? 0) * sizeof(ushort)));
        }

        public byte[] ToPayloadArray()
        {
            using MemoryStream stream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(stream);
            Write(writer);
            return stream.ToArray();
        }

        public void Write(BinaryWriter writer)
        {
            int[] killCounts = KillCounts ?? Array.Empty<int>();
            ushort[] claimableBanners = ClaimableBanners ?? Array.Empty<ushort>();

            writer.Write(checked((short)killCounts.Length));
            for (int i = 0; i < killCounts.Length; i++)
            {
                writer.Write(killCounts[i]);
            }

            writer.Write(checked((short)claimableBanners.Length));
            for (int i = 0; i < claimableBanners.Length; i++)
            {
                writer.Write(claimableBanners[i]);
            }
        }
    }
}
