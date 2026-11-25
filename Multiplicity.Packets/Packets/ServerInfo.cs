using System.IO;
using System.Text;

namespace Multiplicity.Packets
{
    public class ServerInfo : TerrariaPacket
    {
        public bool IsRequest { get; set; } = true;
        public int ListenPort { get; set; }
        public string WorldName { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public ushort WorldWidth { get; set; }
        public bool HasCrimson { get; set; }
        public int GameMode { get; set; }
        public byte MaxPlayers { get; set; }
        public byte ActivePlayers { get; set; }
        public string Reserved { get; set; } = string.Empty;
        public bool HasRemoteAddress { get; set; }
        public string RemoteAddress { get; set; } = string.Empty;
        public int RemotePort { get; set; }

        public ServerInfo() : base((byte)PacketTypes.ServerInfo) { }

        public ServerInfo(BinaryReader reader) : base(reader)
        {
            IsRequest = reader.BaseStream.Position == reader.BaseStream.Length;
            if (IsRequest)
                return;

            ListenPort = reader.ReadInt32();
            WorldName = reader.ReadString();
            HostName = reader.ReadString();
            WorldWidth = reader.ReadUInt16();
            HasCrimson = reader.ReadBoolean();
            GameMode = reader.ReadInt32();
            MaxPlayers = reader.ReadByte();
            ActivePlayers = reader.ReadByte();
            Reserved = reader.ReadString();
            HasRemoteAddress = reader.ReadBoolean();
            if (HasRemoteAddress)
            {
                RemoteAddress = reader.ReadString();
                RemotePort = reader.ReadInt32();
            }
        }

        public override short GetLength() => IsRequest ? (short)0 : checked((short)(14
            + PacketSerializationHelper.GetSerializedStringLength(WorldName)
            + PacketSerializationHelper.GetSerializedStringLength(HostName)
            + PacketSerializationHelper.GetSerializedStringLength(Reserved)
            + (HasRemoteAddress ? PacketSerializationHelper.GetSerializedStringLength(RemoteAddress) + sizeof(int) : 0)));

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
                base.ToStream(stream, includeHeader);
            if (IsRequest)
                return;

            using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
            writer.Write(ListenPort);
            writer.Write(WorldName ?? string.Empty);
            writer.Write(HostName ?? string.Empty);
            writer.Write(WorldWidth);
            writer.Write(HasCrimson);
            writer.Write(GameMode);
            writer.Write(MaxPlayers);
            writer.Write(ActivePlayers);
            writer.Write(Reserved ?? string.Empty);
            writer.Write(HasRemoteAddress);
            if (HasRemoteAddress)
            {
                writer.Write(RemoteAddress ?? string.Empty);
                writer.Write(RemotePort);
            }
        }
    }
}
