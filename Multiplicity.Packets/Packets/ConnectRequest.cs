using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ConnectRequest (0x1) packet.
    /// </summary>
    public class ConnectRequest : TerrariaPacket
    {
        public const int Protocol317 = 317;
        public const int Protocol318 = 318;
        public const string VersionPrefix = "Terraria";

        /// <summary>
        /// Gets or sets the protocol banner string. Example: "Terraria318".
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectRequest"/> class.
        /// </summary>
        public ConnectRequest()
            : base((byte)PacketTypes.ConnectRequest)
        {
            Version = BuildVersion(Protocol318);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectRequest"/> class.
        /// </summary>
        /// <param name="br">br</param>
        public ConnectRequest(BinaryReader br)
            : base(br)
        {
            this.Version = br.ReadString();
        }

        public override string ToString()
        {
            return $"[ConnectRequest: Version = {Version}]";
        }

        public static string BuildVersion(int protocol) => $"{VersionPrefix}{protocol}";

        public bool IsProtocol(int protocol) =>
            string.Equals(Version, BuildVersion(protocol), StringComparison.Ordinal);

        #region implemented abstract members of TerrariaPacket

        public override short GetLength()
        {
            return (short)PacketSerializationHelper.GetSerializedStringLength(Version);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            /*
             * Length and ID headers get written in the base packet class.
             */
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            /*
             * Always make sure to not close the stream when serializing.
             * 
             * It is up to the caller to decide if the underlying stream
             * gets closed.  If this is a network stream we do not want
             * the regressions of unconditionally closing the TCP socket
             * once the payload of data has been sent to the client.
             */
            using (BinaryWriter br = new BinaryWriter(stream, new System.Text.UTF8Encoding(), leaveOpen: true))
            {
                br.Write(Version ?? string.Empty);
            }
        }

        #endregion

    }
}
