using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SyncProjectileTrackers : TerrariaPacket
        {
            private const short BasePayloadLength = 1;
    
            public byte PlayerId { get; set; }
    
            public TrackedProjectileReferenceData PiggyBankProjectile { get; set; } = new TrackedProjectileReferenceData();
    
            public TrackedProjectileReferenceData VoidLensChestProjectile { get; set; } = new TrackedProjectileReferenceData();
    
            public SyncProjectileTrackers()
                : base((byte)PacketTypes.SyncProjectileTrackers)
            {
            }
    
            public SyncProjectileTrackers(BinaryReader br)
                : base(br)
            {
                PlayerId = br.ReadByte();
                PiggyBankProjectile = new TrackedProjectileReferenceData(br);
                VoidLensChestProjectile = new TrackedProjectileReferenceData(br);
            }
    
            public override short GetLength()
            {
                return (short)(BasePayloadLength +
                    (PiggyBankProjectile ?? new TrackedProjectileReferenceData()).GetLength() +
                    (VoidLensChestProjectile ?? new TrackedProjectileReferenceData()).GetLength());
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
                    (PiggyBankProjectile ?? new TrackedProjectileReferenceData()).ToStream(bw);
                    (VoidLensChestProjectile ?? new TrackedProjectileReferenceData()).ToStream(bw);
                }
            }
        }
}

