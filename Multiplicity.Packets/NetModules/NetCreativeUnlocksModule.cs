using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// Restored creative item sacrifice synchronization module in Terraria protocol 321+.
    /// </summary>
    public sealed class NetCreativeUnlocksModule : TerrariaNetModule
    {
        private const short PayloadLength = sizeof(short) + sizeof(ushort);

        public short ItemId { get; set; }
        public ushort SacrificeCount { get; set; }

        public NetCreativeUnlocksModule()
            : base(NetworkModuleTypes.NetCreativeUnlocksModule)
        {
        }

        public NetCreativeUnlocksModule(BinaryReader br)
            : base(br)
        {
            ItemId = br.ReadInt16();
            SacrificeCount = br.ReadUInt16();
        }

        public override short GetLength() => PayloadLength;

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
                base.ToStream(stream, includeHeader);

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            bw.Write(ItemId);
            bw.Write(SacrificeCount);
        }
    }
}
