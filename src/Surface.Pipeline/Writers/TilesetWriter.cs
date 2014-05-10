using System.IO;
using Lunt;
using Lunt.IO;
using Surface.Pipeline;
using Surface.Pipeline.Content;

namespace Surface.Pipeline.Writers
{
    public sealed class TilesetWriter : Writer<TilesetContent>
    {
        public override void Write(Context context, IFile target, TilesetContent value)
        {
            using (var stream = target.Open(FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new BinaryWriter(stream))
            {
                // Write the asset name.
                writer.Write(value.Asset);

                // Write all tiles.
                writer.Write(value.Tiles.Count);
                foreach (var tile in value.Tiles)
                {
                    writer.Write7BitEncodedInteger(tile.TilesetIndex);
                    writer.Write7BitEncodedInteger(tile.TilesetPosition.X);
                    writer.Write7BitEncodedInteger(tile.TilesetPosition.Y);
                    writer.Write((byte) tile.Type);

                    // Write the frames as well.
                    writer.Write7BitEncodedInteger(tile.Frames.Count);
                    foreach (var frame in tile.Frames)
                    {
                        writer.Write7BitEncodedInteger(frame.TilesetPosition.X);
                        writer.Write7BitEncodedInteger(frame.TilesetPosition.Y);
                        writer.Write7BitEncodedInteger(frame.Delay);
                    }
                }
            }
        }
    }
}