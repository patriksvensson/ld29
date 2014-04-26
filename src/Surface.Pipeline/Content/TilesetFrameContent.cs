using Surface.Core.Primitives;

namespace Surface.Pipeline.Content
{
    public class TilesetFrameContent
    {
        public Cell TilesetPosition { get; set; }
        public int Delay { get; set; }

        public TilesetFrameContent()
        {
            TilesetPosition = new Cell(-1,-1);
            Delay = 500;
        }
    }
}