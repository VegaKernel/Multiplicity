using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct UpdateNPCNameView
    {
        private readonly PacketView _packet;
        private readonly UpdateNPCName _packetObject;

        public UpdateNPCNameView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.UpdateNPCName, payload))
        {
        }

        public static UpdateNPCNameView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new UpdateNPCNameView(payload);
        }

        public UpdateNPCNameView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<UpdateNPCName>(packet, PacketTypes.UpdateNPCName);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public UpdateNPCName Packet => _packetObject;

        public short NpcId => _packetObject.NpcId;

        public string Name => _packetObject.Name;

        public int TownNpcVariationIndex => _packetObject.TownNpcVariationIndex;

        public bool HasNameData => _packetObject.HasNameData;
    }
}


