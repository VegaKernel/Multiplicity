using System.IO;
using Multiplicity.Packets.Extensions;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The CreateCombatTextExtended (0x77) packet.
    /// </summary>
    public class CreateCombatTextExtended : TerrariaPacket
    {
        private const short FixedPayloadLength = 11;

        public float X { get; set; }
        public float Y { get; set; }
        public ColorStruct Color { get; set; }
        public NetworkText CombatText { get; set; }

        public CreateCombatTextExtended()
            : base((byte)PacketTypes.CreateCombatTextExtended)
        {
            CombatText = new NetworkText();
        }

        public CreateCombatTextExtended(BinaryReader br)
            : base(br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
            Color = br.ReadColor();
            CombatText = br.ReadNetworkText();
        }

        public override short GetLength()
        {
            return (short)(FixedPayloadLength + (CombatText?.GetLength() ?? 0));
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(X);
                bw.Write(Y);
                bw.Write(Color);
                bw.Write(CombatText ?? new NetworkText());
            }
        }

        public override string ToString()
        {
            return $"[CreateCombatTextExtended: X = {X}, Y = {Y}, Color = {Color}, CombatText = {CombatText?.Text}]";
        }
    }
}
