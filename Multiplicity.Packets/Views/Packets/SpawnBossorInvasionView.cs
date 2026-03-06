using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct SpawnBossorInvasionView
    {
        private readonly PacketView _packet;
        private readonly SpawnBossorInvasion _packetObject;

        public SpawnBossorInvasionView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.SpawnBossorInvasion, payload))
        {
        }

        public static SpawnBossorInvasionView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new SpawnBossorInvasionView(payload);
        }

        public SpawnBossorInvasionView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<SpawnBossorInvasion>(packet, PacketTypes.SpawnBossorInvasion);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public SpawnBossorInvasion Packet => _packetObject;

        public short PlayerId => _packetObject.PlayerId;

        public short Type => _packetObject.Type;
    }
}


