using System.IO;
using Multiplicity.Packets.Extensions;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The CreateCombatText (0x51) packet.
    /// </summary>
    public class CreateCombatText : TerrariaPacket
    {
        private const short FixedPayloadLength = 15;

        public float X { get; set; }
        public float Y { get; set; }
        public ColorStruct Color { get; set; }
        public int HealAmount { get; set; }

        public CreateCombatText()
            : base((byte)PacketTypes.CreateCombatText)
        {
        }

        public CreateCombatText(BinaryReader br)
            : base(br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
            Color = br.ReadColor();
            HealAmount = br.ReadInt32();
        }

        public override short GetLength()
        {
            return FixedPayloadLength;
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
                bw.Write(HealAmount);
            }
        }

        public override string ToString()
        {
            return $"[CreateCombatText: X = {X}, Y = {Y}, Color = {Color}, HealAmount = {HealAmount}]";
        }
    }
}
