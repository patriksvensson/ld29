using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Lunt;
using Lunt.IO;
using NTiled;
using Surface.Pipeline.Processors;

namespace Surface.Pipeline.Importers
{
    [Importer(".tmx", DefaultProcessor = typeof(TiledProcessor))]
    public class TiledImporter : Importer<TiledMap>
    {
        public override TiledMap Import(Context context, IFile file)
        {
            var reader = new TiledReader();
            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var doc = XDocument.Load(stream);
                return reader.Read(doc);   
            }            
        }
    }
}
