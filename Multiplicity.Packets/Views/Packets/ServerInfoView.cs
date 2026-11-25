using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ServerInfoView
    {
        private readonly PacketView _packet;

        public ServerInfoView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ServerInfo, payload))
        {
        }

        public static ServerInfoView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ServerInfoView(payload);
        }

        public ServerInfoView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ServerInfo);
            _packet = packet;
            PacketReader reader = packet.CreatePayloadReader();
            if (reader.EndOfBuffer)
                return;

            ListenPort = reader.ReadInt32();
            WorldNameUtf8 = reader.ReadLengthPrefixedBytes();
            HostNameUtf8 = reader.ReadLengthPrefixedBytes();
            WorldWidth = reader.ReadUInt16();
            HasCrimson = reader.ReadBoolean();
            GameMode = reader.ReadInt32();
            MaxPlayers = reader.ReadByte();
            ActivePlayers = reader.ReadByte();
            ReservedUtf8 = reader.ReadLengthPrefixedBytes();
            HasRemoteAddress = reader.ReadBoolean();
            if (HasRemoteAddress)
            {
                RemoteAddressUtf8 = reader.ReadLengthPrefixedBytes();
                RemotePort = reader.ReadInt32();
            }
            reader.EnsureEnd();
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public bool IsRequest => _packet.PayloadSpan.IsEmpty;
        public int ListenPort { get; }
        public ReadOnlySpan<byte> WorldNameUtf8 { get; }
        public ReadOnlySpan<byte> HostNameUtf8 { get; }
        public ushort WorldWidth { get; }
        public bool HasCrimson { get; }
        public int GameMode { get; }
        public byte MaxPlayers { get; }
        public byte ActivePlayers { get; }
        public ReadOnlySpan<byte> ReservedUtf8 { get; }
        public bool HasRemoteAddress { get; }
        public ReadOnlySpan<byte> RemoteAddressUtf8 { get; }
        public int RemotePort { get; }
        public ServerInfo Packet => PacketViewMaterializer.Materialize<ServerInfo>(_packet, PacketTypes.ServerInfo);
    }
}
