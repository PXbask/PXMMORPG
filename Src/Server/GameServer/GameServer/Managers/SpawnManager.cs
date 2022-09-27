using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using Common;

namespace GameServer.Managers
{
    internal class Spawner
    {
        public SpawnRuleDefine SpawnRuleDefine { get; set; }
        private Map map;
        //刷新时间
        private float spawnTime = 0;
        //消失时间
        private float unSpawnTime = 0;
        private bool spawned = false;
        public SpawnPointDefine spawnPoint = null;

        public Spawner(SpawnRuleDefine define,Map map)
        {
            this.SpawnRuleDefine = define;
            this.map = map;
            if (DataManager.Instance.SpawnPoints.ContainsKey(this.map.ID))
            {
                if (DataManager.Instance.SpawnPoints[this.map.ID].ContainsKey(this.SpawnRuleDefine.SpawnPoint))
                {
                    spawnPoint=DataManager.Instance.SpawnPoints[this.map.ID][this.SpawnRuleDefine.SpawnPoint];
                }
                else
                {
                    Log.ErrorFormat("SpawnRule[{0}] SpawnPoint[{1}] not existed", this.SpawnRuleDefine.ID, this.SpawnRuleDefine.SpawnPoint);
                }
            }
        }
        public void Update()
        {
            if (this.CanSpawn())
                this.Spawn();
        }

        private void Spawn()
        {
            this.spawned = true;
            Log.InfoFormat("Map:[{0}] Spawn[{1}:Mon:{2},Lv:{3} At point:{4}",
                this.SpawnRuleDefine.MapID,this.SpawnRuleDefine.ID,this.SpawnRuleDefine.SpawnMonID,
                this.SpawnRuleDefine.SpawnLevel,this.spawnPoint.Position.ToString());
            this.map.MonsterManager.Create(this.SpawnRuleDefine.SpawnMonID,
                this.SpawnRuleDefine.SpawnLevel, spawnPoint.Position, spawnPoint.Direction);
        }

        private bool CanSpawn()
        {
            if (this.spawned)
                return false;
            if (this.unSpawnTime + this.SpawnRuleDefine.SpawnPeriod > Time.time)
                return false;
            return true;
        }
    }
    internal class SpawnManager
    {
        private List<Spawner> Rules = new List<Spawner>();
        private Map Map;
        internal void Init(Map map)
        {
            this.Map = map;
            if (DataManager.Instance.SpawnRules.ContainsKey(map.Define.ID))
            {
                foreach(var rule in DataManager.Instance.SpawnRules[map.Define.ID].Values)
                {
                    this.Rules.Add(new Spawner(rule, this.Map));
                }
            }
        }

        internal void Update()
        {
            if (this.Rules.Count == 0)
                return;
            for(int i = 0; i < this.Rules.Count; i++)
            {
                this.Rules[i].Update();
            }
        }
    }
}
