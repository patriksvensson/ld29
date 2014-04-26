using System.IO;

namespace Surface.Pipeline
{
    internal static class BinaryWriterExtensions
    {
        public static void Write7BitEncodedInteger(this BinaryWriter writer, int value)
        {
            uint num = (uint) value;
            while (num >= 128U)
            {
                writer.Write((byte) (num | 128U));
                num >>= 7;
            }
            writer.Write((byte) num);
        }
    }
}