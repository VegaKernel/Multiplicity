using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct UpdateShieldStrengthsView
    {
        private readonly PacketView _packet;
        private readonly UpdateShieldStrengths _packetObject;

        public UpdateShieldStrengthsView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.UpdateShieldStrengths, payload))
        {
        }

        public static UpdateShieldStrengthsView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new UpdateShieldStrengthsView(payload);
        }

        public UpdateShieldStrengthsView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<UpdateShieldStrengths>(packet, PacketTypes.UpdateShieldStrengths);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public UpdateShieldStrengths Packet => _packetObject;

        public ushort SolarTowerShield => _packetObject.SolarTowerShield;

        public ushort VortexTowerShield => _packetObject.VortexTowerShield;

        public ushort NebulaTowerShield => _packetObject.NebulaTowerShield;

        public ushort StardustTowerShield => _packetObject.StardustTowerShield;
    }
}


