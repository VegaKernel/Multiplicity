using System;
using System.IO;
using System.Net;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Packet id 67 is unused by vanilla Terraria 1.4.5.x and is used by the upstream
    /// Dimensions server proxy as an extension carrier.
    /// </summary>
    public class Placeholder : TerrariaPacket
    {
        /// <summary>
        /// Dimensions message type 0 reports the client's address observed by the proxy.
        /// </summary>
        public const short DimensionsRealIpMessageType = 0;

        /// <summary>
        /// Dimensions message type, or null when this is a bare vanilla placeholder packet.
        /// </summary>
        public short? DimensionsMessageType { get; set; }

        /// <summary>
        /// Dimensions string payload. For message type 0 this is the proxy-reported client IP.
        /// </summary>
        public string DimensionsPayload { get; set; } = string.Empty;

        public bool IsDimensionsMessage => DimensionsMessageType.HasValue;

        public bool IsDimensionsRealIp => DimensionsMessageType == DimensionsRealIpMessageType;

        /// <summary>
        /// Returns the IP text reported by Dimensions for message type 0, otherwise null.
        /// The value is a report from an extension peer and is not authenticated by the Terraria protocol.
        /// </summary>
        public string ReportedRealIp => IsDimensionsRealIp ? DimensionsPayload : null;

        public Placeholder()
            : base((byte)PacketTypes.Placeholder)
        {
        }

        public Placeholder(BinaryReader br)
            : base(br)
        {
            if (br.BaseStream.Position >= br.BaseStream.Length)
            {
                return;
            }

            DimensionsMessageType = br.ReadInt16();
            DimensionsPayload = br.ReadString();
        }

        public static Placeholder CreateDimensionsRealIp(string ipAddress)
        {
            if (!IPAddress.TryParse(ipAddress, out IPAddress parsedAddress))
            {
                throw new ArgumentException("Dimensions real-IP payload must be a valid IPv4 or IPv6 address.", nameof(ipAddress));
            }

            return new Placeholder
            {
                DimensionsMessageType = DimensionsRealIpMessageType,
                DimensionsPayload = parsedAddress.ToString()
            };
        }

        public bool TryGetReportedRealIp(out IPAddress address)
        {
            if (!IsDimensionsRealIp)
            {
                address = null;
                return false;
            }

            return IPAddress.TryParse(DimensionsPayload, out address);
        }

        public override string ToString()
        {
            return IsDimensionsMessage
                ? $"[DimensionsMessage: Type = {DimensionsMessageType} Payload = {DimensionsPayload}]"
                : "[Placeholder]";
        }

        public override short GetLength()
        {
            if (!DimensionsMessageType.HasValue)
            {
                return 0;
            }

            return checked((short)(sizeof(short) + PacketSerializationHelper.GetSerializedStringLength(DimensionsPayload ?? string.Empty)));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            if (!DimensionsMessageType.HasValue)
            {
                return;
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(DimensionsMessageType.Value);
            bw.Write(DimensionsPayload ?? string.Empty);
        }
    }
}
