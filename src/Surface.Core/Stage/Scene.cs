using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surface.Core.Graphics;

namespace Surface.Core
{
    public class Scene
    {
        private readonly Map _map;
        private bool _contentLoaded;

        public Map Map
        {
            get { return _map; }
        }

        public Scene(Map map)
        {
            _map = map;
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            if (!_contentLoaded)
            {
                _contentLoaded = true;
            }
        }

        public void UnloadContent()
        {
            if (_contentLoaded)
            {
                _contentLoaded = false;
            }
        }

        public void Update(GameTime gameTime)
        {
            // Update the map.
            _map.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch batch, Camera camera)
        {
            // Draw the map and all entities.
            for (int layerIndex = 0; layerIndex < _map.Layers.Length; layerIndex++)
            {
                _map.Draw(gameTime, batch, camera, _map.Layers[layerIndex]);
            }
        }
    }
}
