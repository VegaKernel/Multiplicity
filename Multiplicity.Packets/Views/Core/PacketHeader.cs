using System.IO;

namespace Multiplicity.Packets.Views
{
    public readonly struct PacketHeader
    {
        public const int Size = TerrariaPacket.PACKET_HEADER_LEN;

        public PacketHeader(ushort packetLength, byte rawPacketId)
        {
            if (packetLength < Size)
            {
                throw new InvalidDataException($"Packet length {packetLength} is smaller than header size {Size}.");
            }

            PacketLength = packetLength;
            RawPacketId = rawPacketId;
            PacketType = (PacketTypes)rawPacketId;
        }

        public ushort PacketLength { get; }

        public byte RawPacketId { get; }

        public PacketTypes PacketType { get; }

        public int PayloadLength => PacketLength - Size;

        public static PacketHeader FromPayload(PacketTypes packetType, int payloadLength)
        {
            return FromPayload((byte)packetType, payloadLength);
        }

        public static PacketHeader FromPayload(byte rawPacketId, int payloadLength)
        {
            if (payloadLength < 0)
            {
                throw new InvalidDataException($"Payload length {payloadLength} cannot be negative.");
            }

            int packetLength = payloadLength + Size;
            if (packetLength > ushort.MaxValue)
            {
                throw new InvalidDataException(
                    $"Payload length {payloadLength} is too large to fit into a Terraria packet header.");
            }

            return new PacketHeader((ushort)packetLength, rawPacketId);
        }
    }
}
