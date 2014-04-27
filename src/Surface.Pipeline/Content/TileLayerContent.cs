using System.Collections.Generic;

namespace Surface.Pipeline.Content
{
    public sealed class TileLayerContent : LayerContent
    {
        public List<TileContent> Tiles { get; set; }

        public TileLayerContent()
        {
            Tiles = new List<TileContent>();
        }
    }
}