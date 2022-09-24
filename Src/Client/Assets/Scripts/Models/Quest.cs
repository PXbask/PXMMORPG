using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Manager;
using SkillBridge.Message;

namespace Models
{
    public class Quest
    {
        public NQuestInfo Info { get; set; }
        public QuestDefine Define { get; set; }
        public Quest(NQuestInfo info)
        {
            Info = info;
            Define = DataManager.Instance.Quests[info.QuestId];
        }
        public Quest(QuestDefine define)
        {
            Define = define;
            Info = null;
        }
        public string GetTypeName()
        {
            return string.Empty;//TODO
        }
    }
}
