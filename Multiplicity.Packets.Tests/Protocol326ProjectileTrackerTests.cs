using System;
using Multiplicity.Packets;
using Multiplicity.Packets.Models;
using Multiplicity.Packets.Views;
using Xunit;

namespace Multiplicity.Packets.Tests
{
    public class Protocol326ProjectileTrackerTests
    {
        [Fact]
        public void SyncProjectileTrackers_UsesPackedProjectileKeys()
        {
            SyncProjectileTrackers packet = new SyncProjectileTrackers
            {
                PlayerId = 9,
                PiggyBankProjectile = new TrackedProjectileReferenceData
                {
                    Key = new ProjectileKey(spawner: 3, index: 42, generation: 7),
                    ProjectileType = 525
                },
                VoidLensChestProjectile = new TrackedProjectileReferenceData
                {
                    Key = new ProjectileKey(spawner: 4, index: 100, generation: 12),
                    ProjectileType = 734
                }
            };

            byte[] payload = packet.ToPayloadArray();

            Assert.Equal(new byte[]
            {
                9,
                0x03, 0x2A, 0x1C, 0x00,
                0x0D, 0x02,
                0x04, 0x64, 0x30, 0x00,
                0xDE, 0x02
            }, payload);
            Assert.Equal(SyncProjectileTrackers.PayloadLength, payload.Length);
            Assert.Equal(13, payload.Length);

            SyncProjectileTrackers parsed = Assert.IsType<SyncProjectileTrackers>(
                TerrariaPacket.DeserializePayload(PacketTypes.SyncProjectileTrackers, payload));
            SyncProjectileTrackersView view = new SyncProjectileTrackersView(
                PacketView.FromPayload(PacketTypes.SyncProjectileTrackers, payload));

            Assert.Equal(new ProjectileKey(3, 42, 7), parsed.PiggyBankProjectile.Key);
            Assert.Equal((short)525, parsed.PiggyBankProjectile.ProjectileType);
            Assert.Equal(new ProjectileKey(4, 100, 12), parsed.VoidLensChestProjectile.Key);
            Assert.Equal((short)734, parsed.VoidLensChestProjectile.ProjectileType);
            Assert.Equal(parsed.PiggyBankProjectile.Key, view.PiggyBankKey);
            Assert.Equal(parsed.VoidLensChestProjectile.Key, view.VoidLensChestKey);
            Assert.Equal(payload, parsed.ToPayloadArray());
        }

        [Fact]
        public void SyncProjectileTrackers_EmptyReferencesStillUseSixBytesEach()
        {
            SyncProjectileTrackers packet = new SyncProjectileTrackers { PlayerId = 2 };

            byte[] payload = packet.ToPayloadArray();
            PacketPayloadMetadata metadata = TerrariaPacket.GetPayloadMetadata(PacketTypes.SyncProjectileTrackers);

            Assert.Equal(13, payload.Length);
            Assert.Equal(new byte[12], payload[1..]);
            Assert.True(metadata.HasFixedPayloadLength);
            Assert.Equal((short)13, metadata.FixedPayloadLength);
        }
    }
}
