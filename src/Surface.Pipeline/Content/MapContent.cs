using System.Collections.Generic;

namespace Surface.Pipeline.Content
{
    public class MapContent
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public List<TilesetReference> Tilesets { get; set; }
        public List<TileLayerContent> Layers { get; set; }

        public MapContent()
        {
            Tilesets = new List<TilesetReference>();
            Layers = new List<TileLayerContent>();
        }
    }
}