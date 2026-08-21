using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    public sealed class SyncItemCannotBeTakenByEnemies : WorldItemSyncPacketBase
    {
        public SyncItemCannotBeTakenByEnemies()
            : base((byte)PacketTypes.SyncItemCannotBeTakenByEnemiesDeprecated)
        {
        }

        public SyncItemCannotBeTakenByEnemies(BinaryReader br)
            : base(br)
        {
        }
    }
}
