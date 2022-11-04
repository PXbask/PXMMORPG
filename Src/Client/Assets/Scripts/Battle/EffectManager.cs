using Common.Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Battle
{
    public class EffectManager
    {
        private Creature Owner;
        private Dictionary<BuffEffect, int> Effects = new Dictionary<BuffEffect, int>();
        public EffectManager(Creature creature)
        {
            this.Owner = creature;
        }
        public bool HasEffect(BuffEffect effect)
        {
            int count;
            if (this.Effects.TryGetValue(effect, out count))
            {
                return count > 0;
            }
            return false;
        }
        internal void AddEffect(BuffEffect effect)
        {
            Debug.LogFormat("[{0}].AddEffect {1}", this.Owner.Name, effect.ToString());
            if (!this.Effects.ContainsKey(effect))
                this.Effects.Add(effect, 1);
            else
                this.Effects[effect]++;
        }

        internal void RemoveEffect(BuffEffect effect)
        {
            int count;
            Debug.LogFormat("[{0}].RemoveEffect {1}", this.Owner.Name, effect.ToString());
            if (this.Effects.TryGetValue(effect, out count))
            {
                if (count > 0)
                {
                    this.Effects[effect]--;
                }
            }
        }
    }
}
