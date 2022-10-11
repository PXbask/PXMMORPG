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
        void OnEntityChanged(Entities.Entity entity);
        void OnEntityEvent(EntityEvent @event, int param);
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
        internal void OnEntitySync(NEntitySync data)
        {
            Entity entity = null;
            entities.TryGetValue(data.Id, out entity);
            if(entity != null)
            {
                if(data.Entity!= null)
                    entity.EntityData=data.Entity;
                if (notifiers.ContainsKey(data.Id))
                {
                    notifiers[entity.entityId].OnEntityChanged(entity);
                    notifiers[entity.entityId].OnEntityEvent(data.Event, data.Param);
                }
            }
        }
    }
}
