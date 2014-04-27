using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Surface.Core;
using Surface.Core.Content;
using Surface.Core.Graphics;
using Surface.Core.Input;
using Surface.Core.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Surface.Entities;

namespace Surface.Screens
{
    public sealed class GameScreen : Screen
    {
        private readonly GraphicsDevice _device;
        private readonly KeyboardInput _keyboard;
        private readonly IContentService _content;

        private SpriteBatch _batch;
        private Scene _scene;
        private Camera _camera;
        private Player _player;
        private Texture2D _playerTexture;
        private PrimitiveBatch _primitiveBatch;
        private HealthBar _healthBar;

        public GameScreen(GraphicsDevice device, KeyboardInput keyboard, IContentService content)
        {
            _device = device;
            _keyboard = keyboard;
            _content = content;
        }

        public override void Initialize()
        {
            _batch = new SpriteBatch(_device);
            _primitiveBatch = new PrimitiveBatch(_batch);
            _camera = new Camera();
            _player = new Player();
            _healthBar = new HealthBar();
        }

        public override void LoadContent()
        {
            _primitiveBatch.LoadContent();

            _scene = _content.Load<Scene>("maps/level");
            _playerTexture = _content.Load<Texture2D>("textures/player");

            _player.LoadContent(_content);

            _scene.Initialize();
            _scene.LoadContent(_content);

            _camera.SetFocus(_player);
            _camera.Initialize(_scene.Map.GetSizeInPixels());
        }

        public override void Update(GameTime gameTime)
        {
            if (_keyboard.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // OMFG...
            var direction = Vector2.Zero;
            if (_keyboard.IsKeyDown(Keys.Right))
            {
                direction.X = 1;
            }
            else if (_keyboard.IsKeyDown(Keys.Left))
            {
                direction.X = -1;
            }
            else
            {
                direction.X = 0;
            }

            if (_keyboard.IsKeyDown(Keys.Up))
            {
                var pos = new Vector2(_player.Position.X + 7, _player.Position.Y + 15);
                if (_scene.Map.IsOnTile(pos, TileType.Ladder))
                {
                    direction.Y = -1;
                    _player.IsClimbing = true;
                }
            }
            else
            {
                direction.Y = 0;
                _player.IsClimbing = false;
            }

            var wasOnGround = _player.IsOnGround;
            _player.IsOnGround = _scene.Map.IsBlocked(_player.BoundingBox);
            if (_player.IsOnGround && !wasOnGround)
            {
                // Adjust the Y position.
                _player.Position = new Vector2(_player.Position.X, (int)_player.Position.Y / 16 * 16);
            }

            _player.IsInWater = _scene.Map.IsOnTile(_player.Position, TileType.Water);
            _player.IsOnLadder = _scene.Map.IsOnTile(_player.Position, TileType.Ladder);

            _player.IsJumping = _keyboard.IsKeyDown(Keys.Space);
            _player.Update(gameTime, direction);

            _camera.Update(gameTime);
            _scene.Update(gameTime);
        }

        public override void Render(GameTime gameTime)
        {
            _device.Clear(Color.CornflowerBlue);

            _batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            // Draw scene.
            for (int layerIndex = 0; layerIndex < _scene.Map.Layers.Length; layerIndex++)
            {
                _scene.Map.Draw(gameTime, _batch, _camera, _scene.Map.Layers[layerIndex]);
                if (layerIndex == 1)
                {
                    // Draw player
                    //_batch.Draw(_playerTexture, _camera.Transform(_player.Position));
                    _player.Draw(gameTime, _batch, _camera);
                }

                // W00t...
                foreach (var entity in _scene.Entities)
                {
                    if (entity.Layer == layerIndex)
                    {
                        entity.Draw(gameTime, _batch, _camera);
                    }
                }
            }

            // Render health
            _healthBar.Draw(_primitiveBatch, _player);

            _batch.End();
        }

    }
}
