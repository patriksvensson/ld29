using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunt;
using Lunt.Diagnostics;
using Lunt.IO;
using Microsoft.Xna.Framework;
using NTiled;
using Surface.Core.Primitives;
using Surface.Pipeline.Content;
using Path = System.IO.Path;

namespace Surface.Pipeline.Processors
{
    public class TiledProcessor : LuntProcessor<TiledMap, MapContent>
    {
        public override MapContent Process(LuntContext context, TiledMap source)
        {
            var map = new MapContent();
            map.Size = new Size(source.Width, source.Height);

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
                    Asset = new FilePath(string.Concat("tilesets/", Path.GetFileNameWithoutExtension(tileset.Image.Source)))
                });
                index++;
            }
        }

        private static void ProcessLayers(TiledMap source, MapContent map)
        {
            var layerId = -1;
            for (var layerIndex = 0; layerIndex < source.Layers.Count; layerIndex++)
            {
                var layer = source.Layers[layerIndex];

                // Is this a tile layer?
                var tileLayer = layer as TiledTileLayer;
                if (tileLayer != null)
                {
                    layerId++;
                    map.Layers.Add(CreateTileLayer(source, map, tileLayer, layerId));                    
                }

                // Is this an object layer?
                var objectGroup = layer as TiledObjectGroup;
                if (objectGroup != null)
                {
                    if (layerId == -1)
                    {
                        const string format = "The first layer cannot be an object layer ({0})";
                        var message = string.Format(format, layer.Name);
                        throw new InvalidOperationException(message);
                    }

                    var previousLayer = map.GetLayerById(layerId);
                    if (previousLayer == null)
                    {
                        const string format = "Could not find previous layer (id={0}) when merging objects.";
                        var message = string.Format(format, layerId);
                        throw new InvalidOperationException(message);                        
                    }

                    var objects = GetMapObjects(objectGroup);
                    foreach (var obj in objects)
                    {
                        previousLayer.Entities.Add(obj);
                    }
                }
            }
        }

        private static TileLayerContent CreateTileLayer(TiledMap source, MapContent map, TiledTileLayer layer, int id)
        {
            var content = new TileLayerContent();
            content.Id = id;
            content.Name = layer.Name;
            content.Opacity = layer.Opacity;

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

        private static IEnumerable<EntityContent> GetMapObjects(TiledObjectGroup layer)
        {
            var result = new List<EntityContent>();
            foreach (var obj in layer.Objects)
            {
                if (string.IsNullOrWhiteSpace(obj.Type))
                {
                    throw new InvalidOperationException("Found object without type!");
                }
                if (string.IsNullOrWhiteSpace(obj.Name))
                {
                    throw new InvalidOperationException("Found object without name!");
                }

                var content = new EntityContent();
                content.Name = obj.Name;
                content.Type = obj.Type;
                content.Position = new Vector2(obj.X, obj.Y);

                foreach (var property in obj.Properties)
                {
                    var objProperty = new PropertyContent();
                    objProperty.Name = property.Key;
                    objProperty.Value = property.Value;

                    content.Properties.Add(objProperty);
                }

                result.Add(content);
            }
            return result;
        }
    }
}
