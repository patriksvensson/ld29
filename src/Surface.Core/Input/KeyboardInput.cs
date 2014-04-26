using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Surface.Core.Input
{
    public class KeyboardInput
    {
        private KeyboardState _current;
        private KeyboardState _previous;

        public bool IsKeyDown(Keys key)
        {
            return _current.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return _current.IsKeyUp(key);
        }

        public bool WasKeyPressed(Keys key)
        {
            return _previous.IsKeyDown(key)
                && _current.IsKeyUp(key);
        }

        public Keys[] GetPressedKeys()
        {
            return _current.GetPressedKeys();
        }

        internal void Update()
        {
            _previous = _current;
            _current = Keyboard.GetState();
        }
    }
}
