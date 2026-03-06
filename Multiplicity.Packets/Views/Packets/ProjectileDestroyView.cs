using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct ProjectileDestroyView
    {
        private readonly PacketView _packet;
        private readonly ProjectileDestroy _packetObject;

        public ProjectileDestroyView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.ProjectileDestroy, payload))
        {
        }

        public static ProjectileDestroyView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new ProjectileDestroyView(payload);
        }

        public ProjectileDestroyView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<ProjectileDestroy>(packet, PacketTypes.ProjectileDestroy);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public ProjectileDestroy Packet => _packetObject;

        public short ProjectileId => _packetObject.ProjectileId;

        public byte Owner => _packetObject.Owner;
    }
}


