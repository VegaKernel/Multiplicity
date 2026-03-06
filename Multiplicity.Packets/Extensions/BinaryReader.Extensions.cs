using System;
using System.Drawing;
using System.IO;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static ColorStruct ReadColor(this BinaryReader br)
        {
            return new ColorStruct { R = br.ReadByte(), G = br.ReadByte(), B = br.ReadByte() };
        }

        public static NetworkText ReadNetworkText(this BinaryReader br)
        {
            return new NetworkText(br);
        }
    }
}

