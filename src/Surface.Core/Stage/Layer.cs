using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Surface.Core
{
    public sealed class Layer
    {
        private readonly int _id;
        private readonly string _name;
        private readonly Tile[] _tiles;
        private readonly Color _color;

        public int Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public Tile[] Tiles
        {
            get { return _tiles; }
        }

        public Color Color
        {
            get { return _color; }
        }

        public Layer(int id, string name, Tile[] tiles, float opacity)
        {
            _id = id;
            _name = name;
            _tiles = tiles;
            _color = new Color(Color.White, opacity);
        }
    }
}
