using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Surface.Core.Primitives;
using Surface.Core.Stage;

namespace Surface.Core.Content.Readers
{
    public class SceneReader : ContentReader<Scene>
    {
        private readonly IEntityFactory _factory;

        public SceneReader(IEntityFactory factory)
        {
            _factory = factory;
        }

        public override Scene Read(ContentReaderContext context)
        {
            using (var reader = new BinaryReader(context.Stream))
            {
                var size = new Size(reader.ReadInt32(), reader.ReadInt32());
                var mapTileCount = size.Width * size.Height;

                var map = new Map(string.Empty, size);
                var entities = new List<Entity>();

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

                    var entityCount = reader.ReadInt32();
                    for (int n = 0; n < entityCount; n++)
                    {
                        var data = new EntityData();
                        data.Name = reader.ReadString();
                        data.Type = reader.ReadString();
                        data.Position = new Vector2(reader.ReadSingle(), reader.ReadSingle());
                        data.Size = new Size(reader.ReadInt32(), reader.ReadInt32());

                        var propertyCount = reader.ReadInt32();
                        for (int m = 0; m < propertyCount; m++)
                        {
                            var propKey = reader.ReadString();
                            var propValue = reader.ReadString();
                            data.Properties.Add(propKey, propValue);
                        }

                        var entity = _factory.Create(data);
                        entity.Layer = layerId;
                        entities.Add(entity);
                    }

                    var layer = new Layer(layerId, layerName, tiles, layerOpacity);
                    map.AddLayer(layerId, layer);
                }

                return new Scene(map, entities);
            }
        }
    }
}
