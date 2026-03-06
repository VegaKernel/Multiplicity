namespace Multiplicity.Packets.Models
{
    public sealed class Tile
    {
        public byte Header1 { get; set; }

        public byte Header2 { get; set; }

        public byte Header3 { get; set; }

        public bool IsActive { get; set; }

        public bool HasWall { get; set; }

        public bool HasLiquid { get; set; }

        public bool WireRed { get; set; }

        public bool WireGreen { get; set; }

        public bool WireBlue { get; set; }

        public bool WireYellow { get; set; }

        public bool IsHalfBrick { get; set; }

        public bool HasActuator { get; set; }

        public bool IsInActive { get; set; }

        public bool HasTileColor { get; set; }

        public bool HasWallColor { get; set; }

        public bool IsFullbrightBlock { get; set; }

        public bool IsFullbrightWall { get; set; }

        public bool IsInvisibleBlock { get; set; }

        public bool IsInvisibleWall { get; set; }

        public ushort Type { get; set; }

        public short FrameX { get; set; } = -1;

        public short FrameY { get; set; } = -1;

        public ushort Wall { get; set; }

        public byte TileColor { get; set; }

        public byte WallColor { get; set; }

        public byte Slope { get; set; }

        public byte LiquidAmount { get; set; }

        public byte LiquidType { get; set; }
    }
}
