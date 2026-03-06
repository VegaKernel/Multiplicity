using System;
using System.Collections.Generic;
using Multiplicity.Packets.Models;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetLiquidModuleView
    {
        private readonly ReadOnlySpan<byte> _sourceSpan;
        private readonly NetLiquidModule _module;

        public NetLiquidModuleView(ReadOnlySpan<byte> payload)
        {
            _sourceSpan = payload;
            _module = NetModuleViewMaterializer.Materialize<NetLiquidModule>(NetworkModuleTypes.NetLiquidModule, payload);
        }

        public NetLiquidModuleView(LoadNetModuleView module)
        {
            _sourceSpan = module.ModulePayloadSpan;
            _module = NetModuleViewMaterializer.Materialize<NetLiquidModule>(module, NetworkModuleTypes.NetLiquidModule);
        }

        public static NetLiquidModuleView FromPayload(ReadOnlySpan<byte> payload) => new NetLiquidModuleView(payload);

        public ReadOnlySpan<byte> SourceSpan => _sourceSpan;
        public int UpdateCount => _module.Updates?.Count ?? 0;
        public IReadOnlyList<LiquidUpdate> Updates => _module.Updates;
    }
}