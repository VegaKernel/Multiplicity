using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct EmptyPayloadPacketView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public EmptyPayloadPacketView(PacketView packet, PacketTypes expectedPacketType)
        {
            packet.EnsureType(expectedPacketType);
            PacketReader reader = packet.CreatePayloadReader();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PacketType = expectedPacketType;
        }

        public PacketHeader Header { get; }
        public PacketTypes PacketType { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
    }

    public readonly ref struct TileEntityItemSyncPacketView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public TileEntityItemSyncPacketView(PacketView packet, PacketTypes expectedPacketType)
        {
            packet.EnsureType(expectedPacketType);
            PacketReader reader = packet.CreatePayloadReader();
            short tileX = reader.ReadInt16();
            short tileY = reader.ReadInt16();
            short itemType = reader.ReadInt16();
            byte prefix = reader.ReadByte();
            short stack = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PacketType = expectedPacketType;
            TileX = tileX;
            TileY = tileY;
            ItemType = itemType;
            Prefix = prefix;
            Stack = stack;
        }

        public PacketHeader Header { get; }
        public PacketTypes PacketType { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public short TileX { get; }
        public short TileY { get; }
        public short ItemType { get; }
        public byte Prefix { get; }
        public short Stack { get; }
    }

    public readonly ref struct EmojiView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public EmojiView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.Emoji);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            byte emoteId = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            EmoteId = emoteId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public byte EmoteId { get; }
    }

    public readonly ref struct TileEntityDisplayDollItemSyncView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public TileEntityDisplayDollItemSyncView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.TileEntityDisplayDollItemSync);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            int tileEntityId = reader.ReadInt32();
            byte itemIndex = reader.ReadByte();
            DisplayDollSyncCommand command = (DisplayDollSyncCommand)reader.ReadByte();
            byte pose = 0;
            ushort itemType = 0;
            ushort stack = 0;
            byte prefix = 0;

            if (command == DisplayDollSyncCommand.Pose)
            {
                pose = reader.ReadByte();
            }
            else
            {
                itemType = reader.ReadUInt16();
                stack = reader.ReadUInt16();
                prefix = reader.ReadByte();
            }

            reader.EnsureEnd();
            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            TileEntityId = tileEntityId;
            ItemIndex = itemIndex;
            Command = command;
            Pose = pose;
            ItemType = itemType;
            Stack = stack;
            Prefix = prefix;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public int TileEntityId { get; }
        public byte ItemIndex { get; }
        public DisplayDollSyncCommand Command { get; }
        public byte Pose { get; }
        public ushort ItemType { get; }
        public ushort Stack { get; }
        public byte Prefix { get; }
        public bool HasItemData => Command != DisplayDollSyncCommand.Pose;
    }

    public readonly ref struct RequestTileEntityInteractionView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public RequestTileEntityInteractionView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.RequestTileEntityInteraction);
            PacketReader reader = packet.CreatePayloadReader();
            int tileEntityId = reader.ReadInt32();
            byte playerId = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            TileEntityId = tileEntityId;
            PlayerId = playerId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public int TileEntityId { get; }
        public byte PlayerId { get; }
    }

    public readonly ref struct TileEntityHatRackItemSyncView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public TileEntityHatRackItemSyncView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.TileEntityHatRackItemSync);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            int tileEntityId = reader.ReadInt32();
            byte encodedSlot = reader.ReadByte();
            ushort itemType = reader.ReadUInt16();
            ushort stack = reader.ReadUInt16();
            byte prefix = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            TileEntityId = tileEntityId;
            EncodedSlot = encodedSlot;
            ItemType = itemType;
            Stack = stack;
            Prefix = prefix;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public int TileEntityId { get; }
        public byte EncodedSlot { get; }
        public ushort ItemType { get; }
        public ushort Stack { get; }
        public byte Prefix { get; }
    }

    public readonly ref struct SyncTilePickingView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;

        public SyncTilePickingView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SyncTilePicking);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short tileX = reader.ReadInt16();
            short tileY = reader.ReadInt16();
            byte pickPower = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _sourceSpan = packet.SourceSpan;
            PlayerId = playerId;
            TileX = tileX;
            TileY = tileY;
            PickPower = pickPower;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId { get; }
        public short TileX { get; }
        public short TileY { get; }
        public byte PickPower { get; }
    }
}



