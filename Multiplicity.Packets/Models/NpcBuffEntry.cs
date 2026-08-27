namespace Multiplicity.Packets.Models
{
    public sealed class NpcBuffEntry
    {
        public NpcBuffEntry()
        {
        }

        public NpcBuffEntry(ushort buffType, ushort time)
        {
            BuffType = buffType;
            Time = time;
        }

        public ushort BuffType { get; set; }

        public ushort Time { get; set; }
    }
}
