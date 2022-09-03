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
        public List<Entity> AllEntities = new List<Entity>();
        public Dictionary<int, List<Entity>> MapEntities = new Dictionary<int, List<Entity>>();

        public void AddEntity(int mapid,Entity entity)
        {
            AllEntities.Add(entity);
            entity.EntityData.Id = ++idx;
            List<Entity> entities = null;
            if(!MapEntities.TryGetValue(mapid,out entities)){
                entities = new List<Entity>();
                MapEntities.Add(mapid, entities);
            }
            entities.Add(entity);
        }
        public void RemoveEntity(int map,Entity entity)
        {
            AllEntities.Remove(entity);
            MapEntities[map].Remove(entity);
        }
    }
}
