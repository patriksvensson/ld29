using System.Collections.Generic;
using Surface.Core;
using Surface.Core.Primitives;

namespace Surface.Pipeline.Content
{
    public class TilesetTileContent
    {
        public int TilesetIndex { get; set; }
        public Cell TilesetPosition { get; set; }
        public TileType Type { get; set; }
        public List<TilesetFrameContent> Frames { get; set; }

        public TilesetTileContent()
        {
            Frames = new List<TilesetFrameContent>();
        }
    }
}