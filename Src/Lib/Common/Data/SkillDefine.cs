using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum SkillType
    {
        Normal,
        Skill,
    }
    public enum SkillTarget
    {
        None,
        Target,
        Position,
        Self
    }

    public class SkillDefine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public SkillType Type { get; set; }
        public int UnlockLevel { get; set; }
        public SkillTarget CastTarget { get; set; }
        public int CastRange { get; set; }
        public float CastTime { get; set; }
        public float CD { get; set; }
        public int MPCost { get; set; }
        public bool Bullet { get; set; }
        public float BulletSpeed { get; set; }
        public string BulletResource { get; set; }
        public int AOERange { get; set; }
        public float Duration { get; set; }
        public float Interval { get; set; }
        public List<float> HitTimes { get; set; }
        public List<int> Buff { get; set; }
        public float AD { get; set; }
        public float AP { get; set; }
        public float ADFator { get; set; }
        public float APFator { get; set; }
        public string SkillAnim { get; set; }
    }
}
