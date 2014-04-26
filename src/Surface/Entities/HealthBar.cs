using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Surface.Core.Graphics;

namespace Surface.Entities
{
    public class HealthBar
    {
        private readonly Rectangle _base;

        public HealthBar()
        {
            _base = new Rectangle(10, 10, 11, 101);
        }

        public void Draw(PrimitiveBatch batch, Player player)
        {
            batch.DrawRectangle(_base, Color.White, 1);
            var health = new Rectangle(11, 11, 10, (int) player.GillPower);
            batch.DrawFilledRectangle(health, Color.Blue);
        }
    }
}
