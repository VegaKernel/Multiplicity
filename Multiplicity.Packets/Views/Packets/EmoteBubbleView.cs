using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct EmoteBubbleView
    {
        private readonly PacketView _packet;
        private readonly EmoteBubble _packetObject;

        public EmoteBubbleView(ReadOnlySpan<byte> payload)
            : this(PacketView.FromPayload(PacketTypes.EmoteBubble, payload))
        {
        }

        public static EmoteBubbleView FromPayload(ReadOnlySpan<byte> payload)
        {
            return new EmoteBubbleView(payload);
        }

        public EmoteBubbleView(PacketView packet)
        {
            _packet = packet;
            _packetObject = PacketViewMaterializer.Materialize<EmoteBubble>(packet, PacketTypes.EmoteBubble);
        }

        public PacketHeader Header => _packet.Header;

        public ReadOnlySpan<byte> SourceSpan => _packet.SourceSpan;

        public EmoteBubble Packet => _packetObject;

        public int BubbleId => _packetObject.BubbleId;

        public EmoteBubbleAnchorType AnchorType => _packetObject.AnchorType;

        public ushort AnchorMetaData => _packetObject.AnchorMetaData;

        public ushort Lifetime => _packetObject.Lifetime;

        public byte Emote => _packetObject.Emote;

        public ProjectileKey ProjectileKey => _packetObject.ProjectileKey;
    }
}


