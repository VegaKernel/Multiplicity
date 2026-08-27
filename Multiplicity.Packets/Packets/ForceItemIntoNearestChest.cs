using System;
using System.IO;

namespace Multiplicity.Packets
{
    public enum QuickStackChestsPayloadKind : byte
    {
        InventoryRequest = 0,
        BlockedChestList = 1
    }

    /// <summary>
    /// Packet 85 (QuickStackChests). The payload is direction-dependent:
    /// client-to-server sends Int32 count + Int16 inventory slot ids + Boolean smartStack;
    /// server-to-client sends Int32 count + UInt16 blocked chest ids.
    /// </summary>
    public class ForceItemIntoNearestChest : TerrariaPacket
    {
        private const short CountPayloadLength = 4;
        private const short EntryPayloadLength = 2;
        private const short SmartStackPayloadLength = 1;

        public QuickStackChestsPayloadKind PayloadKind { get; set; }
        public short[] InventorySlots { get; set; } = Array.Empty<short>();
        public bool SmartStack { get; set; }
        public ushort[] BlockedChestIds { get; set; } = Array.Empty<ushort>();

        [Obsolete("Packet 85 is QuickStackChests, not a single inventory-slot packet. Use InventorySlots.")]
        public byte InventorySlot
        {
            get => InventorySlots.Length == 0 ? (byte)0 : unchecked((byte)InventorySlots[0]);
            set
            {
                PayloadKind = QuickStackChestsPayloadKind.InventoryRequest;
                InventorySlots = new[] { (short)value };
            }
        }

        public ForceItemIntoNearestChest()
            : base((byte)PacketTypes.ForceItemIntoNearestChest)
        {
        }

        public ForceItemIntoNearestChest(BinaryReader br)
            : base(br)
        {
            int count = br.ReadInt32();
            if (count < 0)
            {
                throw new InvalidDataException($"QuickStackChests contains negative entry count {count}.");
            }

            long entryBytes = checked((long)count * EntryPayloadLength);
            long remaining = br.BaseStream.Length - br.BaseStream.Position;

            if (remaining == entryBytes + SmartStackPayloadLength)
            {
                PayloadKind = QuickStackChestsPayloadKind.InventoryRequest;
                InventorySlots = new short[count];
                for (int i = 0; i < count; i++)
                {
                    InventorySlots[i] = br.ReadInt16();
                }
                SmartStack = br.ReadBoolean();
                return;
            }

            if (remaining == entryBytes)
            {
                PayloadKind = QuickStackChestsPayloadKind.BlockedChestList;
                BlockedChestIds = new ushort[count];
                for (int i = 0; i < count; i++)
                {
                    BlockedChestIds[i] = br.ReadUInt16();
                }
                return;
            }

            throw new InvalidDataException(
                $"QuickStackChests entry count {count} does not match remaining payload length {remaining}.");
        }

        public override short GetLength()
        {
            int count = PayloadKind == QuickStackChestsPayloadKind.InventoryRequest
                ? InventorySlots?.Length ?? 0
                : BlockedChestIds?.Length ?? 0;

            int length = CountPayloadLength + checked(count * EntryPayloadLength);
            if (PayloadKind == QuickStackChestsPayloadKind.InventoryRequest)
            {
                length += SmartStackPayloadLength;
            }

            return checked((short)length);
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            if (PayloadKind == QuickStackChestsPayloadKind.InventoryRequest)
            {
                short[] slots = InventorySlots ?? Array.Empty<short>();
                bw.Write(slots.Length);
                for (int i = 0; i < slots.Length; i++)
                {
                    bw.Write(slots[i]);
                }
                bw.Write(SmartStack);
            }
            else
            {
                ushort[] chests = BlockedChestIds ?? Array.Empty<ushort>();
                bw.Write(chests.Length);
                for (int i = 0; i < chests.Length; i++)
                {
                    bw.Write(chests[i]);
                }
            }
        }

        public override string ToString()
        {
            int count = PayloadKind == QuickStackChestsPayloadKind.InventoryRequest
                ? InventorySlots?.Length ?? 0
                : BlockedChestIds?.Length ?? 0;
            return $"[QuickStackChests: PayloadKind = {PayloadKind} Count = {count} SmartStack = {SmartStack}]";
        }
    }
}
