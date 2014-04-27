using System.Collections.Generic;

namespace Surface.Pipeline.Content
{
    public abstract class LayerContent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Opacity { get; set; }
        public List<EntityContent> Entities { get; set; }

        public LayerContent()
        {
            Entities = new List<EntityContent>();
        }
    }
}