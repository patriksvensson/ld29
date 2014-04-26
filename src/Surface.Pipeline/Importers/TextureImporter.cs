using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Lunt;
using Lunt.IO;
using Surface.Pipeline.Content;
using Surface.Pipeline.Processors;

namespace Surface.Pipeline.Importers
{
    [LuntImporter(".png", ".jpg", ".bmp", DefaultProcessor = typeof(TextureProcessor))]
    public sealed class TextureImporter : LuntImporter<TextureContent>
    {
        public override TextureContent Import(LuntContext context, IFile file)
        {
            return new TextureContent { Bitmap = new Bitmap(file.Path.FullPath, false)};
        }
    }
}
