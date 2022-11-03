using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum BuffTarget
    {
        None = 0,
        Target,
        Self,
        Position
    }
    public enum BuffEffect
    {
        None = 0,
        Stun,///眩晕
        Invincible,
    }

    public enum TriggerType
    {
        None = 0,
        SkillCast = 1,
        SkillHit = 2
    }

    public class BuffDefine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public BuffTarget Target { get; set; }
        public BuffEffect Effect { get; set; }
        public TriggerType Trigger { get; set; }
        public int CD { get; set; }
        public float Duration { get; set; }
        public float Interval { get; set; }
        public float AD { get; set; }
        public float AP { get; set; }
        public float ADFator { get; set; }
        public float APFator { get; set; }
        public float DEFRatio { get; set; }
    }
}
