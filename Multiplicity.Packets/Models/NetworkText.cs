using System;
using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets.Models
{
    /// <summary>
    /// Represents a translatable string of text.
    /// </summary>
    public class NetworkText
    {
        private const short FixedPayloadLength = 1;
        private const short SubstitutionCountPayloadLength = 1;

        public enum Mode : byte
        {
            Literal = 0,
            Formattable,
            LocalizationKey
        }

        public byte TextMode { get; set; }
        public string Text { get; set; }
        public byte SubstitutionListLength { get; set; }
        public NetworkText[] SubstitutionList { get; set; }

        public NetworkText()
        {
            Text = string.Empty;
            SubstitutionList = Array.Empty<NetworkText>();
        }

        public NetworkText(BinaryReader br)
        {
            TextMode = br.ReadByte();
            Text = br.ReadString();
            if (TextMode != (byte)Mode.Literal)
            {
                SubstitutionListLength = br.ReadByte();
                SubstitutionList = new NetworkText[SubstitutionListLength];

                for (int i = 0; i < SubstitutionListLength; i++)
                {
                    SubstitutionList[i] = br.ReadNetworkText();
                }
            }
            else
            {
                SubstitutionList = Array.Empty<NetworkText>();
            }
        }

        public void ToStream(BinaryWriter bw)
        {
            bw.Write(TextMode);
            bw.Write(Text ?? string.Empty);
            if (TextMode != (byte)Mode.Literal)
            {
                byte substitutionCount = (byte)Math.Min(SubstitutionList?.Length ?? 0, byte.MaxValue);
                SubstitutionListLength = substitutionCount;
                bw.Write(substitutionCount);

                for (int i = 0; i < substitutionCount; i++)
                {
                    bw.Write(SubstitutionList[i]);
                }
            }
        }

        public short GetLength()
        {
            short length = FixedPayloadLength;
            length += (short)PacketSerializationHelper.GetSerializedStringLength(Text);

            if (TextMode != (byte)Mode.Literal)
            {
                int substitutionCount = Math.Min(SubstitutionList?.Length ?? 0, byte.MaxValue);
                length += SubstitutionCountPayloadLength;
                for (int i = 0; i < substitutionCount; i++)
                {
                    length += SubstitutionList[i].GetLength();
                }
            }

            return length;
        }
    }
}
