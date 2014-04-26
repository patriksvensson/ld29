using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Surface.Core.Content
{
    internal sealed class FileSystemResolver : IContentResolver
    {
        private readonly string _root;

        public FileSystemResolver()
        {
            _root = Path.GetDirectoryName(typeof (FileSystemResolver).Assembly.Location);
        }

        public bool Exist(string asset)
        {
            var path = Path.Combine(_root, "data", asset);
            return File.Exists(path);
        }

        public Stream GetStream(string asset)
        {
            var path = Path.Combine(_root, "data", asset);
            Debug.Assert(File.Exists(path), "But you promised...");
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}
