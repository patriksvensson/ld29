using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surface.Core.Content;
using Surface.Core.Graphics;
using Surface.Core.Primitives;

namespace Surface.Entities
{
    public class Player : IPositionable
    {
        private Vector2 _velocity;
        private readonly Vector2 _gravity;
        private readonly Vector2 _waterGravity;
        private Vector2 _position;
        private Vector2 _direction;
        private Vector2 _jumpForce;
        private float _gillPower; // pun intended
        private float _jumpCutOff;

        private Sprite _sprite;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)_position.X + 4, (int)_position.Y + 10, 8, 6); }
        }

        public bool IsOnGround { get; set; }
        public bool IsJumping { get; set; }
        public bool IsInWater { get; set; }
        public bool IsOnLadder { get; set; }
        public bool IsClimbing { get; set; }

        public float GillPower
        {
            get { return _gillPower; }
        }

        public Player()
        {
            _position = new Vector2(1100, 271);
            _gravity = new Vector2(0, 150);
            _waterGravity = new Vector2(0, 50);
            _jumpForce = new Vector2(0, -150);
            _jumpCutOff = -180f;
            _gillPower = 100f;
        }

        public void LoadContent(IContentService content)
        {
            var texture = content.Load<Texture2D>("textures/playersheet");
            _sprite = new Sprite(texture, new Size(16, 16));

            _sprite.Register("Walk-Right", new Animation(
                new[] {
                    new Frame(new Cell(0,0), TimeSpan.FromMilliseconds(75)), 
                    new Frame(new Cell(1,0), TimeSpan.FromMilliseconds(75)), 
                    new Frame(new Cell(2,0), TimeSpan.FromMilliseconds(75)), 
                    new Frame(new Cell(1,0), TimeSpan.FromMilliseconds(75)), 
                }, new Size(16, 16)));

            _sprite.Register("Walk-Left", new Animation(
                new[] {
                    new Frame(new Cell(0,1), TimeSpan.FromMilliseconds(75)), 
                    new Frame(new Cell(1,1), TimeSpan.FromMilliseconds(75)), 
                    new Frame(new Cell(2,1), TimeSpan.FromMilliseconds(75)), 
                    new Frame(new Cell(1,1), TimeSpan.FromMilliseconds(75)), 
                }, new Size(16, 16)));

            _sprite.Register("Idle-Right", new Animation(
                new[] {
                    new Frame(new Cell(0,2), TimeSpan.FromMilliseconds(150)), 
                    new Frame(new Cell(1,2), TimeSpan.FromMilliseconds(150)), 
                    new Frame(new Cell(2,2), TimeSpan.FromMilliseconds(150)), 
                    new Frame(new Cell(1,2), TimeSpan.FromMilliseconds(150)), 
                }, new Size(16, 16)));

            _sprite.Register("Idle-Left", new Animation(
                new[] {
                    new Frame(new Cell(0,3), TimeSpan.FromMilliseconds(150)), 
                    new Frame(new Cell(1,3), TimeSpan.FromMilliseconds(150)), 
                    new Frame(new Cell(2,3), TimeSpan.FromMilliseconds(150)), 
                    new Frame(new Cell(1,3), TimeSpan.FromMilliseconds(150)), 
                }, new Size(16, 16)));

            _sprite.Register("Climb", new Animation(
                new[] {
                    new Frame(new Cell(0,4), TimeSpan.FromMilliseconds(150)), 
                    new Frame(new Cell(1,4), TimeSpan.FromMilliseconds(150)), 
                    new Frame(new Cell(2,4), TimeSpan.FromMilliseconds(150)), 
                    new Frame(new Cell(1,4), TimeSpan.FromMilliseconds(150)), 
                }, new Size(16, 16)));
        }

        public void Update(GameTime gameTime, Vector2 direction)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate the applied gravity.
            var appliedGravity = (IsInWater ? _waterGravity : _gravity) * dt;
            _velocity += appliedGravity;

            if (IsOnLadder && IsClimbing)
            {
                _velocity = Vector2.Zero;
            }
            else
            {

                if (IsOnGround)
                {
                    // No velocity on the ground.
                    _velocity.Y = 0;

                    if (IsJumping)
                    {
                        _velocity += _jumpForce;
                    }
                }
            }

            var appliedDirection = direction * 128 * dt;
            if (direction.X > 0)
            {
                _sprite.Play("Walk-Right");
            }
            else if (direction.X < 0)
            {
                _sprite.Play("Walk-Left");
            }
            else
            {
                if (IsOnLadder && IsClimbing)
                {
                    _sprite.Play("Climb");
                }
                else
                {
                    _sprite.Play("Idle-Left");   
                }                
            }

            if (IsInWater)
            {                
                _gillPower += 8 * dt;
                _gillPower = Math.Min(100f, _gillPower);
            }
            else
            {
                // Lose one point for each second on land.
                _gillPower -= 2 * dt;
                _gillPower = Math.Max(0f, _gillPower);
            }

            // Update the position.
            _position += _velocity * dt + appliedDirection;
            _sprite.Position = _position;
            _sprite.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch batch, Camera camera)
        {
            _sprite.Draw(gameTime, batch, camera);
        }
    }
}
