using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Surface.Core.Primitives
{
    public struct Frame : IEquatable<Frame>
    {
        public readonly Cell Cell;
        public readonly TimeSpan Delay;

        private static readonly Frame _empty;

        public static Frame Empty
        {
            get { return _empty; }
        }

        static Frame()
        {
            _empty = new Frame();
        }

        public Frame(Cell cell, TimeSpan delay)
        {
            this.Cell = cell;
            this.Delay = delay;
        }

        public bool Equals(Frame other)
        {
            return ((this.Cell == other.Cell) && (this.Delay == other.Delay));
        }

        public override bool Equals(object obj)
        {
            if (obj is Frame)
            {
                return Equals((Frame)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (this.Cell.GetHashCode() + this.Delay.GetHashCode());
        }

        public override string ToString()
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            return string.Format(culture, "{0} {1}", this.Cell, this.Delay);
        }

        public static bool operator ==(Frame a, Frame b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Frame a, Frame b)
        {
            if (a.Cell == b.Cell)
            {
                return (a.Delay != b.Delay);
            }
            return true;
        }
    }
}
