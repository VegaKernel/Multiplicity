using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ShimmerActionsView
        {
            private readonly ReadOnlySpan<byte> _sourceSpan;
    
            public ShimmerActionsView(PacketView packet)
            {
                packet.EnsureType(PacketTypes.ShimmerActions);
                PacketReader reader = packet.CreatePayloadReader();
                ShimmerActionKind action = (ShimmerActionKind)reader.ReadByte();
                float positionX = reader.ReadSingle();
                float positionY = reader.ReadSingle();
                int coinLuckAmount = 0;
    
                if (action == ShimmerActionKind.AddCoinLuck)
                {
                    coinLuckAmount = reader.ReadInt32();
                }
    
                reader.EnsureEnd();
                Header = packet.Header;
                _sourceSpan = packet.SourceSpan;
                Action = action;
                PositionX = positionX;
                PositionY = positionY;
                CoinLuckAmount = coinLuckAmount;
            }
    
            public PacketHeader Header { get; }
            public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
            public ShimmerActionKind Action { get; }
            public float PositionX { get; }
            public float PositionY { get; }
            public int CoinLuckAmount { get; }
            public bool HasCoinLuckAmount => Action == ShimmerActionKind.AddCoinLuck;
        }
}

