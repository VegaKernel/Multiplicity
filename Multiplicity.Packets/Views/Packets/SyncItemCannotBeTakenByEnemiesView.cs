using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SyncItemCannotBeTakenByEnemiesView
    {
        private readonly WorldItemSyncView _view;

        public SyncItemCannotBeTakenByEnemiesView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.SyncItemCannotBeTakenByEnemiesDeprecated, payload))
        {
        }

        public static SyncItemCannotBeTakenByEnemiesView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new SyncItemCannotBeTakenByEnemiesView(payload);
        }

        public SyncItemCannotBeTakenByEnemiesView(PacketView packet)
        {
            _view = new WorldItemSyncView(packet);
        }

        public PacketHeader Header => _view.Header;
        public ReadOnlySpan<byte> SourceSpan => _view.SourceSpan;
        public short ItemIndex => _view.ItemIndex;
        public float PositionX => _view.PositionX;
        public float PositionY => _view.PositionY;
        public float VelocityX => _view.VelocityX;
        public float VelocityY => _view.VelocityY;
        public short Stack => _view.Stack;
        public byte Prefix => _view.Prefix;
        public WorldItemSyncFlags Flags => _view.Flags;
        public short ItemNetId => _view.ItemNetId;
    }
}
