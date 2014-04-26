using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surface.Core.Graphics;
using Surface.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surface.Core.Content.Readers
{
    public class TilesetReader : ContentReader<Tileset>
    {
        public override Tileset Read(ContentReaderContext context)
        {
            using (var reader = new BinaryReader(context.Stream))
            {
                // Read the texture.
                var asset = reader.ReadString();
                var texture = context.Content.Load<Texture2D>(asset);

                var tileset = new Tileset(texture, asset);

                var tileCount = reader.ReadInt32();
                for (var index = 0; index < tileCount; index++)
                {
                    var tilesetIndex = reader.Read7BitEncodedInteger();
                    var x = reader.Read7BitEncodedInteger();
                    var y = reader.Read7BitEncodedInteger();
                    var type = (TileType)reader.ReadByte();
                    var source = new Rectangle(x * 16, y * 16, 16, 16);

                    var frameCount = reader.Read7BitEncodedInteger();
                    Frame[] frames = new Frame[frameCount];
                    for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
                    {
                        var cell = new Cell(reader.Read7BitEncodedInteger(), reader.Read7BitEncodedInteger());
                        var delay = TimeSpan.FromMilliseconds(reader.Read7BitEncodedInteger());
                        var frame = new Frame(cell, delay);
                        frames[frameIndex] = frame;
                    }
                    var animation = frameCount > 0 ? new Animation(frames, new Size(16, 16)) : null;

                    tileset.AddTile(new Tile(tileset, type, source, tilesetIndex, animation));
                }

                return tileset;
            }
        }
    }
}
