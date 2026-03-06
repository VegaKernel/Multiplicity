using System;

namespace Multiplicity.Packets.Views
{
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
}

