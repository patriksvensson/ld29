using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Surface.Core.Content
{
    public class ContentReaderContext
    {
        private readonly Stream _stream;
        private readonly GraphicsDevice _device;
        private readonly IContentService _content;

        public GraphicsDevice Device
        {
            get { return _device; }
        }

        public Stream Stream
        {
            get { return _stream; }
        }

        public IContentService Content
        {
            get { return _content; }
        }

        public ContentReaderContext(Stream stream, GraphicsDevice device, IContentService content)
        {
            _stream = stream;
            _device = device;
            _content = content;
        }
    }
}