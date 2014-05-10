using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lunt;
using Lunt.Debugging;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Surface.Pipeline.Importers;
using Path = System.IO.Path;

namespace Surface.Pipeline.Debugger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Surface Pipeline Debugger";
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Surface Pipeline Debugger\r\n");
            Console.ResetColor();

            var assembly = typeof(TextureImporter).Assembly;
            var bootstrapper = new DebuggerBootstrapper(assembly);

            using (var debugger = new Lunt.Debugging.Debugger(bootstrapper))
            {
                debugger.Run("../../../../assets/build.xml");
            }
        }
    }
}
