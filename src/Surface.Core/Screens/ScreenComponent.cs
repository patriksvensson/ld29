using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Surface.Core.Screens
{
    internal sealed class ScreenComponent : DrawableGameComponent
    {
        private readonly List<Screen> _screens;
        private readonly List<Screen> _screensToUpdate;
        private bool _isInitialized;

        public int Count
        {
            get { return _screens.Count; }
        }

        public ScreenComponent(GameEngine engine)
            : base(engine)
        {
            _screens = new List<Screen>();
            _screensToUpdate = new List<Screen>();
        }

        public void Push(Screen screen)
        {
            if (_screens.Contains(screen))
            {
                return;
            }

            if (_isInitialized)
            {
                screen.Prepare(this);
                screen.Initialize();
                screen.LoadContent();
            }

            _screens.Add(screen);
        }

        public void Pop(Screen screen)
        {
            if (!_screens.Contains(screen))
            {
                return;
            }

            _screens.Remove(screen);
            _screensToUpdate.Remove(screen);

            if (_isInitialized)
            {
                screen.UnloadContent();
            }

            if (_screens.Count == 0)
            {
                this.Game.Exit();
            }
        }

        public override void Initialize()
        {
            // Initialize all screens.
            foreach (Screen screen in _screens)
            {
                screen.Prepare(this);
                screen.Initialize();
            }

            // Initialize the component.
            base.Initialize();

            // We're now initialized.
            _isInitialized = true;
        }


        protected override void LoadContent()
        {
            foreach (Screen screen in _screens)
            {
                screen.LoadContent();
            }
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            foreach (Screen screen in _screens)
            {
                screen.UnloadContent();
            }
            base.UnloadContent();
        }

        public override void Update(GameTime time)
        {
            _screensToUpdate.Clear();
            _screensToUpdate.AddRange(_screens);

            bool otherScreenHasFocus = false; // TODO: !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (_screensToUpdate.Count > 0)
            {
                Screen screen = _screensToUpdate[_screensToUpdate.Count - 1];
                _screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

                screen.UpdateGameScreen(time, coveredByOtherScreen, !otherScreenHasFocus);

                if (screen.State == ScreenState.FadingIn ||
                    screen.State == ScreenState.FullyVisible)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    // Only do this if the screen actually want input.
                    if (!otherScreenHasFocus && screen.WantInput)
                    {
                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                    {
                        coveredByOtherScreen = true;
                    }
                }
            }
        }

        public override void Draw(GameTime time)
        {
            foreach (Screen screen in _screens)
            {
                if (screen.State != ScreenState.Hidden)
                {
                    screen.Render(time);
                }
            }
        }
    }
}