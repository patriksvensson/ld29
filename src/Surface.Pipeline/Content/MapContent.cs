using System.Collections.Generic;
using Surface.Core.Primitives;

namespace Surface.Pipeline.Content
{
    public class MapContent
    {
        public Size Size { get; set; }

        public List<TilesetReference> Tilesets { get; set; }
        public List<TileLayerContent> Layers { get; set; }

        public MapContent()
        {
            Tilesets = new List<TilesetReference>();
            Layers = new List<TileLayerContent>();
        }
    }
}