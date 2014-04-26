using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Surface.Core.Content
{
    internal sealed class ContentService : IContentService
    {
        private readonly GraphicsDevice _device;
        private readonly IContentResolver _resolver;
        private readonly Dictionary<Type, IContentReader> _readers;

        public GraphicsDevice GraphicsDevice
        {
            get { return _device; }
        }

        public ContentService(GraphicsDevice device, IContentResolver resolver, IEnumerable<IContentReader> readers)
        {
            _device = device;
            _resolver = resolver;
            _readers = readers.ToDictionary(r => r.GetSourceType());
        }

        public T Load<T>(string asset) where T : class
        {
            var path = Path.ChangeExtension(asset, ".dat");
            if (!_resolver.Exist(path))
            {
                throw new FileNotFoundException("The asset could not be found.", asset);
            }
            using (var stream = _resolver.GetStream(path))
            {
                var context = new ContentReaderContext(stream, _device);
                return (T) _readers[typeof (T)].Read(context); // lol
            }
        }
    }
}
