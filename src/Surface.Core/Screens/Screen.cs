using System;
using Microsoft.Xna.Framework;

namespace Surface.Core.Screens
{
    public abstract class Screen
    {
        private ScreenComponent _owner;
        private ScreenState _state;
        private float _transition;
        private TimeSpan _transitionOnTime;
        private TimeSpan _transitionOffTime;
        private bool _isCovered;
        private bool _isExiting;
        private bool _hasInputFocus;

        #region Properties

        public ScreenState State
        {
            get { return _state; }
        }

        public virtual bool IsPopup
        {
            get { return false; }
        }

        public virtual bool WantInput
        {
            get { return true; }
        }

        public bool HasInputFocus
        {
            get { return _hasInputFocus; }
        }

        public float Transition
        {
            get { return 1f - _transition; }
        }

        public TimeSpan TransitionOnTime
        {
            get { return _transitionOnTime; }
            protected set { _transitionOnTime = value; }
        }

        public TimeSpan TransitionOffTime
        {
            get { return _transitionOffTime; }
            protected set { _transitionOffTime = value; }
        }

        public bool IsCovered
        {
            get { return _isCovered; }
        }

        public bool IsActive
        {
            get
            {
                return _hasInputFocus && (_state == ScreenState.FadingIn
                                          || _state == ScreenState.FullyVisible);
            }
        }

        public bool IsExiting
        {
            get { return _isExiting; }
        }

        #endregion

        protected Screen()
        {
            _state = ScreenState.FadingIn;
            _isExiting = false;
            _isCovered = false;
            _hasInputFocus = false;
            _transition = 1;
            _transitionOnTime = TimeSpan.Zero;
            _transitionOffTime = TimeSpan.Zero;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Render(GameTime gameTime);

        public virtual void Initialize()
        {
        }

        public virtual void LoadContent()
        {
        }

        public virtual void UnloadContent()
        {
        }

        public void Push(Screen screen)
        {
            _owner.Push(screen);
        }

        public void Pop()
        {
            if (_transitionOffTime == TimeSpan.Zero)
            {
                _owner.Pop(this);
            }
            else
            {
                _isExiting = true;
            }
        }

        public void Exit()
        {
            _owner.Game.Exit();
        }

        internal void Prepare(ScreenComponent owner)
        {
            _owner = owner;
            _isExiting = false;
        }

        internal void UpdateGameScreen(GameTime gameTime, bool isCovered, bool otherScreenHasInput)
        {
            _isCovered = isCovered;
            _hasInputFocus = !otherScreenHasInput;

            if (_isExiting)
            {
                // If the screen is going away to die, it should transition off.
                _state = ScreenState.FadingOut;

                if (!this.UpdateTransition(gameTime, _transitionOffTime, 1))
                {
                    // When the transition finishes, remove the screen.					
                    _owner.Pop(this);
                }
            }
            else if (_isCovered)
            {
                // If the screen is covered by another, it should transition off.
                _state = this.UpdateTransition(gameTime, _transitionOffTime, 1) ? ScreenState.FadingOut : ScreenState.Hidden;
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                _state = this.UpdateTransition(gameTime, _transitionOnTime, -1) ? ScreenState.FadingIn : ScreenState.FullyVisible;
            }

            // Update the screen for real.
            this.Update(gameTime);
        }

        private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            // How much should we transition?
            float transitionDelta = (time == TimeSpan.Zero)
                ? 1 : (float) (gameTime.ElapsedGameTime.TotalMilliseconds/time.TotalMilliseconds);

            // Update the transition position.
            _transition += transitionDelta*direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (_transition <= 0)) || ((direction > 0) && (_transition >= 1)))
            {
                _transition = MathHelper.Clamp(_transition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }
    }
}