using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surface.Core.Graphics
{
    internal sealed class VirtualScreen : IVirtualScreen
    {
        private int _width;
        private int _height;
        private RenderTarget2D _renderTarget;
        private GraphicsDevice _device;
        private Texture2D _backBuffer;
        private SpriteBatch _batch;

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public void Initialize(GraphicsDevice device, int width, int height)
        {
            _device = device;
            _width = width;
            _height = height;
        }

        public void LoadContent()
        {
            _renderTarget = new RenderTarget2D(_device, _width, _height);
            _batch = new SpriteBatch(_device);
        }

        public void BeginDraw()
        {
            _device.SetRenderTarget(_renderTarget);
        }

        public void EndDraw()
        {
            _device.SetRenderTarget(null);
            _backBuffer = (Texture2D) _renderTarget;
            _batch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            _batch.Draw(_backBuffer, new Rectangle(0,0, _device.Viewport.Width, _device.Viewport.Height), Color.White);
            _batch.End();
        }
    }
}
