using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surface.Core.Graphics;
using Surface.Core.Primitives;

namespace Surface.Core
{
    public sealed class Map
    {
        private readonly string _name;
        private readonly Size _size;
        private Tileset[] _tilesets;
        private Layer[] _layers;

        public string Name
        {
            get { return _name; }
        }

        public Size Size
        {
            get { return _size; }
        }

        public Layer[] Layers
        {
            get { return _layers; }
        }

        public Map(string name, Size size)
        {
            _name = name;
            _size = size;
            _tilesets = new Tileset[0];
            _layers = new Layer[0];
        }

        public void AddTileset(int id, Tileset tileset)
        {
            if (_tilesets.Length < id + 1)
            {
                Array.Resize(ref _tilesets, id + 1);
            }
            _tilesets[id] = tileset;
        }

        public void AddLayer(int id, Layer layer)
        {
            if (_layers.Length < id + 1)
            {
                Array.Resize(ref _layers, id + 1);
            }
            _layers[id] = layer;
        }

        public Tileset GetTileset(int id)
        {
            return _tilesets[id];
        }

        public Size GetSizeInPixels()
        {
            return new Size(_size.Width * 16, _size.Height * 16);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Tileset tile in _tilesets)
            {
                if (tile != null)
                {
                    tile.Update(gameTime);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera, Layer layer)
        {
            // Calculate the first visible cell index.
            float cameraCellX = (int)camera.Position.X / 16;
            float cameraCellY = (int)camera.Position.Y / 16;
            var firstIndex = (int)cameraCellX + (_size.Width * (int)cameraCellY);

            // Calculate stuff that we will need later.
            var horizontalCells = camera.ScreenSize.Width / 16;
            var verticalCells = camera.ScreenSize.Height / 16;
            var mapCellCount = _size.Width * _size.Height;

            for (var y = 0; y < verticalCells + 1; y++)
            {
                // How much should we increment the index position with?
                var verticalIncrement = y * _size.Width;

                for (var x = 0; x < horizontalCells + 1; x++)
                {
                    var index = firstIndex + verticalIncrement + x;

                    if (index < 0)
                    {
                        continue;
                    }

                    if (index >= mapCellCount)
                    {
                        return;
                    }

                    if (layer.Tiles[index] != null)
                    {
                        var tx = index % _size.Width * 16;
                        var ty = index / _size.Width * 16;
                        var position = new Vector2(tx, ty);
                        position = camera.Transform(position);

                        // Get the source rectangle and the texture.
                        var sourceRectangle = layer.Tiles[index].GetRectangle();
                        var texture = layer.Tiles[index].Tileset.Texture;

                        // Draw the tile. 
                        spriteBatch.Draw(texture, position, sourceRectangle, layer.Color);
                    }
                }
            }
        }

        public bool IsBlocked(Rectangle boundingBox)
        {
            foreach (var point in GetPoints(boundingBox))
            {
                var x = (int)(point.X / 16);
                var y = (int)(point.Y / 16);
                var index = (int)(x + (_size.Width * (int)y));
                foreach (var layer in _layers)
                {
                    var tile = layer.Tiles[index];
                    if (tile != null)
                    {
                        if (tile.Type == TileType.Wall)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsOnTile(Vector2 position, TileType type)
        {
            var x = (int)(position.X / 16);
            var y = (int)(position.Y / 16);
            var index = (int)(x + (_size.Width * (int)y));
            foreach (var layer in _layers)
            {
                var tile = layer.Tiles[index];
                if (tile != null)
                {
                    if (tile.Type == type)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private IEnumerable<Vector2> GetPoints(Rectangle rectangle)
        {
            yield return new Vector2(rectangle.Left, rectangle.Top);
            yield return new Vector2(rectangle.Right, rectangle.Top);
            yield return new Vector2(rectangle.Left, rectangle.Bottom);
            yield return new Vector2(rectangle.Right, rectangle.Bottom);
        }
    }
}
