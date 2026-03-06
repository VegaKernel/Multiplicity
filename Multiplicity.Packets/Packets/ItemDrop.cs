using System.IO;

namespace Multiplicity.Packets
{
    /// <summary>
    /// The ItemDrop (0x15) packet.
    /// </summary>
    public class ItemDrop : WorldItemSyncPacketBase
    {
        public ItemDrop()
            : base((byte)PacketTypes.ItemDrop)
        {
        }

        public ItemDrop(BinaryReader br)
            : base(br)
        {
        }

        public override string ToString()
        {
            return $"[ItemDrop: ItemIndex = {ItemIndex} PositionX = {PositionX} PositionY = {PositionY} VelocityX = {VelocityX} VelocityY = {VelocityY} Stack = {Stack} Prefix = {Prefix} Flags = {Flags} ItemNetId = {ItemNetId}]";
        }
    }
}

