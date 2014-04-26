using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Surface.Core
{
    public static class BinaryReaderExtensions
    {
        public static int Read7BitEncodedInteger(this BinaryReader reader)
        {
            int returnValue = 0;
            int bitIndex = 0;
            while (bitIndex != 35)
            {
                byte currentByte = reader.ReadByte();
                returnValue |= (currentByte & sbyte.MaxValue) << bitIndex;
                bitIndex += 7;

                if ((currentByte & 128) == 0)
                    return returnValue;
            }
            throw new FormatException("Bad encoded integer.");
        }
    }
}
