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
    }
}
