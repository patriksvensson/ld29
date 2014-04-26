using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Surface.Core.Content.Readers
{
    public sealed class SurfaceTextureReader : ContentReader<Texture2D>
    {
        public override Texture2D Read(ContentReaderContext context)
        {
            return Texture2D.FromStream(context.Device, context.Stream);
        }
    }
}
