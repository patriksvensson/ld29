using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunt;
using Surface.Pipeline.Content;

namespace Surface.Pipeline.Processors
{
    public sealed class TextureProcessor : LuntProcessor<TextureContent, TextureContent>
    {
        public override TextureContent Process(LuntContext context, TextureContent source)
        {
            if (context.Asset.Metadata.IsDefined("Scale"))
            {
                var scale = Convert.ToInt32(context.Asset.Metadata.GetValue("Scale")); // yolo
                return new TextureContent {
                    Bitmap = ScaleBitmap(source.Bitmap, (float) scale)
                };
            }
            return source;
        }

        // http://stackoverflow.com/a/2683538
        private static Bitmap ScaleBitmap(Bitmap bitmap, float scaleFactor)
        {
            var height = (int)((float)bitmap.Size.Height * scaleFactor);
            var width = (int)((float)bitmap.Size.Width * scaleFactor);
            var newBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage((Image)newBitmap))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.ScaleTransform(scaleFactor, scaleFactor);

                var rectangle = new Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height);
                g.DrawImage(bitmap, rectangle, rectangle, GraphicsUnit.Pixel);
            }
            return (newBitmap);
        }
    }
}
