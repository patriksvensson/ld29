using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Lunt;
using Lunt.IO;
using Surface.Core;
using Surface.Core.Primitives;
using Surface.Pipeline.Content;
using System.Drawing;

namespace Surface.Pipeline.Importers
{
    [Importer(".tileset")]
    public class TilesetImporter : Importer<TilesetContent>
    {
        public override TilesetContent Import(Context context, IFile file)
        {
            TilesetContent tileset = new TilesetContent();
            XDocument document = LoadXml(file);

            // Find the root element.
            XElement root = document.Element("tileset");
            if (root == null)
            {
                throw new InvalidOperationException("Could not find start element 'tileset' in tileset definition.");
            }

            // Read tileset information.
            tileset.Asset = root.ReadAttribute("asset", string.Empty);
            var assetPath = context.InputDirectory.Combine(new FilePath(tileset.Asset + ".png"));
            if (!assetPath.Exists(context.FileSystem))
            {
                string message = string.Format("The tileset texture '{0}' does not exist.", assetPath);
                throw new InvalidOperationException(message);
            }

            // Find out the total size of the image.
            var dimensions = ImageHelper.GetDimensions(assetPath.FullPath);
            if (dimensions.Width % 16 == 0 && dimensions.Height % 16 != 0)
            {
                const string message = "Tileset size is not dividable by 16.";
                throw new InvalidOperationException(message);
            }

            // Find the tiles element.
            XElement[] tileElements = root.Elements("tile").ToArray();
            if (tileElements.Length == 0)
            {
                string message = string.Format("The tileset '{0}' contain no tiles.", file.Path.FullPath);
                throw new InvalidOperationException(message);
            }

            // Iterate all tiles.
            ReadTiles(tileElements, tileset, dimensions);

            // Complement missing tiles.
            for (int y = 0; y < (dimensions.Height / 16); y++)
            {
                for (int x = 0; x < (dimensions.Width / 16); x++)
                {
                    int tileIndex = x + ((dimensions.Width / 16) * y);
                    if (tileset.Tiles.All(t => t.TilesetIndex != tileIndex))
                    {
                        var tile = new TilesetTileContent();
                        tile.TilesetIndex = tileIndex;
                        tile.TilesetPosition = new Cell(x, y);
                        tile.Type = TileType.Nothing;
                        tileset.Tiles.Add(tile);
                    }
                }
            }

            // Return the tileset.
            return tileset;
        }

        private void ReadTiles(IEnumerable<XElement> tileElements, TilesetContent tileset, Surface.Core.Primitives.Size dimensions)
        {
            var readTiles = new List<Cell>();
            foreach (var tileElement in tileElements)
            {
                // Get the cx position for the tile.
                var x = tileElement.ReadAttribute("x", -1);
                if (x == -1)
                {
                    throw new InvalidOperationException("Tile is missing its x position.");
                }
                // Get the cy position for the tile.
                var y = tileElement.ReadAttribute("y", -1);
                if (y == -1)
                {
                    throw new InvalidOperationException("Tile is missing its y position.");
                }
                // Get the type of the tile.
                var type = tileElement.ReadAttribute("type", string.Empty);
                if (string.IsNullOrWhiteSpace(type))
                {
                    throw new InvalidOperationException("Tile is missing its type.");
                }

                // Create the tile.
                var tile = new TilesetTileContent();
                tile.TilesetIndex = x + ((dimensions.Width / 16) * y);
                tile.TilesetPosition = new Cell(x, y);
                tile.Type = GetTileContentType(type);

                // Got frames?
                var frames = ReadFrames(tileElement);
                if (frames.Count > 0)
                {
                    tile.Frames.AddRange(frames);
                }

                // Has this tile been read before?
                if (readTiles.Any(cell => cell.X == x && cell.Y == y))
                {
                    string message = string.Format("Duplicate tile definition found in tileset ({0}, {1}).", x, y);
                    throw new InvalidOperationException(message);
                }

                // Add this tile to the list of read ones.
                readTiles.Add(new Cell(x, y));

                // Add the tile to the tileset.
                tileset.Tiles.Add(tile);
            }
        }

        private List<TilesetFrameContent> ReadFrames(XContainer root)
        {
            var frames = new List<TilesetFrameContent>();
            var frameElements = root.Elements("frame").ToArray();
            if (frameElements.Length > 0)
            {
                foreach (var frameElement in frameElements)
                {
                    var x = frameElement.ReadAttribute("x", -1);
                    if (x == -1)
                    {
                        throw new InvalidOperationException("Missing x position for frame.");
                    }
                    var y = frameElement.ReadAttribute("y", -1);
                    if (y == -1)
                    {
                        throw new InvalidOperationException("Missing y position for frame.");
                    }
                    var delay = frameElement.ReadAttribute("delay", -1);
                    if (delay == -1)
                    {
                        throw new InvalidOperationException("Missing y position for frame.");
                    }

                    var frame = new TilesetFrameContent();
                    frame.TilesetPosition = new Cell(x,y);
                    frame.Delay = delay;
                    frames.Add(frame);
                }
            }
            return frames;
        }

        private XDocument LoadXml(IFile file)
        {
            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return XDocument.Load(stream);
            }
        }

        private TileType GetTileContentType(string value)
        {
            if (value.Equals("Wall", StringComparison.OrdinalIgnoreCase))
            {
                return TileType.Wall;
            }
            if (value.Equals("Water", StringComparison.OrdinalIgnoreCase))
            {
                return TileType.Water;
            }
            if (value.Equals("Ladder", StringComparison.OrdinalIgnoreCase))
            {
                return TileType.Ladder;
            }
            else
            {
                return TileType.Nothing;
            }
        }
    }
 }
