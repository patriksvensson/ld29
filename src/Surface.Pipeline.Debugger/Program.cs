using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lunt;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;
using Surface.Pipeline.Importers;

namespace Surface.Pipeline.Debugger
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileSystem = new FileSystem();
            var environment = new BuildEnvironment(fileSystem);
            var log = new ConsoleLog();

            var reader = new BuildConfigurationXmlReader(fileSystem);
            var configuration = reader.Read("../../../../assets/build.xml");

            var workingDirectory = environment.GetWorkingDirectory();

            configuration.Incremental = false;
            configuration.InputDirectory = workingDirectory.Combine(new DirectoryPath("../../../../assets"));
            configuration.OutputDirectory = System.IO.Path.GetFullPath("output"); // relative to bin

            var scanner = new PipelineAssemblyScanner(log, typeof (TextureImporter).Assembly);
            var components = new PipelineComponentCollection(scanner);

            var engine = new BuildEngine(environment, components, new HashComputer(), log);
            var result = engine.Build(configuration);
        }
    }

    public class ConsoleLog : IBuildLog
    {
        public void Write(Verbosity verbosity, LogLevel level, string message)
        {
            Console.WriteLine(message);
        }
    }
}
