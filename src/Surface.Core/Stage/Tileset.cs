using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Surface.Core
{
    public class Tileset
    {
        private readonly Texture2D _texture;
        private readonly string _asset;
        private readonly Tile[] _tiles;

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public string Asset
        {
            get { return _asset; }
        }

        public Tileset(Texture2D texture, string asset)
        {
            _asset = asset;
            _texture = texture;
            _tiles = new Tile[(texture.Width / 16) * (texture.Height / 16)];
        }

        public void AddTile(Tile tile)
        {
            _tiles[tile.TilesetIndex] = tile;
        }

        public Tile GetTile(int index)
        {
            return _tiles[index];
        }

        public void Update(GameTime gameTime)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile != null)
                {
                    tile.Update(gameTime);
                }
            }
        }
    }
}
