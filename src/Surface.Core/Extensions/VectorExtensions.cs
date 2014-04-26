using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Surface.Core
{
    public static class VectorExtensions
    {
        public static Vector2 ToNearestPixel(this Vector2 vector)
        {
            return new Vector2((float)Math.Floor(vector.X), (float)Math.Floor(vector.Y));
        }
    }
}
