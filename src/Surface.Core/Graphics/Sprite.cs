using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surface.Core.Primitives;

namespace Surface.Core.Graphics
{
    public sealed class Sprite
    {
        private readonly Texture2D _texture;
        private readonly Size _size;
        private readonly Dictionary<string, Animation> _animations;
        private Animation _currentAnimation;
        private Vector2 _position;

        public Sprite(Texture2D texture, Size size)
        {
            _animations = new Dictionary<string, Animation>();
            _texture = texture;
            _size = size;
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public void Register(string key, Animation animation)
        {
            if (_animations.ContainsKey(key))
            {
                throw new InvalidOperationException("Animation key already registered.");
            }
            _animations.Add(key, animation);
        }

        public void Play(string key)
        {
            if (key != null)
            {
                if (!_animations.ContainsKey(key))
                {
                    throw new InvalidOperationException("Unknown animation");
                }
                _currentAnimation = _animations[key];
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_currentAnimation != null)
            {
                _currentAnimation.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch batch, Camera camera)
        {
            var position = camera.Transform(_position);
            if (_currentAnimation != null)
            {
                batch.Draw(_texture, position, _currentAnimation.GetCurrentFrame(),
                    Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
