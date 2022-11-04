using Common.Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace Battle
{
    public class Buff
    {
        internal bool isStopped = false;
        private Creature Owner;
        public int buffId;
        public BuffDefine define;
        private int casterId;
        public float time = 0;
        private int hit = 0;

        public Buff(Creature owner, int buffId, BuffDefine define, int casterId)
        {
            this.Owner = owner;
            this.buffId = buffId;
            this.define = define;
            this.casterId = casterId;
            this.OnAdd();
        }

        private void OnAdd()
        {
            Debug.LogFormat("[ADD] BuffID:[{0}] Name:[{1}]", this.buffId, this, define.Name);
            isStopped = false;
            this.AddEffect();
            this.AddAttr();
        }
        internal void OnRemove()
        {
            Debug.LogFormat("[REMOVE] BuffID:[{0}] Name:[{1}]", this.buffId, this, define.Name);
            isStopped = true;
            this.RemoveEffect();
            this.RemoveAttr();
        }
        private void AddAttr()
        {
            if (this.define.DEFRatio != 0)
            {
                this.Owner.Attributes.Buff.DEF += this.Owner.Attributes.Basic.DEF * this.define.DEFRatio;
                this.Owner.Attributes.InitFinalAttributes();
            }
        }
        private void RemoveAttr()
        {
            if (this.define.DEFRatio != 0)
            {
                this.Owner.Attributes.Buff.DEF = this.Owner.Attributes.Basic.DEF * this.define.DEFRatio;
                this.Owner.Attributes.InitFinalAttributes();
            }
        }
        private void AddEffect()
        {
            if (!this.define.Effect.Equals(BuffEffect.None))
            {
                this.Owner.AddBuffEffect(this.define.Effect);
            }
        }
        private void RemoveEffect()
        {
            if (!this.define.Effect.Equals(BuffEffect.None))
            {
                this.Owner.RemoveBuffEffect(this.define.Effect);
            }
        }
        internal void OnUpdate(float delta)
        {
            if (isStopped) return;
            this.time += delta;
            if (time > this.define.Duration)
            {
                this.OnRemove();
            }
        }

        private void DoBuffDamage()
        {
            throw new NotImplementedException();
        }
    }
}
