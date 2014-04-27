using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Surface.Core.Stage;
using Surface.Entities;

namespace Surface
{
    public class EntityFactory : IEntityFactory
    {
        private readonly IKernel _kernel;
        private readonly IDictionary<string, Func<EntityData, Entity>> _blueprints;

        public EntityFactory(IKernel kernel)
        {
            _kernel = kernel; // Nooooo, not the dreaded service locator...
            _blueprints = new Dictionary<string, Func<EntityData, Entity>>(StringComparer.OrdinalIgnoreCase);
            _blueprints.Add("Bottle", CreateBottle);
        }

        public Entity Create(EntityData data)
        {
            if (_blueprints.ContainsKey(data.Type))
            {
                var entity = _blueprints[data.Type](data);
                entity.Name = data.Name;
                entity.Position = data.Position;
                return entity;
            }
            throw new InvalidOperationException("Unknown entity");
        }

        public Bottle CreateBottle(EntityData data)
        {
            var entity = _kernel.Get<Bottle>();
            entity.Message = data.Properties["Message"]; // yolo
            return entity;            
        }
    }
}
