using System.IO;
using Multiplicity.Packets.Extensions.Dimensions;
using Multiplicity.Packets.Extensions.Mobile;

namespace Multiplicity.Packets.Extensions
{
    /// <summary>
    /// Predefined opt-in extension registries. Vanilla decoding remains the default.
    /// </summary>
    public static class PacketExtensions
    {
        public static PacketExtensionRegistry Dimensions { get; } =
            PacketExtensionRegistry.CreateBuilder()
                .Add(DimensionsMessage.PacketId, TryDecodeDimensions)
                .Build();

        public static PacketExtensionRegistry Mobile { get; } =
            PacketExtensionRegistry.CreateBuilder()
                .Add(ServerInfo.PacketId, DecodeServerInfo)
                .Add(PlayerPlatformInfo.PacketId, DecodePlayerPlatformInfo)
                .Build();

        public static PacketExtensionRegistry Common { get; } =
            PacketExtensionRegistry.CreateBuilder()
                .Add(DimensionsMessage.PacketId, TryDecodeDimensions)
                .Add(ServerInfo.PacketId, DecodeServerInfo)
                .Add(PlayerPlatformInfo.PacketId, DecodePlayerPlatformInfo)
                .Build();

        private static TerrariaPacket TryDecodeDimensions(BinaryReader reader)
        {
            // A bare packet 67 is still the vanilla Placeholder. Dimensions claims the id only
            // when there is enough payload for Int16 messageType plus a BinaryWriter string length byte.
            if (reader.BaseStream.Length - reader.BaseStream.Position <= TerrariaPacket.PacketHeaderLength)
            {
                return null;
            }

            return new DimensionsMessage(reader);
        }

        private static TerrariaPacket DecodeServerInfo(BinaryReader reader) => new ServerInfo(reader);

        private static TerrariaPacket DecodePlayerPlatformInfo(BinaryReader reader) => new PlayerPlatformInfo(reader);
    }
}
