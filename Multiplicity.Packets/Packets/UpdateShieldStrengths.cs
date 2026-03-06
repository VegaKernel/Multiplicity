using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The UpdateShieldStrengths (0x65) packet.
    /// </summary>
    public class UpdateShieldStrengths : TerrariaPacket
    {
        private const short SingleShieldLength = sizeof(ushort);
        private const short ShieldCount = 4;
        private const short PayloadLength = SingleShieldLength * ShieldCount;

        public ushort SolarTowerShield { get; set; }
        public ushort VortexTowerShield { get; set; }
        public ushort NebulaTowerShield { get; set; }
        public ushort StardustTowerShield { get; set; }

        public UpdateShieldStrengths()
            : base((byte)PacketTypes.UpdateShieldStrengths)
        {
        }

        public UpdateShieldStrengths(BinaryReader br)
            : base(br)
        {
            SolarTowerShield = br.ReadUInt16();
            VortexTowerShield = br.ReadUInt16();
            NebulaTowerShield = br.ReadUInt16();
            StardustTowerShield = br.ReadUInt16();
        }

        public override short GetLength()
        {
            return PayloadLength;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using (BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                bw.Write(SolarTowerShield);
                bw.Write(VortexTowerShield);
                bw.Write(NebulaTowerShield);
                bw.Write(StardustTowerShield);
            }
        }

        public override string ToString()
        {
            return $"[UpdateShieldStrengths: SolarTowerShield = {SolarTowerShield}, VortexTowerShield = {VortexTowerShield}, NebulaTowerShield = {NebulaTowerShield}, StardustTowerShield = {StardustTowerShield}]";
        }
    }
}
