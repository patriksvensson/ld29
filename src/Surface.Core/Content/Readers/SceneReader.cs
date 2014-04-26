using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Surface.Core.Primitives;

namespace Surface.Core.Content.Readers
{
    public class SceneReader : ContentReader<Scene>
    {
        public override Scene Read(ContentReaderContext context)
        {
            using (var reader = new BinaryReader(context.Stream))
            {
                var size = new Size(reader.ReadInt32(), reader.ReadInt32());
                var mapTileCount = size.Width * size.Height;

                var map = new Map(string.Empty, size);

                // Read tilesets.
                var tilesetCount = reader.ReadInt32();
                for (int i = 0; i < tilesetCount; i++)
                {
                    var id = reader.ReadInt32();
                    var asset = reader.ReadString();
                    var tileset = context.Content.Load<Tileset>(asset);
                    map.AddTileset(id, tileset);
                }

                // Read layers.
                var layerCount = reader.ReadInt32();
                for (int i = 0; i < layerCount; i++)
                {
                    var layerId = reader.ReadInt32();
                    var layerName = reader.ReadString();
                    var layerOpacity = reader.ReadSingle();
                    var layerTileCount = reader.ReadInt32();

                    var tiles = new Tile[mapTileCount];
                    for (int j = 0; j < layerTileCount; j++)
                    {
                        int tilesetId = reader.Read7BitEncodedInteger();
                        int tilesetIndex = reader.Read7BitEncodedInteger();
                        int gridIndex = reader.Read7BitEncodedInteger();

                        // Find the tile in the tileset.
                        Tileset tileset = map.GetTileset(tilesetId);
                        Tile tile = tileset.GetTile(tilesetIndex);

                        // Add the tile to the layer.
                        tiles[gridIndex] = tile;
                    }

                    var layer = new Layer(layerId, layerName, tiles, layerOpacity);
                    map.AddLayer(layerId, layer);
                }
                return new Scene(map);
            }
        }
    }
}
