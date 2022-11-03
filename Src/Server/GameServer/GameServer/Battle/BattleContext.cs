using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    class BattleContext
    {
        public Battle Battle { get; set; }
        public Creature Caster { get; set; }
        public Creature Target { get; set; }
        public Core.Vector3Int Position { get; set; }
        public NSkillCastInfo CastInfo { get; set; }
        public SkillResult Result { get; set; }
        public BattleContext(Battle battle)
        {
            this.Battle = battle;
        }
    }
}
