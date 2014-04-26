using System;

namespace Surface.Core.Primitives
{
    public struct Size : IEquatable<Size>
    {
        public readonly int Width;
        public readonly int Height;

        private static readonly Size _zero;

        public static Size Zero
        {
            get { return _zero; }
        }

        static Size()
        {
            _zero = new Size();
        }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(Size other)
        {
            return ((Width == other.Width) && (Height == other.Height));
        }

        public override bool Equals(object obj)
        {
            if (obj is Size)
            {
                return Equals((Size)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Width.GetHashCode() + Height.GetHashCode());
        }

        public static bool operator ==(Size a, Size b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Size a, Size b)
        {
            if (a.Width == b.Width)
            {
                return (a.Height != b.Height);
            }
            return true;
        }
    }
}