using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Surface.Core.Primitives;

namespace Surface.Core.Graphics
{
    public sealed class Camera
    {
        private Vector2 _position;
        private Size _screenSize;
        private Size _mapSize;
        private IPositionable _focus;

        public Size ScreenSize
        {
            get { return _screenSize; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Camera()
        {
            _position = new Vector2(0, 0);
            _screenSize = new Size(320, 240);
        }

        public void Initialize(Size size)
        {
            _mapSize = size;
            _position = new Vector2(0, 0);
        }

        public void SetFocus(IPositionable focus)
        {
            _focus = focus;
        }

        public Vector2 Transform(Vector2 position)
        {
            var transformed = position - _position;
            return new Vector2(transformed.X, transformed.Y);
        }

        public Vector2 Transform(float x, float y)
        {
            var transformed = new Vector2(x, y) - _position;
            return new Vector2(transformed.X, transformed.Y);
        }

        public Rectangle Transform(Rectangle position)
        {
            var rectPos = new Vector2(position.X, position.Y);
            var v = Transform(rectPos);
            var newRect = new Rectangle((int)v.X, (int)v.Y, position.Width, position.Height);
            return newRect;
        }

        public void Update(GameTime gameTime)
        {
            if (_focus != null)
            {
                // Update the position.
                _position.X = (_focus.Position.X) - 160;
                _position.Y = (_focus.Position.Y) - 120;

                // Clamp the X position.
                if (_position.X > _mapSize.Width - _screenSize.Width)
                {
                    _position.X = _mapSize.Width - _screenSize.Width;
                }
                if (_position.X < 0)
                {
                    _position.X = 0;
                }

                // Clamp the Y position.
                if (_position.Y > _mapSize.Height - _screenSize.Height)
                {
                    _position.Y = _mapSize.Height - _screenSize.Height;
                }
                if (_position.Y < 0)
                {
                    _position.Y = 0;
                }
            }
        }
    }
}
