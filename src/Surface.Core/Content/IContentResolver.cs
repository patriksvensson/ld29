using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Surface.Core.Content
{
    public interface IContentResolver
    {
        bool Exist(string asset);
        Stream GetStream(string asset);
    }
}
