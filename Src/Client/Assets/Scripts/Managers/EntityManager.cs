using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using SkillBridge.Message;

namespace Manager
{
    interface IEntityNotify
    {
        void OnEntityRemoved();
    }
    internal class EntityManager:Singleton<EntityManager>
    {
        Dictionary<int,Entities.Entity> entities=new Dictionary<int,Entities.Entity>();
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();

        public void RegisterEntityChangeNotify(int entityid,IEntityNotify notify)
        {
            this.notifiers.Add(entityid, notify);
        }
        public void AddEntity(Entity entity)
        {
            entities.Add(entity.entityId, entity);
        }
        public void RemoveEntity(NEntity entity)
        {
            entities.Remove(entity.Id);
            if (notifiers.ContainsKey(entity.Id))
            {
                notifiers[entity.Id].OnEntityRemoved();
                notifiers.Remove(entity.Id);
            }
        }
    }
}
