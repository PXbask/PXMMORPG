using Common.Battle;
using GameServer.Battle;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.AI
{
    class AIAgent
    {
        private Monster monster;
        private AIBase ai;
        public AIAgent(Monster monster)
        {
            this.monster = monster;
            string aiName = monster.Define.AI;
            if (string.IsNullOrEmpty(aiName))
            {
                aiName = AIMonsterPassive.ID;
            }
            switch (aiName)
            {
                case AIMonsterPassive.ID:
                    this.ai = new AIMonsterPassive(this.monster);
                    break;
                case AIBoss.ID:
                    this.ai = new AIBoss(this.monster);
                    break;
                default:
                    break;
            }
        }

        internal void OnDamage(NDamageInfo damage, Creature creature)
        {
            if (this.ai != null)
            {
                this.ai.OnDamage(damage, creature);
            }
        }

        internal void Update()
        {
            if (this.ai != null)
            {
                this.ai.Update();
            }
        }

    }
}
