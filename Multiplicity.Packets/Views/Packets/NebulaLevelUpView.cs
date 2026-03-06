using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NebulaLevelUpView
    {
        private readonly PacketView _packet;
        private readonly NebulaLevelUp _packetObject;

        public NebulaLevelUpView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.NebulaLevelUp, payload))
        {
        }

        public static NebulaLevelUpView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new NebulaLevelUpView(payload);
        }

        public NebulaLevelUpView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<NebulaLevelUp>(packet, PacketTypes.NebulaLevelUp);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public NebulaLevelUp Packet => _packetObject;

        public byte PlayerId => _packetObject.PlayerId;

        public NebulaLevelUpType LevelUpType => _packetObject.LevelUpType;

        public float OriginX => _packetObject.OriginX;

        public float OriginY => _packetObject.OriginY;
    }
}


