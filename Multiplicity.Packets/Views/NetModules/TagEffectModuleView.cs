using System;
using System.Collections.Generic;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct TagEffectModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;
        private readonly TagEffectModule _module;

        public TagEffectModuleView(ReadOnlySpan<byte> payload)
        {
            _sourceSpan = payload;
            _module = NetModuleViewMaterializer.Materialize<TagEffectModule>(NetworkModuleTypes.TagEffectModule, payload);
        }

        public TagEffectModuleView(LoadNetModuleView module)
        {
            _sourceSpan = module.ModulePayloadSpan;
            _module = NetModuleViewMaterializer.Materialize<TagEffectModule>(module, NetworkModuleTypes.TagEffectModule);
        }

        public static TagEffectModuleView FromPayload(ReadOnlySpan<byte> payload) => new TagEffectModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public byte PlayerId => _module.PlayerId;
        public TagEffectMessageType MessageType => _module.MessageType;
        public short EffectType => _module.EffectType;
        public byte NpcIndex => _module.NpcIndex;
        public bool IncludeProcTimeEntries => _module.IncludeProcTimeEntries;
        public IReadOnlyList<TagEffectNpcTimeEntry> TimeEntries => _module.TimeEntries;
        public IReadOnlyList<TagEffectNpcTimeEntry> ProcTimeEntries => _module.ProcTimeEntries;
    }
}