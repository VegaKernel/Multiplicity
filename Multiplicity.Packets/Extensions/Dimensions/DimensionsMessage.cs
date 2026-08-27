using System;
using System.IO;
using System.Net;

namespace Multiplicity.Packets.Extensions.Dimensions
{
    /// <summary>
    /// Upstream Dimensions extension carried over vanilla-unused packet id 67.
    /// Payload: Int16 messageType followed by a BinaryWriter string payload.
    /// </summary>
    public sealed class DimensionsMessage : TerrariaPacket
    {
        public const byte PacketId = 67;
        public const short RealIpMessageType = 0;

        public short MessageType { get; set; }

        public string Payload { get; set; } = string.Empty;

        public bool IsRealIp => MessageType == RealIpMessageType;

        public string ReportedRealIp => IsRealIp ? Payload : null;

        public DimensionsMessage()
            : base(PacketId)
        {
        }

        public DimensionsMessage(BinaryReader reader)
            : base(reader)
        {
            MessageType = reader.ReadInt16();
            Payload = reader.ReadString();
        }

        public static DimensionsMessage CreateRealIp(string ipAddress)
        {
            if (!IPAddress.TryParse(ipAddress, out IPAddress parsedAddress))
            {
                throw new ArgumentException("Dimensions real-IP payload must be a valid IPv4 or IPv6 address.", nameof(ipAddress));
            }

            return new DimensionsMessage
            {
                MessageType = RealIpMessageType,
                Payload = parsedAddress.ToString()
            };
        }

        public bool TryGetReportedRealIp(out IPAddress address)
        {
            if (!IsRealIp)
            {
                address = null;
                return false;
            }

            return IPAddress.TryParse(Payload, out address);
        }

        public override short GetLength()
        {
            return checked((short)(sizeof(short) + PacketSerializationHelper.GetSerializedStringLength(Payload ?? string.Empty)));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            writer.Write(MessageType);
            writer.Write(Payload ?? string.Empty);
        }

        public override string ToString()
        {
            return $"[DimensionsMessage: Type = {MessageType} Payload = {Payload}]";
        }
    }
}
