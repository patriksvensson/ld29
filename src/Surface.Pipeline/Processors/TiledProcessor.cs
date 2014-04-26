using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunt;
using Lunt.Diagnostics;
using Lunt.IO;
using NTiled;
using Surface.Pipeline.Content;

namespace Surface.Pipeline.Processors
{
    public class TiledProcessor : LuntProcessor<TiledMap, MapContent>
    {
        public override MapContent Process(LuntContext context, TiledMap source)
        {
            var map = new MapContent();
            map.Width = source.Width;
            map.Height = source.Height;

            ProcessTilesets(source, map);
            ProcessLayers(source, map);

            return map;
        }

        private static void ProcessTilesets(TiledMap source, MapContent map)
        {
            var index = 0;
            foreach (var tileset in source.Tilesets)
            {
                map.Tilesets.Add(new TilesetReference
                {
                    Index = index,
                    Asset = new FilePath(string.Concat("tilesets/", System.IO.Path.GetFileNameWithoutExtension(tileset.Image.Source)))
                });
                index++;
            }
        }

        private static void ProcessLayers(TiledMap source, MapContent map)
        {
            var layerId = 0;
            for (int layerIndex = 0; layerIndex < source.Layers.Count; layerIndex++)
            {
                var layer = source.Layers[layerIndex];

                var tileLayer = layer as TiledTileLayer;
                if (tileLayer != null)
                {                    
                    map.Layers.Add(CreateTileLayer(source, map, tileLayer, layerId));
                    layerId++;
                }
            }
        }

        private static TileLayerContent CreateTileLayer(TiledMap source, MapContent map, TiledTileLayer layer, int id)
        {
            var content = new TileLayerContent();
            content.Id = id;
            content.Name = layer.Name;

            for (var gridIndex = 0; gridIndex < layer.Tiles.Length; gridIndex++)
            {
                var tileIndex = layer.Tiles[gridIndex];
                if (tileIndex == 0)
                {
                    // The tile with index 0 is reserved (nothing).
                    continue;
                }

                // Find witch tiled tileset the index belongs to...
                var tiledTileset = source.Tilesets.Where(t => t.FirstId <= tileIndex).OrderByDescending(t => t.FirstId).FirstOrDefault();
                if (tiledTileset == null)
                {
                    tiledTileset = source.Tilesets.Where(t => t.FirstId >= tileIndex).OrderByDescending(t => t.FirstId).FirstOrDefault();
                    if (tiledTileset == null)
                    {
                        throw new InvalidOperationException("Could not find tileset.");
                    }
                }

                // Find the tileset reference.
                var tileset = map.Tilesets[source.Tilesets.IndexOf(tiledTileset)];

                var tileContent = new TileContent();
                tileContent.GridIndex = gridIndex;
                tileContent.TileSetId = tileset.Index;
                tileContent.TilesetIndex = tileIndex - tiledTileset.FirstId;

                content.Tiles.Add(tileContent);
            }
            return content;
        }
    }
}
