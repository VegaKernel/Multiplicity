using System.IO;
using System.Text;

namespace Multiplicity.Packets
{
    public enum PlayerPlatform : byte
    {
        None, Stadia, Xbox, PlayStation, Editor, Nintendo, Steam, GameCenter
    }

    public class PlayerPlatformInfo : TerrariaPacket
    {
        public byte PlayerSlot { get; set; }
        public PlayerPlatform Platform { get; set; }
        public string PlatformUserId { get; set; } = string.Empty;
        public string PlatformName { get; set; } = string.Empty;
        public bool IsHost { get; set; }

        public PlayerPlatformInfo() : base((byte)PacketTypes.PlayerPlatformInfo) { }

        public PlayerPlatformInfo(BinaryReader reader) : base(reader)
        {
            PlayerSlot = reader.ReadByte();
            Platform = (PlayerPlatform)reader.ReadByte();
            PlatformUserId = reader.ReadString();
            if (Platform == PlayerPlatform.Nintendo)
                PlatformName = reader.ReadString();
            IsHost = reader.ReadBoolean();
        }

        public override short GetLength() => checked((short)(3
            + PacketSerializationHelper.GetSerializedStringLength(PlatformUserId)
            + (Platform == PlayerPlatform.Nintendo ? PacketSerializationHelper.GetSerializedStringLength(PlatformName) : 0)));

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
                base.ToStream(stream, includeHeader);

            using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
            writer.Write(PlayerSlot);
            writer.Write((byte)Platform);
            writer.Write(PlatformUserId ?? string.Empty);
            if (Platform == PlayerPlatform.Nintendo)
                writer.Write(PlatformName ?? string.Empty);
            writer.Write(IsHost);
        }
    }
}
