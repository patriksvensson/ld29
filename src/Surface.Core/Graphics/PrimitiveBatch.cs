using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Surface.Core.Graphics
{
    public sealed class PrimitiveBatch
    {
        private readonly SpriteBatch _batch;
        static Texture2D _pointTexture;

        public PrimitiveBatch(SpriteBatch batch)
        {
            _batch = batch;
        }

        public void LoadContent()
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(_batch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }            
        }

        public void DrawRectangle(Rectangle rectangle, Color color, int lineWidth)
        {
            Debug.Assert(_pointTexture != null, "No texture!");
            _batch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            _batch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            _batch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            _batch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }

        public void DrawFilledRectangle(Rectangle rectangle, Color color)
        {
            _batch.Draw(_pointTexture, rectangle, color);
        }
    }
}
