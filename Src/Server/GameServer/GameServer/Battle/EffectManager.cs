﻿using Common;
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
        public bool HasEffect(BuffEffect effect)
        {
            if(this.Effects.TryGetValue(effect, out int count))
            {
                return count > 0;
            }
            return false;
        }
        internal void AddEffect(BuffEffect effect)
        {
            Log.InfoFormat("[{0}].AddEffect {1}", this.Owner.Name, effect.ToString());
            if(!this.Effects.ContainsKey(effect))
                this.Effects.Add(effect, 1);
            else
                this.Effects[effect]++;
        }

        internal void RemoveEffect(BuffEffect effect)
        {
            Log.InfoFormat("[{0}].RemoveEffect {1}", this.Owner.Name, effect.ToString());
            if (this.Effects.TryGetValue(effect,out int count))
            {
                if (count > 0)
                {
                    this.Effects[effect]--;
                }
            }
        }
    }
}
