using System;

namespace Multiplicity.Packets.Views
{
    public readonly ref struct NetCreativeUnlocksModuleView
    {
        private readonly LoadNetModuleView _module;
        private readonly NetCreativeUnlocksModule _packetObject;

        public NetCreativeUnlocksModuleView(LoadNetModuleView module)
        {
            _module = module;
            _packetObject = NetModuleViewMaterializer.Materialize<NetCreativeUnlocksModule>(module, NetworkModuleTypes.NetCreativeUnlocksModule);
        }

        public NetworkModuleTypes ModuleType => _module.ModuleType;
        public ReadOnlySpan<byte> ModulePayloadSpan => _module.ModulePayloadSpan;
        public NetCreativeUnlocksModule Module => _packetObject;
        public short ItemId => _packetObject.ItemId;
        public ushort SacrificeCount => _packetObject.SacrificeCount;
    }
}
