using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Surface.Core;
using Surface.Screens;

namespace Surface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var bootstrapper = new GameEngineBootstrapper())
            {
                bootstrapper.Register(Register);
                bootstrapper.Run<GameScreen>();
            }
        }

        private static void Register(IKernel kernel)
        {
            // Register game related services here.

            kernel.Bind<GameScreen>().ToSelf().InSingletonScope();
        }
    }
}
