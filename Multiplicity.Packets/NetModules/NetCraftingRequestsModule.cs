using System.Collections.Generic;
using System.IO;

namespace Multiplicity.Packets
{
    public sealed class NetCraftingRequestsModule : TerrariaNetModule
    {
        public NetCraftingRequestsPayloadKind PayloadKind { get; set; } = NetCraftingRequestsPayloadKind.Request;

        public List<CraftingRequestItem> Items { get; } = new List<CraftingRequestItem>();

        public List<int> ChestIndices { get; } = new List<int>();

        public bool Approved { get; set; }

        public NetCraftingRequestsModule()
            : base(NetworkModuleTypes.NetCraftingRequestsModule)
        {
        }

        public NetCraftingRequestsModule(BinaryReader br)
            : base(br)
        {
            int remaining = (int)(br.BaseStream.Length - br.BaseStream.Position);
            if (remaining == 1)
            {
                PayloadKind = NetCraftingRequestsPayloadKind.Response;
                Approved = br.ReadBoolean();
                return;
            }

            PayloadKind = NetCraftingRequestsPayloadKind.Request;
            int itemCount = br.Read7BitEncodedInt();
            for (int i = 0; i < itemCount; i++)
            {
                Items.Add(new CraftingRequestItem
                {
                    ItemIdOrRecipeGroup = br.ReadInt32(),
                    Stack = br.Read7BitEncodedInt()
                });
            }

            int chestCount = br.Read7BitEncodedInt();
            for (int i = 0; i < chestCount; i++)
            {
                ChestIndices.Add(br.Read7BitEncodedInt());
            }
        }

        public override short GetLength()
        {
            if (PayloadKind == NetCraftingRequestsPayloadKind.Response)
            {
                return sizeof(bool);
            }

            int length = PacketSerializationHelper.Get7BitEncodedIntLength(Items.Count);
            for (int i = 0; i < Items.Count; i++)
            {
                length += sizeof(int);
                length += PacketSerializationHelper.Get7BitEncodedIntLength(Items[i].Stack);
            }

            length += PacketSerializationHelper.Get7BitEncodedIntLength(ChestIndices.Count);
            for (int i = 0; i < ChestIndices.Count; i++)
            {
                length += PacketSerializationHelper.Get7BitEncodedIntLength(ChestIndices[i]);
            }

            return (short)length;
        }

        public override void ToStream(Stream stream, bool includeHeader = true)
        {
            if (includeHeader)
            {
                base.ToStream(stream, includeHeader);
            }

            using BinaryWriter bw = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true);
            if (PayloadKind == NetCraftingRequestsPayloadKind.Response)
            {
                bw.Write(Approved);
                return;
            }

            bw.Write7BitEncodedInt(Items.Count);
            for (int i = 0; i < Items.Count; i++)
            {
                bw.Write(Items[i].ItemIdOrRecipeGroup);
                bw.Write7BitEncodedInt(Items[i].Stack);
            }

            bw.Write7BitEncodedInt(ChestIndices.Count);
            for (int i = 0; i < ChestIndices.Count; i++)
            {
                bw.Write7BitEncodedInt(ChestIndices[i]);
            }
        }
    }
}