using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Lunt;
using Lunt.IO;

namespace Surface.Pipeline.Writers
{
    public sealed class TextureWriter : LuntWriter<TextureContent>
    {
        public override void Write(LuntContext context, IFile target, TextureContent value)
        {
            using (var stream = target.Open(FileMode.Create, FileAccess.Write, FileShare.None))
            {
                value.Bitmap.Save(stream, ImageFormat.Png);
            }
        }
    }
}
