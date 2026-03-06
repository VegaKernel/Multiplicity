using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SpectatePlayerView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public SpectatePlayerView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SpectatePlayer);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            short targetPlayerId = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            TargetPlayerId = targetPlayerId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public byte PlayerId { get; }
        public short TargetPlayerId { get; }
    }

    public readonly ref struct SyncItemDespawnView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public SyncItemDespawnView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SyncItemDespawn);
            PacketReader reader = packet.CreatePayloadReader();
            short itemId = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            ItemId = itemId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public short ItemId { get; }
    }

    public readonly ref struct ItemUseSoundView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public ItemUseSoundView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ItemUseSound);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public byte PlayerId { get; }
    }

    public readonly ref struct NpcHurtByDebuffView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public NpcHurtByDebuffView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.NpcHurtByDebuff);
            PacketReader reader = packet.CreatePayloadReader();
            byte npcId = reader.ReadByte();
            short damage = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            NpcId = npcId;
            Damage = damage;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public byte NpcId { get; }
        public short Damage { get; }
    }

    public readonly ref struct TeLeashedEntityAnchorPlaceItemView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public TeLeashedEntityAnchorPlaceItemView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.TELeashedEntityAnchorPlaceItem);
            PacketReader reader = packet.CreatePayloadReader();
            short tileX = reader.ReadInt16();
            short tileY = reader.ReadInt16();
            short itemType = reader.ReadInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            TileX = tileX;
            TileY = tileY;
            ItemType = itemType;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public short TileX { get; }
        public short TileY { get; }
        public short ItemType { get; }
    }

    public readonly ref struct TeamChangeFromUiView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public TeamChangeFromUiView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.TeamChangeFromUI);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            byte team = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
            Team = team;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public byte PlayerId { get; }
        public byte Team { get; }
    }

    public readonly ref struct ExtraSpawnSectionLoadedView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public ExtraSpawnSectionLoadedView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.ExtraSpawnSectionLoaded);
            PacketReader reader = packet.CreatePayloadReader();
            byte playerId = reader.ReadByte();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            PlayerId = playerId;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public byte PlayerId { get; }
    }

    public readonly ref struct RequestSectionView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public RequestSectionView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.RequestSection);
            PacketReader reader = packet.CreatePayloadReader();
            ushort sectionX = reader.ReadUInt16();
            ushort sectionY = reader.ReadUInt16();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            SectionX = sectionX;
            SectionY = sectionY;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public ushort SectionX { get; }
        public ushort SectionY { get; }
    }

    public readonly ref struct SyncItemPositionView
    {
        private readonly ReadOnlySpan<byte> _packetSpan;

        public SyncItemPositionView(PacketView packet)
        {
            packet.EnsureType(PacketTypes.SyncItemPosition);
            PacketReader reader = packet.CreatePayloadReader();
            short itemId = reader.ReadInt16();
            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            reader.EnsureEnd();

            Header = packet.Header;
            _packetSpan = packet.PacketSpan;
            ItemId = itemId;
            PositionX = positionX;
            PositionY = positionY;
        }

        public PacketHeader Header { get; }
        public ReadOnlySpan<byte> PacketSpan => _packetSpan;
        public short ItemId { get; }
        public float PositionX { get; }
        public float PositionY { get; }
    }
}
