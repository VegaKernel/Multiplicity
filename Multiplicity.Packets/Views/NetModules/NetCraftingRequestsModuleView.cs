using System;
using System.Collections.Generic;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetCraftingRequestsModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;
        private readonly NetCraftingRequestsModule _module;

        public NetCraftingRequestsModuleView(ReadOnlySpan<byte> payload)
        {
            _sourceSpan = payload;
            _module = NetModuleViewMaterializer.Materialize<NetCraftingRequestsModule>(NetworkModuleTypes.NetCraftingRequestsModule, payload);
        }

        public NetCraftingRequestsModuleView(LoadNetModuleView module)
        {
            _sourceSpan = module.ModulePayloadSpan;
            _module = NetModuleViewMaterializer.Materialize<NetCraftingRequestsModule>(module, NetworkModuleTypes.NetCraftingRequestsModule);
        }

        public static NetCraftingRequestsModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetCraftingRequestsModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public NetCraftingRequestsPayloadKind PayloadKind => _module.PayloadKind;
        public bool Approved => _module.Approved;
        public IReadOnlyList<CraftingRequestItem> Items => _module.Items;
        public IReadOnlyList<int> ChestIndices => _module.ChestIndices;
    }
}