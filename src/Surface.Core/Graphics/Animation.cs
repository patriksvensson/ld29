using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Surface.Core.Primitives;

namespace Surface.Core.Graphics
{
    public sealed class Animation
    {
        private readonly Rectangle[] _frames;
        private readonly double[] _delays;
        private readonly int _frameCount;
        private double _timeElapsed;
        private int _currentFrameIndex;

        public Animation(Frame[] frames, Size frameSize)
        {
            _frames = new Rectangle[frames.Length];
            _delays = new double[frames.Length];
            _frameCount = frames.Length;

            for (int i = 0; i < frames.Length; i++)
            {
                _frames[i] = new Rectangle(frames[i].Cell.X * frameSize.Width, frames[i].Cell.Y * frameSize.Height, frameSize.Width, frameSize.Height);
                _delays[i] = frames[i].Delay.TotalSeconds;
            }
        }

        public void Update(GameTime gameTime)
        {
            _timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            var timeToUpdate = _delays[_currentFrameIndex];
            if (_timeElapsed > timeToUpdate)
            {
                _timeElapsed -= timeToUpdate;
                if (_currentFrameIndex < _frameCount - 1)
                {
                    _currentFrameIndex ++;
                }
                else
                {
                    _currentFrameIndex = 0;
                }
            }
        }

        public Rectangle GetCurrentFrame()
        {
            return _frames[_currentFrameIndex];
        }
    }
}
