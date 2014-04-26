using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surface.Core.Content
{
    public abstract class ContentReader<T> : IContentReader
        where T :class
    {
        public Type GetSourceType()
        {
            return typeof (T);
        }

        public abstract T Read(ContentReaderContext context);

        object IContentReader.Read(ContentReaderContext context)
        {
            return Read(context);
        }
    }
}
