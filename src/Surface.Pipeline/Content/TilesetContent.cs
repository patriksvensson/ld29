using System.Collections.Generic;

namespace Surface.Pipeline.Content
{
    public class TilesetContent
    {
        public string Asset { get; set; }
        public List<TilesetTileContent> Tiles { get; set; }

        public TilesetContent()
        {
            Tiles = new List<TilesetTileContent>();
        }
    }
}