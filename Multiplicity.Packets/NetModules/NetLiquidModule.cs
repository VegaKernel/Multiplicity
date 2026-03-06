using System.Collections.Generic;
using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    /// <summary>
    /// NetModule communicating bulk updates to liquids in tiles.
    /// </summary>
    public class NetLiquidModule : TerrariaNetModule
    {
        private const short CountPayloadLength = 2;
        private const short SingleUpdatePayloadLength = 6;

        public List<LiquidUpdate> Updates { get; set; }

        public NetLiquidModule()
            : base(NetworkModuleTypes.NetLiquidModule)
        {
            Updates = new List<LiquidUpdate>();
        }

        public NetLiquidModule(BinaryReader br)
            : base(br)
        {
            Updates = new List<LiquidUpdate>();

            int length = br.ReadUInt16();
            for (int i = 0; i < length; i++)
            {
                Updates.Add(new LiquidUpdate(br));
            }
        }

        public override short GetLength()
        {
            return (short)(CountPayloadLength + ((Updates?.Count ?? 0) * SingleUpdatePayloadLength));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                int updateCount = Updates?.Count ?? 0;
                bw.Write((ushort)updateCount);
                for (int i = 0; i < updateCount; i++)
                {
                    Updates[i].ToStream(stream);
                }
            }
        }
    }
}
