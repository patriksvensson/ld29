using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surface.Core.Content;
using Surface.Core.Graphics;

namespace Surface.Core.Stage
{
    public abstract class Entity
    {
        private string _name;
        private Vector2 _position;

        public int Layer { get; set; }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        protected Entity()
        {
            _name = string.Empty;
            _position = Vector2.Zero;
        }

        public virtual void Initialize()
        {            
        }

        public virtual void LoadContent(IContentService content)
        {
        }

        public virtual void UnloadContent()
        {
        }

        public virtual void Update(GameTime gameTime, Scene scene)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch, Camera camera)
        {
        }
    }
}
