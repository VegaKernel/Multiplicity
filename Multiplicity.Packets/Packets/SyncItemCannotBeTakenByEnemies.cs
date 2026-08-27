using System;
using System.IO;

namespace Multiplicity.Packets
{
    [PacketRegistrationIgnore]
    public sealed class SyncItemCannotBeTakenByEnemies : WorldItemSyncPacketBase
    {
        public SyncItemCannotBeTakenByEnemies()
            : base((byte)PacketTypes.SyncItemCannotBeTakenByEnemiesDeprecated)
        {
            throw new NotSupportedException("Packet 148 is deprecated and cannot be sent in Terraria protocol 325.");
        }

        public SyncItemCannotBeTakenByEnemies(BinaryReader br)
            : base(br)
        {
            throw new InvalidDataException("Packet 148 is deprecated in Terraria protocol 325.");
        }
    }
}
