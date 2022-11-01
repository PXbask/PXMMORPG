using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GameServer.Battle
{
    class Bullet
    {
        private Skill skill;
        private Entities.Creature target;
        private bool TimeMode = true;
        private float duration = 0;
        private float flyTime = 0;
        public bool isStopped = false;
        private SkillBridge.Message.NSkillHitInfo hitInfo;
        public Bullet(Skill skill, Entities.Creature target, SkillBridge.Message.NSkillHitInfo hitInfo)
        {
            this.skill = skill;
            this.target = target;
            this.hitInfo = hitInfo;
            int dis = this.skill.owner.Distance(target);
            if (TimeMode)
                this.duration = dis / skill.Define.BulletSpeed;
            Log.InfoFormat("Bullet[{0}].CastBullet[{1}] Target:{2} Distance:{3} Time:{4}"
                , this.skill.Define.Name, this.skill.Define.BulletResource, target.Define.Name, dis, this.duration);
        }
        public void Update()
        {
            if (isStopped) return;
            if (TimeMode)
                UpdateTime();
            else
                UpdatePos();
        }

        private void UpdateTime()
        {
            flyTime += TimeUtil.deltaTime;
            if(flyTime > duration)
            {
                this.hitInfo.isBullet = true;
                skill.DoHit(this.hitInfo);
                this.isStopped = true;
            }
        }

        private void UpdatePos()
        {
            throw new NotImplementedException();
        }
    }
}
