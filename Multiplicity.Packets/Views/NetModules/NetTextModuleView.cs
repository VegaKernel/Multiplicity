using System;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetTextModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;
        private readonly NetTextModule _module;

        public NetTextModuleView(ReadOnlySpan<byte> payload)
        {
            _sourceSpan = payload;
            _module = NetModuleViewMaterializer.Materialize<NetTextModule>(NetworkModuleTypes.NetTextModule, payload);
        }

        public NetTextModuleView(LoadNetModuleView module)
        {
            _sourceSpan = module.ModulePayloadSpan;
            _module = NetModuleViewMaterializer.Materialize<NetTextModule>(module, NetworkModuleTypes.NetTextModule);
        }

        public static NetTextModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetTextModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public NetTextModulePayloadKind PayloadKind => _module.PayloadKind;
        public string CommandName => _module.CommandName;
        public string ChatMessage => _module.ChatMessage;
        public byte AuthorId => _module.AuthorId;
        public NetworkText ServerText => _module.ServerText;
        public ColorStruct MessageColor => _module.MessageColor;
        public ReadOnlySpan<byte> RawPayloadSpan => _module.RawPayloadMemory.Span;
    }
}