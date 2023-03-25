using Common;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class ArenaManager : Singleton<ArenaManager>
    {
        public const int ArenaMapId = 5;
        public const int MaxInstance = 100;

        private Queue<int> InstanceIndexs = new Queue<int>();
        //private Dictionary<int, Arena> Arenas = new Dictionary<int, Arena>();
        internal void Init()
        {
            for(int i = 0; i < MaxInstance; i++)
            {
                InstanceIndexs.Enqueue(i);
            }
        }

        //internal Arena NewArena(ArenaInfo info, NetConnection<NetSession> red, NetConnection<NetSession> blue)
        //{
        //    var instance = InstanceIndexs.Dequeue();
        //    var map = MapManager.Instance.GetInstance(ArenaMapId, instance);
        //    Arena arena = new Arena(map, info, red, blue);
        //    this.Arenas[instance] = arena;
        //    arena.PlayerEnter();
        //    return arena;
        //}
    }
}
