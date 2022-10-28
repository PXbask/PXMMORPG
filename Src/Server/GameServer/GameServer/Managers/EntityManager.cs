using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
namespace GameServer.Managers
{
    internal class EntityManager:Singleton<EntityManager>
    {
        private int idx = 0;
        public Dictionary<int,Entity> AllEntities = new Dictionary<int, Entity>();
        public Dictionary<int, List<Entity>> MapEntities = new Dictionary<int, List<Entity>>();

        public void AddEntity(int mapid,Entity entity)
        {
            entity.EntityData.Id = ++idx;
            AllEntities.Add(entity.entityId, entity);
            List<Entity> entities = null;
            if(!MapEntities.TryGetValue(mapid,out entities)){
                entities = new List<Entity>();
                MapEntities.Add(mapid, entities);
            }
            entities.Add(entity);
        }
        public void RemoveEntity(int map,Entity entity)
        {
            AllEntities.Remove(entity.entityId);
            MapEntities[map].Remove(entity);
        }
        public Entity GetEntity(int id)
        {
            Entity entity = null;
            this.AllEntities.TryGetValue(id, out entity);
            return entity;
        }
        internal Creature GetCreature(int casterId)
        {
            return this.GetEntity(casterId) as Creature;
        }
    }
}
