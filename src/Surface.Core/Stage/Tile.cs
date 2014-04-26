using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Surface.Core.Graphics;

namespace Surface.Core
{
    public sealed class Tile
    {
        private readonly Tileset _tileset;
        private readonly TileType _type;
        private readonly Rectangle _source;
        private readonly int _tilesetIndex;
        private readonly Animation _animation;

        public Tileset Tileset
        {
            get { return _tileset; }
        }

        public int TilesetIndex
        {
            get { return _tilesetIndex; }
        }

        public TileType Type
        {
            get { return _type; }
        }

        public Tile(Tileset tileset, TileType type, Rectangle source, int tilesetIndex, Animation animation)
        {
            _tileset = tileset;
            _type = type;
            _source = source;
            _tilesetIndex = tilesetIndex;
            _animation = animation;
        }

        public Rectangle GetRectangle()
        {
            if (_animation != null)
            {
                return _animation.GetCurrentFrame();
            }
            return _source;
        }

        public void Update(GameTime gameTime)
        {
            if (_animation != null)
            {
                _animation.Update(gameTime);
            }
        }
    }
}
