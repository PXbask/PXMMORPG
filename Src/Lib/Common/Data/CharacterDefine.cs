using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public class CharacterDefine
    {
        public int TID { get; set; }
        public string Name { get; set; }
        public CharacterClass Class { get; set; }
        public string Resource { get; set; }
        public string Description { get; set; }

        //基本属性
        public int Speed { get; set; }
        public float MaxHP { get; set; }
        public float MaxMp { get; set; }
        public float GrowthSTR { get; set; }
        public float GrowthINT { get; set; }
        public float GrowthDEX { get; set; }
        /// <summary>
        /// 力量
        /// </summary>
        public float STR { get; set; }
        /// <summary>
        /// 智力
        /// </summary>
        public float INT { get; set; }
        /// <summary>
        /// 敏捷
        /// </summary>
        public float DEX { get; set; }
        /// <summary>
        /// 物理攻击
        /// </summary>
        public float AD { get; set; }
        /// <summary>
        /// 法术攻击
        /// </summary>
        public float AP { get; set; }
        public float DEF { get; set; }
        public float MDEF { get; set; }
        /// <summary>
        /// 攻击速度
        /// </summary>
        public float SPD { get; set; }
        /// <summary>
        /// 暴击概率
        /// </summary>
        public float CRI { get; set; }
    }
}
