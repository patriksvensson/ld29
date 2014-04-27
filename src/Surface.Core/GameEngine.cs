using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Surface.Core.Graphics;
using Surface.Core.Input;

namespace Surface.Core
{
    using System.Diagnostics;
    using Surface.Core.Screens;

    internal class GameEngine : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly KeyboardInput _keyboard;
        private readonly VirtualScreen _screen;
        private Action<GameEngine> _initializeCallback = e => { };

        public GameEngine(KeyboardInput keyboard, VirtualScreen screen)
        {
            _keyboard = keyboard;
            _screen = screen;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.IsFullScreen = true;            
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.ApplyChanges();

            IsMouseVisible = !_graphics.IsFullScreen;
            IsFixedTimeStep = true;
            Window.Title = "Beneath the Surface";

            Components.Add(new ScreenComponent(this));
        }

        internal void SetInitializeCallback(Action<GameEngine> callback)
        {
            _initializeCallback = callback;
        }

        protected override void Initialize()
        {
            _initializeCallback(this);
            _screen.Initialize(GraphicsDevice, 320, 240);
            base.Initialize();
        }

        #region Overrides of Game

        protected override void LoadContent()
        {
            _screen.LoadContent();
            base.LoadContent();
        }

        #endregion

        public void SetFirstScreen(Screen screen)
        {
            var component = Components.OfType<ScreenComponent>().FirstOrDefault();
            Debug.Assert(component != null, "Could not resolve screen component!");
            Debug.Assert(component.Count == 0, "A first screen has already been pushed to the stack.");
            component.Push(screen);
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                // Update keyboard.
                _keyboard.Update();

                base.Update(gameTime);   
            }            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Removed virtual screen for sub pixel movement (scaled sprites)
            _screen.BeginDraw();           
            base.Draw(gameTime);
            _screen.EndDraw();
        }
    }
}
