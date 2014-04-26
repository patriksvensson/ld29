using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Surface.Core.Content
{
    public class ContentReaderContext
    {
        private readonly Stream _stream;
        private readonly GraphicsDevice _device;

        public GraphicsDevice Device
        {
            get { return _device; }
        }

        public Stream Stream
        {
            get { return _stream; }
        }

        public ContentReaderContext(Stream stream, GraphicsDevice device)
        {
            _stream = stream;
            _device = device;
        }
    }
}