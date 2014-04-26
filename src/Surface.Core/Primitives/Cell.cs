using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Surface.Core.Primitives
{
    public struct Cell : IEquatable<Cell>
    {
        public readonly int X;
        public readonly int Y;

        private static readonly Cell _empty;

        public static Cell Empty
        {
            get { return _empty; }
        }

        static Cell()
        {
            _empty = new Cell();
        }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Cell other)
        {
            return ((X == other.X) && (Y == other.Y));
        }

        public override bool Equals(object obj)
        {
            if (obj is Cell)
            {
                return Equals((Cell)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() + Y.GetHashCode());
        }

        public override string ToString()
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            return string.Format(culture, "{0} {1}",
                X.ToString(culture), Y.ToString(culture));
        }

        public static bool operator ==(Cell a, Cell b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Cell a, Cell b)
        {
            if (a.X == b.X)
            {
                return (a.Y != b.Y);
            }
            return true;
        }
    }
}
