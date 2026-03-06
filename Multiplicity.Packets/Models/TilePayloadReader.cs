using System;
using System.IO;
using Multiplicity.Packets.Views;

namespace Multiplicity.Packets.Models
{
    public static class TilePayloadReader
    {
        public static Tile Read(BinaryReader reader, Func<ushort, bool> isFrameImportant)
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(isFrameImportant);

            Tile tile = new Tile
            {
                Header1 = reader.ReadByte(),
                Header2 = reader.ReadByte(),
                Header3 = reader.ReadByte()
            };

            PopulateTileFlags(tile);

            if (tile.HasTileColor)
            {
                tile.TileColor = reader.ReadByte();
            }

            if (tile.HasWallColor)
            {
                tile.WallColor = reader.ReadByte();
            }

            if (tile.IsActive)
            {
                tile.Type = reader.ReadUInt16();
                if (isFrameImportant(tile.Type))
                {
                    tile.FrameX = reader.ReadInt16();
                    tile.FrameY = reader.ReadInt16();
                }

                tile.Slope = ReadSlope(tile.Header2);
            }

            if (tile.HasWall)
            {
                tile.Wall = reader.ReadUInt16();
            }

            if (tile.HasLiquid)
            {
                tile.LiquidAmount = reader.ReadByte();
                tile.LiquidType = reader.ReadByte();
            }

            return tile;
        }

        public static Tile Read(ref PacketReader reader, Func<ushort, bool> isFrameImportant)
        {
            ArgumentNullException.ThrowIfNull(isFrameImportant);

            Tile tile = new Tile
            {
                Header1 = reader.ReadByte(),
                Header2 = reader.ReadByte(),
                Header3 = reader.ReadByte()
            };

            PopulateTileFlags(tile);

            if (tile.HasTileColor)
            {
                tile.TileColor = reader.ReadByte();
            }

            if (tile.HasWallColor)
            {
                tile.WallColor = reader.ReadByte();
            }

            if (tile.IsActive)
            {
                tile.Type = reader.ReadUInt16();
                if (isFrameImportant(tile.Type))
                {
                    tile.FrameX = reader.ReadInt16();
                    tile.FrameY = reader.ReadInt16();
                }

                tile.Slope = ReadSlope(tile.Header2);
            }

            if (tile.HasWall)
            {
                tile.Wall = reader.ReadUInt16();
            }

            if (tile.HasLiquid)
            {
                tile.LiquidAmount = reader.ReadByte();
                tile.LiquidType = reader.ReadByte();
            }

            return tile;
        }

        public static Tile[,] ReadRectangle(BinaryReader reader, int width, int height, Func<ushort, bool> isFrameImportant)
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(isFrameImportant);
            ValidateDimensions(width, height);

            Tile[,] tiles = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = Read(reader, isFrameImportant);
                }
            }

            return tiles;
        }

        public static Tile[,] ReadRectangle(ref PacketReader reader, int width, int height, Func<ushort, bool> isFrameImportant)
        {
            ArgumentNullException.ThrowIfNull(isFrameImportant);
            ValidateDimensions(width, height);

            Tile[,] tiles = new Tile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = Read(ref reader, isFrameImportant);
                }
            }

            return tiles;
        }

        private static void PopulateTileFlags(Tile tile)
        {
            byte header1 = tile.Header1;
            byte header2 = tile.Header2;
            byte header3 = tile.Header3;

            tile.IsActive = (header1 & (1 << 0)) != 0;
            tile.HasWall = (header1 & (1 << 2)) != 0;
            tile.HasLiquid = (header1 & (1 << 3)) != 0;
            tile.WireRed = (header1 & (1 << 4)) != 0;
            tile.IsHalfBrick = (header1 & (1 << 5)) != 0;
            tile.HasActuator = (header1 & (1 << 6)) != 0;
            tile.IsInActive = (header1 & (1 << 7)) != 0;

            tile.WireGreen = (header2 & (1 << 0)) != 0;
            tile.WireBlue = (header2 & (1 << 1)) != 0;
            tile.HasTileColor = (header2 & (1 << 2)) != 0;
            tile.HasWallColor = (header2 & (1 << 3)) != 0;
            tile.WireYellow = (header2 & (1 << 7)) != 0;

            tile.IsFullbrightBlock = (header3 & (1 << 0)) != 0;
            tile.IsFullbrightWall = (header3 & (1 << 1)) != 0;
            tile.IsInvisibleBlock = (header3 & (1 << 2)) != 0;
            tile.IsInvisibleWall = (header3 & (1 << 3)) != 0;
        }

        private static byte ReadSlope(byte header2)
        {
            byte slope = 0;
            if ((header2 & (1 << 4)) != 0)
            {
                slope += 1;
            }

            if ((header2 & (1 << 5)) != 0)
            {
                slope += 2;
            }

            if ((header2 & (1 << 6)) != 0)
            {
                slope += 4;
            }

            return slope;
        }

        private static void ValidateDimensions(int width, int height)
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }
        }
    }
}
