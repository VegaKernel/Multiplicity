using System;
using System.Buffers.Binary;

namespace Multiplicity.Packets.Views
{
    internal static class NetModuleViewMaterializer
    {
        public static TModule Materialize<TModule>(LoadNetModuleView module, NetworkModuleTypes expectedType)
            where TModule : TerrariaNetModule
        {
            NetModuleViewHelper.EnsureType(module, expectedType);
            return Materialize<TModule>(expectedType, module.ModulePayloadSpan);
        }

        public static TModule Materialize<TModule>(NetworkModuleTypes moduleType, ReadOnlySpan<byte> payload)
            where TModule : TerrariaNetModule
        {
            byte[] buffer = new byte[TerrariaNetModule.ModuleHeaderLength + payload.Length];
            BinaryPrimitives.WriteUInt16LittleEndian(buffer.AsSpan(), (ushort)moduleType);
            payload.CopyTo(buffer.AsSpan(TerrariaNetModule.ModuleHeaderLength));
            return (TModule)TerrariaNetModule.Deserialize((ReadOnlyMemory<byte>)buffer);
        }
    }
}