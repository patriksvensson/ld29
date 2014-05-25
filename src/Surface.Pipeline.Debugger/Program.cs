using System;
using Lunt;
using Lunt.Debugging;
using Surface.Pipeline.Importers;

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

            var debugger = new LuntDebugger(typeof(TextureImporter).Assembly);
            debugger.Run("../../../../assets/build.xml");
        }
    }
}
