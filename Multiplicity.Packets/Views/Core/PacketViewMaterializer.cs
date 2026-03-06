using System;

namespace Multiplicity.Packets.Views
{
    internal static class PacketViewMaterializer
    {
        public static TPacket Materialize<TPacket>(PacketView packet, PacketTypes expectedType)
            where TPacket : TerrariaPacket
        {
            packet.EnsureType(expectedType);
            return (TPacket)TerrariaPacket.DeserializePayload(expectedType, packet.PayloadSpan.ToArray());
        }
    }
}
