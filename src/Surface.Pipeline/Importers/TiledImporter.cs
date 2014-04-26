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
    [LuntImporter(".tmx", DefaultProcessor = typeof(TiledProcessor))]
    public class TiledImporter : LuntImporter<TiledMap>
    {
        public override TiledMap Import(LuntContext context, IFile file)
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
