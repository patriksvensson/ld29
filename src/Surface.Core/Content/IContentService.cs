using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Surface.Core.Content
{
    public interface IContentService
    {
        GraphicsDevice GraphicsDevice { get; }
        T Load<T>(string asset) where T : class;
    }
}
