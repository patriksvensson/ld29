using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Surface.Core.Content;
using Surface.Core.Input;
using Surface.Core.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surface.Screens
{
    public sealed class GameScreen : Screen
    {
        private readonly GraphicsDevice _device;
        private readonly KeyboardInput _keyboard;
        private readonly IContentService _content;
        
        private SpriteBatch _batch;
        private Texture2D _texture;

        public GameScreen(GraphicsDevice device, KeyboardInput keyboard, IContentService content)
        {
            _device = device;
            _keyboard = keyboard;
            _content = content;
        }

        public override void Initialize()
        {
            _batch = new SpriteBatch(_device);
        }

        public override void LoadContent()
        {
            _texture = _content.Load<Texture2D>("textures/tilesets/lol.png");
        }

        public override void Update(GameTime gameTime)
        {
            if (_keyboard.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
        }

        public override void Render(GameTime gameTime)
        {
            _device.Clear(Color.CornflowerBlue);

            _batch.Begin();
            _batch.Draw(_texture, new Vector2(10,10));
            _batch.End();
        }
    }
}
