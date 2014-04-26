using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunt;
using Lunt.Diagnostics;
using Lunt.IO;
using Surface.Pipeline.Content;

namespace Surface.Pipeline.Writers
{
    public sealed class MapWriter : LuntWriter<MapContent>
    {
        public override void Write(LuntContext context, IFile target, MapContent value)
        {
            context.Log.Verbose("Writing map...");
        }
    }
}
