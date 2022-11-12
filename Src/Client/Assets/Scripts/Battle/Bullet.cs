﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Battle
{
    public class Bullet
    {
        Skill skill;
        private int hit;
        float flyTime = 0;
        public float duration = 0;

        public bool Stopped = false;

        public Bullet(Skill skill)
        {
            this.skill = skill;
            var target = skill.Target;
            this.hit = skill.Hit;
            int distance = skill.owner.Distance(target);
            duration = distance / this.skill.Define.BulletSpeed;
        }
        public void Update()
        {
            if (this.Stopped) return;
            this.flyTime += Time.deltaTime;
            if (this.flyTime > duration)
            {
                this.skill.DoHitDamages(this.hit);
                this.Stop();
            }
        }
        private void Stop()
        {
            this.Stopped = true;
        }
    }
}
