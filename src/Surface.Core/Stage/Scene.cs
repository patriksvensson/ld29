using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surface.Core.Content;
using Surface.Core.Graphics;
using Surface.Core.Stage;

namespace Surface.Core
{
    public class Scene
    {
        private readonly Map _map;
        private bool _contentLoaded;
        private readonly List<Entity> _entities;

        public Map Map
        {
            get { return _map; }
        }

        public List<Entity> Entities
        {
            get { return _entities; }
        }

        public Scene(Map map, IEnumerable<Entity> entities)
        {
            _map = map;
            _entities = new List<Entity>(entities);
        }

        public void Initialize()
        {
        }

        public void LoadContent(IContentService content)
        {
            if (!_contentLoaded)
            {
                foreach (var entity in _entities)
                {
                    entity.LoadContent(content);
                }
                _contentLoaded = true;
            }
        }

        public void UnloadContent()
        {
            if (_contentLoaded)
            {
                foreach (var entity in _entities)
                {
                    entity.UnloadContent();
                }
                _contentLoaded = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            // Update the map.
            _map.Update(gameTime);

            foreach (var entity in _entities)
            {
                entity.Update(gameTime, this);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch batch, Camera camera)
        {
            // TODO: Should use a quad tree (or similar) here to reduce drawn entities.

            // Draw the map and all entities.
            for (int layerIndex = 0; layerIndex < _map.Layers.Length; layerIndex++)
            {
                _map.Draw(gameTime, batch, camera, _map.Layers[layerIndex]);
            }
        }
    }
}
