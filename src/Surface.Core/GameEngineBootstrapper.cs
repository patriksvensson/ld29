using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Surface.Core.Content;
using Surface.Core.Content.Readers;
using Surface.Core.Input;
using Surface.Core.Screens;

namespace Surface.Core
{
    public sealed class GameEngineBootstrapper : IDisposable
    {
        private readonly IKernel _kernel;

        public GameEngineBootstrapper()
        {
            _kernel = new StandardKernel();
        }

        public void Dispose()
        {
            _kernel.Dispose();
        }

        public void Register(Action<IKernel> action)
        {
            action(_kernel);
        }

        public void Run<T>()
            where T : Screen
        {
            _kernel.Bind<GameEngine>().ToSelf().InSingletonScope();
            _kernel.Bind<KeyboardInput>().ToSelf().InSingletonScope();

            // TODO: Really do something about this...

            // Resolve the engine and bind the graphics device.
            var engine = _kernel.Get<GameEngine>();
            engine.SetInitializeCallback(e =>
            {
                // Since we need to register the graphics device before
                // setting the first screen, we do things this way...
                _kernel.Bind<GraphicsDevice>().ToConstant(e.GraphicsDevice);

                // Register services.
                _kernel.Bind<IContentService>().To<ContentService>().InSingletonScope();
                _kernel.Bind<IContentResolver>().To<FileSystemResolver>().InSingletonScope();
                
                // Register readers.
                _kernel.Bind<IContentReader>().To<SurfaceTextureReader>().InSingletonScope();

                // Set the initial screen.
                e.SetFirstScreen(_kernel.Get<T>());
            });
            engine.Run(GameRunBehavior.Synchronous);
        }

        private static void RegisterServicesDependentOnGraphicsDevice(GameEngine engine)
        {
        }
    }
}
