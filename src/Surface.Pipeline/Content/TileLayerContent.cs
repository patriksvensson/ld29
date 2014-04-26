using System.Collections.Generic;

namespace Surface.Pipeline.Content
{
    public sealed class TileLayerContent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TileContent> Tiles { get; set; }

        public TileLayerContent()
        {
            Tiles = new List<TileContent>();
        }
    }
}