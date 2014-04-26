using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surface.Core.Content
{
    public interface IContentReader
    {
        Type GetSourceType();
        object Read(ContentReaderContext context);
    }
}
