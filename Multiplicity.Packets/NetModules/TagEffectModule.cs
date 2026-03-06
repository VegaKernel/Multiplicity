using System.Collections.Generic;
using System.IO;

namespace Multiplicity.Packets
{
    public sealed class TagEffectModule : TerrariaNetModule
    {
        private const byte SparseNpcArrayTerminator = 200;

        public byte PlayerId { get; set; }

        public TagEffectMessageType MessageType { get; set; }

        public short EffectType { get; set; }

        public byte NpcIndex { get; set; }

        public List<TagEffectNpcTimeEntry> TimeEntries { get; } = new List<TagEffectNpcTimeEntry>();

        public List<TagEffectNpcTimeEntry> ProcTimeEntries { get; } = new List<TagEffectNpcTimeEntry>();

        public bool IncludeProcTimeEntries { get; set; }

        public TagEffectModule()
            : base(NetworkModuleTypes.TagEffectModule)
        {
        }

        public TagEffectModule(BinaryReader br)
            : base(br)
        {
            PlayerId = br.ReadByte();
            MessageType = (TagEffectMessageType)br.ReadByte();

            switch (MessageType)
            {
                case TagEffectMessageType.FullState:
                    EffectType = br.ReadInt16();
                    ReadSparseNpcTimeArray(br, TimeEntries);
                    if (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        IncludeProcTimeEntries = true;
                        ReadSparseNpcTimeArray(br, ProcTimeEntries);
                    }
                    break;
                case TagEffectMessageType.ChangeActiveEffect:
                    EffectType = br.ReadInt16();
                    break;
                case TagEffectMessageType.ApplyTagToNpc:
                case TagEffectMessageType.EnableProcOnNpc:
                case TagEffectMessageType.ClearProcOnNpc:
                    NpcIndex = br.ReadByte();
                    break;
            }
        }

        public override short GetLength()
        {
            int length = sizeof(byte) + sizeof(byte);
            switch (MessageType)
            {
                case TagEffectMessageType.FullState:
                    length += sizeof(short);
                    length += GetSparseNpcTimeArrayLength(TimeEntries);
                    if (IncludeProcTimeEntries || ProcTimeEntries.Count > 0)
                    {
                        length += GetSparseNpcTimeArrayLength(ProcTimeEntries);
                    }
                    break;
                case TagEffectMessageType.ChangeActiveEffect:
                    length += sizeof(short);
                    break;
                case TagEffectMessageType.ApplyTagToNpc:
                case TagEffectMessageType.EnableProcOnNpc:
                case TagEffectMessageType.ClearProcOnNpc:
                    length += sizeof(byte);
                    break;
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
            bw.Write(PlayerId);
            bw.Write((byte)MessageType);

            switch (MessageType)
            {
                case TagEffectMessageType.FullState:
                    bw.Write(EffectType);
                    WriteSparseNpcTimeArray(bw, TimeEntries);
                    if (IncludeProcTimeEntries || ProcTimeEntries.Count > 0)
                    {
                        WriteSparseNpcTimeArray(bw, ProcTimeEntries);
                    }
                    break;
                case TagEffectMessageType.ChangeActiveEffect:
                    bw.Write(EffectType);
                    break;
                case TagEffectMessageType.ApplyTagToNpc:
                case TagEffectMessageType.EnableProcOnNpc:
                case TagEffectMessageType.ClearProcOnNpc:
                    bw.Write(NpcIndex);
                    break;
            }
        }

        private static void ReadSparseNpcTimeArray(BinaryReader br, List<TagEffectNpcTimeEntry> target)
        {
            target.Clear();
            while (true)
            {
                byte npcIndex = br.ReadByte();
                if (npcIndex >= SparseNpcArrayTerminator)
                {
                    break;
                }

                target.Add(new TagEffectNpcTimeEntry
                {
                    NpcIndex = npcIndex,
                    TimeLeft = br.ReadInt32()
                });
            }
        }

        private static void WriteSparseNpcTimeArray(BinaryWriter bw, List<TagEffectNpcTimeEntry> source)
        {
            for (int i = 0; i < source.Count; i++)
            {
                bw.Write(source[i].NpcIndex);
                bw.Write(source[i].TimeLeft);
            }

            bw.Write(SparseNpcArrayTerminator);
        }

        private static int GetSparseNpcTimeArrayLength(List<TagEffectNpcTimeEntry> source)
        {
            return (source.Count * (sizeof(byte) + sizeof(int))) + sizeof(byte);
        }
    }
}