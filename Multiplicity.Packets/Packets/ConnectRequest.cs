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
        public const int Protocol325 = 325;
        public const int CurrentProtocol = Protocol325;
        public const string VersionPrefix = "Terraria";

        /// <summary>
        /// Gets or sets the protocol banner string. Example: "Terraria325".
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectRequest"/> class.
        /// </summary>
        public ConnectRequest()
            : base((byte)PacketTypes.ConnectRequest)
        {
            Version = BuildVersion(CurrentProtocol);
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
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter br = new BinaryWriter(stream, new System.Text.UTF8Encoding(), leaveOpen: true))
            {
                br.Write(Version ?? string.Empty);
            }
        }

        #endregion
    }
}
