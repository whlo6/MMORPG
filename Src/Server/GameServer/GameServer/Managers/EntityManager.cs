using Common;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class EntityManager :Singleton<EntityManager>
    {
        private int idx = 0;
        public List<Entity> AllEntities = new List<Entity>();
        public Dictionary<int, List<Entity>> MapEntities = new Dictionary<int, List<Entity>>();

        public void AddEntity(int mapId,Entity entity)
        {
            AllEntities.Add(entity);

            entity.EntityData.Id = ++this.idx;

            List<Entity> entities = null;
            if(!MapEntities.TryGetValue(mapId,out entities))
            {
                entities = new List<Entity>();
                MapEntities[mapId] = entities;
            }
            entities.Add(entity);
        }

        public void RemoveEntity(int mapId,Entity entity)
        {
            Log.InfoFormat("EntityManager::RemoveEntity:mapId[{0}],entityId:[{1}]", mapId, entity.entityId);
            this.AllEntities.Remove(entity);

            if(this.MapEntities.ContainsKey(mapId))
               this.MapEntities[mapId].Remove(entity);
        }
    }
}
