using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Multiplicity.Packets.Generators
{
    /// <summary>
    /// Compile-time guard rails for the source-generated vanilla packet registry.
    /// A packet must be explicitly ignored or be fully registrable; silent omission is forbidden.
    /// </summary>
    [Generator]
    public sealed class VanillaPacketRegistryDiagnosticsGenerator : IIncrementalGenerator
    {
        private const string PacketNamespace = "Multiplicity.Packets";
        private const string PacketBaseMetadataName = "Multiplicity.Packets.TerrariaPacket";
        private const string PacketTypesMetadataName = "Multiplicity.Packets.PacketTypes";
        private const string IgnoreAttributeMetadataName = "Multiplicity.Packets.PacketRegistrationIgnoreAttribute";
        private const string BinaryReaderMetadataName = "System.IO.BinaryReader";

        private static readonly DiagnosticDescriptor MissingPrerequisite = new DiagnosticDescriptor(
            "MPR000",
            "Packet registry generator prerequisite is missing",
            "The vanilla packet registry cannot be validated because required type '{0}' was not found",
            "Multiplicity.PacketRegistry",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor MissingPacketTypeId = new DiagnosticDescriptor(
            "MPR001",
            "Packet class has no matching PacketTypes member",
            "Packet class '{0}' has no matching PacketTypes member. Add the enum member or annotate the class with [PacketRegistrationIgnore]",
            "Multiplicity.PacketRegistry",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor MissingParameterlessConstructor = new DiagnosticDescriptor(
            "MPR002",
            "Packet class has no registry-accessible parameterless constructor",
            "Packet class '{0}' must expose a public, internal, or protected internal parameterless constructor for generated payload metadata",
            "Multiplicity.PacketRegistry",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor MissingBinaryReaderConstructor = new DiagnosticDescriptor(
            "MPR003",
            "Packet class has no registry-accessible BinaryReader constructor",
            "Packet class '{0}' must expose a public, internal, or protected internal constructor accepting System.IO.BinaryReader for generated deserialization",
            "Multiplicity.PacketRegistry",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor DuplicatePacketId = new DiagnosticDescriptor(
            "MPR004",
            "Multiple packet classes map to the same packet id",
            "Packet class '{0}' maps to packet id {1}, which is already registered by '{2}'",
            "Multiplicity.PacketRegistry",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<INamedTypeSymbol?> candidates = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => node is ClassDeclarationSyntax,
                static (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol((ClassDeclarationSyntax)ctx.Node) as INamedTypeSymbol);

            IncrementalValueProvider<(Compilation Compilation, ImmutableArray<INamedTypeSymbol?> Types)> input =
                context.CompilationProvider.Combine(candidates.Collect());

            context.RegisterSourceOutput(input, static (spc, pair) => Validate(spc, pair.Compilation, pair.Types));
        }

        private static void Validate(
            SourceProductionContext context,
            Compilation compilation,
            ImmutableArray<INamedTypeSymbol?> candidates)
        {
            INamedTypeSymbol? packetBase = compilation.GetTypeByMetadataName(PacketBaseMetadataName);
            INamedTypeSymbol? packetTypes = compilation.GetTypeByMetadataName(PacketTypesMetadataName);
            INamedTypeSymbol? ignoreAttribute = compilation.GetTypeByMetadataName(IgnoreAttributeMetadataName);
            INamedTypeSymbol? binaryReader = compilation.GetTypeByMetadataName(BinaryReaderMetadataName);

            bool prerequisitesMissing = false;
            prerequisitesMissing |= ReportMissing(context, packetBase, PacketBaseMetadataName);
            prerequisitesMissing |= ReportMissing(context, packetTypes, PacketTypesMetadataName);
            prerequisitesMissing |= ReportMissing(context, binaryReader, BinaryReaderMetadataName);

            if (prerequisitesMissing)
            {
                return;
            }

            Dictionary<string, int> packetIds = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (ISymbol member in packetTypes!.GetMembers())
            {
                if (member is IFieldSymbol field && field.HasConstantValue && field.ConstantValue is byte value)
                {
                    packetIds[field.Name] = value;
                }
            }

            HashSet<string> seenTypes = new HashSet<string>(StringComparer.Ordinal);
            Dictionary<int, string> registeredIds = new Dictionary<int, string>();

            foreach (INamedTypeSymbol? type in candidates)
            {
                if (type == null || type.IsAbstract || type.TypeKind != TypeKind.Class)
                {
                    continue;
                }

                if (!string.Equals(type.ContainingNamespace?.ToDisplayString(), PacketNamespace, StringComparison.Ordinal) ||
                    !DerivesFrom(type, packetBase!))
                {
                    continue;
                }

                string fullName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (!seenTypes.Add(fullName))
                {
                    continue;
                }

                if (ignoreAttribute != null &&
                    type.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, ignoreAttribute)))
                {
                    continue;
                }

                Location location = type.Locations.FirstOrDefault(static l => l.IsInSource) ?? Location.None;

                if (!packetIds.TryGetValue(type.Name, out int packetId))
                {
                    context.ReportDiagnostic(Diagnostic.Create(MissingPacketTypeId, location, type.ToDisplayString()));
                    continue;
                }

                if (!HasParameterlessConstructor(type))
                {
                    context.ReportDiagnostic(Diagnostic.Create(MissingParameterlessConstructor, location, type.ToDisplayString()));
                }

                if (!HasBinaryReaderConstructor(type, binaryReader!))
                {
                    context.ReportDiagnostic(Diagnostic.Create(MissingBinaryReaderConstructor, location, type.ToDisplayString()));
                }

                if (registeredIds.TryGetValue(packetId, out string? existingType))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        DuplicatePacketId,
                        location,
                        type.ToDisplayString(),
                        packetId,
                        existingType));
                }
                else
                {
                    registeredIds.Add(packetId, type.ToDisplayString());
                }
            }
        }

        private static bool ReportMissing(
            SourceProductionContext context,
            INamedTypeSymbol? symbol,
            string metadataName)
        {
            if (symbol != null)
            {
                return false;
            }

            context.ReportDiagnostic(Diagnostic.Create(MissingPrerequisite, Location.None, metadataName));
            return true;
        }

        private static bool DerivesFrom(INamedTypeSymbol type, INamedTypeSymbol packetBase)
        {
            for (INamedTypeSymbol? current = type.BaseType; current != null; current = current.BaseType)
            {
                if (SymbolEqualityComparer.Default.Equals(current, packetBase))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasParameterlessConstructor(INamedTypeSymbol type)
        {
            return type.InstanceConstructors.Any(static ctor =>
                !ctor.IsStatic &&
                ctor.Parameters.Length == 0 &&
                IsRegistryAccessible(ctor.DeclaredAccessibility));
        }

        private static bool HasBinaryReaderConstructor(INamedTypeSymbol type, INamedTypeSymbol binaryReader)
        {
            return type.InstanceConstructors.Any(ctor =>
                !ctor.IsStatic &&
                ctor.Parameters.Length == 1 &&
                SymbolEqualityComparer.Default.Equals(ctor.Parameters[0].Type, binaryReader) &&
                IsRegistryAccessible(ctor.DeclaredAccessibility));
        }

        private static bool IsRegistryAccessible(Accessibility accessibility)
        {
            return accessibility == Accessibility.Public ||
                   accessibility == Accessibility.Internal ||
                   accessibility == Accessibility.ProtectedOrInternal;
        }
    }
}
