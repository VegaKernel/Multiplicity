using System;
using System.Collections.Generic;
using System.IO;

namespace Multiplicity.Packets.Extensions
{
    /// <summary>
    /// Decoder for a packet extension that overlays a Terraria packet id.
    /// Return null when the extension does not claim the payload so vanilla decoding may continue.
    /// </summary>
    public delegate TerrariaPacket PacketExtensionDecoder(BinaryReader reader);

    /// <summary>
    /// Immutable registry of opt-in packet extensions. Extension decoders are tried before the
    /// vanilla packet decoder for the same byte id, allowing plugin protocols to overlay unused
    /// or repurposed Terraria packet ids without contaminating the vanilla packet model.
    /// </summary>
    public sealed class PacketExtensionRegistry
    {
        private readonly IReadOnlyDictionary<byte, PacketExtensionDecoder[]> _decoders;

        private PacketExtensionRegistry(IReadOnlyDictionary<byte, PacketExtensionDecoder[]> decoders)
        {
            _decoders = decoders;
        }

        public static PacketExtensionRegistry Empty { get; } =
            new PacketExtensionRegistry(new Dictionary<byte, PacketExtensionDecoder[]>());

        public static Builder CreateBuilder() => new Builder();

        internal bool TryDeserialize(BinaryReader reader, byte packetId, out TerrariaPacket packet)
        {
            packet = null;
            if (!_decoders.TryGetValue(packetId, out PacketExtensionDecoder[] decoders))
            {
                return false;
            }

            long start = reader.BaseStream.Position;
            for (int i = 0; i < decoders.Length; i++)
            {
                reader.BaseStream.Position = start;
                TerrariaPacket candidate = decoders[i](reader);
                if (candidate != null)
                {
                    packet = candidate;
                    return true;
                }
            }

            reader.BaseStream.Position = start;
            return false;
        }

        public sealed class Builder
        {
            private readonly Dictionary<byte, List<PacketExtensionDecoder>> _decoders =
                new Dictionary<byte, List<PacketExtensionDecoder>>();

            public Builder Add(byte packetId, PacketExtensionDecoder decoder)
            {
                if (decoder == null)
                {
                    throw new ArgumentNullException(nameof(decoder));
                }

                if (!_decoders.TryGetValue(packetId, out List<PacketExtensionDecoder> list))
                {
                    list = new List<PacketExtensionDecoder>();
                    _decoders.Add(packetId, list);
                }

                list.Add(decoder);
                return this;
            }

            public PacketExtensionRegistry Build()
            {
                Dictionary<byte, PacketExtensionDecoder[]> frozen =
                    new Dictionary<byte, PacketExtensionDecoder[]>(_decoders.Count);

                foreach (KeyValuePair<byte, List<PacketExtensionDecoder>> pair in _decoders)
                {
                    frozen.Add(pair.Key, pair.Value.ToArray());
                }

                return new PacketExtensionRegistry(frozen);
            }
        }
    }
}
