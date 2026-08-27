; Unshipped analyzer release
; https://github.com/dotnet/roslyn-analyzers/blob/main/src/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
MPR000 | Multiplicity.PacketRegistry | Error | Packet registry generator prerequisite is missing
MPR001 | Multiplicity.PacketRegistry | Error | Packet class has no matching PacketTypes member
MPR002 | Multiplicity.PacketRegistry | Error | Packet class has no registry-accessible parameterless constructor
MPR003 | Multiplicity.PacketRegistry | Error | Packet class has no registry-accessible BinaryReader constructor
MPR004 | Multiplicity.PacketRegistry | Error | Multiple packet classes map to the same packet id
