using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Surface.Core.Primitives;

namespace Surface.Core.Stage
{
    public interface IEntityFactory
    {
        Entity Create(EntityData data);
    }

    public class EntityData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public Size Size { get; set; }
        public IDictionary<string, string> Properties { get; set; }

        public EntityData()
        {
            Properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
