using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surface.Core;
using Surface.Core.Content;
using Surface.Core.Graphics;
using Surface.Core.Primitives;
using Surface.Core.Stage;

namespace Surface.Entities
{
    public class Bottle : Entity
    {
        private Sprite _sprite;

        public string Message { get; set; }

        public Bottle()
        {
        }

        public override void LoadContent(IContentService content)
        {
            var texture = content.Load<Texture2D>("textures/bottle");
            _sprite = new Sprite(texture, new Size(16,16));
            _sprite.Position = this.Position;

            _sprite.Register("Botteling", new Animation(
                new[] {
                    new Frame(new Cell(0,0), TimeSpan.FromMilliseconds(75)), 
                    new Frame(new Cell(1,0), TimeSpan.FromMilliseconds(75)),
                    new Frame(new Cell(2,0), TimeSpan.FromMilliseconds(75)), 
                    new Frame(new Cell(3,0), TimeSpan.FromMilliseconds(75)),
                    new Frame(new Cell(2,0), TimeSpan.FromMilliseconds(75)), 
                    new Frame(new Cell(1,0), TimeSpan.FromMilliseconds(75))
                }, new Size(16, 16)));

            _sprite.Play("Botteling");
        }

        public override void Update(GameTime gameTime, Scene scene)
        {
            _sprite.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch, Camera camera)
        {
            _sprite.Draw(gameTime, batch, camera);
        }
    }
}
