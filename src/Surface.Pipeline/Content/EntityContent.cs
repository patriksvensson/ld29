using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Surface.Core.Primitives;

namespace Surface.Pipeline.Content
{
    public class EntityContent
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public Size Size { get; set; }
        public List<PropertyContent> Properties { get; set; }

        public EntityContent()
        {
            this.Properties = new List<PropertyContent>();
        }
    }

    public class PropertyContent
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
