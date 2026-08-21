using System.IO;
using System.Runtime.CompilerServices;

namespace Multiplicity.Packets
{
    internal static class Protocol325Registration
    {
        [ModuleInitializer]
        internal static void Register()
        {
            TerrariaPacket.DeserializerMap.Remove(PacketTypes.SyncItemsWithShimmerDeprecated);
            TerrariaPacket.DeserializerMap.Remove(PacketTypes.SyncItemCannotBeTakenByEnemiesDeprecated);
            TerrariaPacket.DeserializerMap[PacketTypes.DamageNPCAck] = br => new DamageNPCAck(br);
        }
    }
}
