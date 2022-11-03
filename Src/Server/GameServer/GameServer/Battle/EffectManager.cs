using Common.Data;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    class EffectManager
    {
        private Creature Owner;
        private Dictionary<BuffEffect, int> Effects = new Dictionary<BuffEffect, int>();
        public EffectManager(Creature creature)
        {
            this.Owner = creature;
        }

        internal void AddEffect(BuffEffect effect)
        {
            
        }

        internal void RemoveEffect(BuffEffect effect)
        {
            
        }
    }
}
