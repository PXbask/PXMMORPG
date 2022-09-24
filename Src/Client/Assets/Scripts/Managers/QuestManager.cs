using Common.Data;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager {
    public enum NpcQuestStatus
    {
        None = 0,//无任务
        Complete = 1,//拥有已完成可提交任务
        Available = 2,//拥有可接受任务
        Incomplete = 3,//拥有未接受任务
    }
	public class QuestManager : Singleton<QuestManager>
	{
        public List<NQuestInfo> questInfos = new List<NQuestInfo>();
		public Dictionary<int, Quest> allQuests=new Dictionary<int, Quest>();
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();

        public Action OnQuestChanged { get; internal set; }

        internal void Init(List<NQuestInfo> quests)
        {
            this.questInfos = quests;
            allQuests.Clear();
            npcQuests.Clear();
            InitQuests();
        }

        private void InitQuests()
        {
            //初始化已有任务
            foreach(var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                this.AddNpcQuest(quest.Define.AcceptNPC, quest);
                this.AddNpcQuest(quest.Define.SubmitNPC, quest);
                this.allQuests[quest.Info.QuestId] = quest;
            }
            //初始化*可用*任务
            foreach(var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacter.Class)
                    continue;//不符合职业
                if (kv.Value.LimitLevel > User.Instance.CurrentCharacter.Level)
                    continue;//不符合等级
                if (this.allQuests.ContainsKey(kv.Key))
                    continue;//任务已经存在

                if (kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    if(this.allQuests.TryGetValue(kv.Key, out preQuest))//获取前置任务
                    {
                        if (preQuest.Info == null)
                            continue;//前置任务未接取
                        if (preQuest.Info.Status != QuestStatus.Finished)
                            continue;//前置任务未完成
                    }
                    else
                    {
                        continue;//前置任务还没接
                    }
                }
                Quest quest=new Quest(kv.Value);
                this.AddNpcQuest(quest.Define.AcceptNPC, quest);
                this.AddNpcQuest(quest.Define.SubmitNPC, quest);
                this.allQuests.Add(quest.Define.ID, quest);
            }
        }

        private void AddNpcQuest(int npcId, Quest quest)
        {
            if(!this.npcQuests.ContainsKey(npcId))
                this.npcQuests.Add(npcId, new Dictionary<NpcQuestStatus, List<Quest>>());
            List<Quest> availables;
            List<Quest> completes;
            List<Quest> incompletes;
            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                this.npcQuests[npcId].Add(NpcQuestStatus.Available, availables);
            }
            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Complete, out completes))
            {
                completes = new List<Quest>();
                this.npcQuests[npcId].Add(NpcQuestStatus.Complete, completes);
            }
            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplete, out incompletes))
            {
                incompletes = new List<Quest>();
                this.npcQuests[npcId].Add(NpcQuestStatus.Incomplete, incompletes);
            }

            if (quest.Info == null)
            {
                if (npcId == quest.Define.AcceptNPC && !this.npcQuests[npcId][NpcQuestStatus.Available].Contains(quest))
                    this.npcQuests[npcId][NpcQuestStatus.Available].Add(quest);
            }
            else
            {
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.Complated)
                {
                    if (!this.npcQuests[npcId][NpcQuestStatus.Complete].Contains(quest))
                        this.npcQuests[npcId][NpcQuestStatus.Complete].Add(quest);
                }
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.InProgress)
                {
                    if (!this.npcQuests[npcId][NpcQuestStatus.Incomplete].Contains(quest))
                        this.npcQuests[npcId][NpcQuestStatus.Incomplete].Add(quest);
                }
            }
        }
        /// <summary>
        /// 获取npc任务状态
        /// </summary>
        /// <param name="npcid"></param>
        /// <returns></returns>
        public NpcQuestStatus GetQuestStatusByNpc(int npcid)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if(this.npcQuests.TryGetValue(npcid, out status))
            {
                if(status[NpcQuestStatus.Complete].Count > 0)
                    return NpcQuestStatus.Complete;
                if(status[NpcQuestStatus.Available].Count > 0)
                    return NpcQuestStatus.Available;
                if(status[NpcQuestStatus.Incomplete].Count > 0)
                    return NpcQuestStatus.Incomplete;
            }
            return NpcQuestStatus.None;
        }
        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (this.npcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Complete].First());
                if (status[NpcQuestStatus.Available].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Available].First());
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Incomplete].First());
            }
            return false;
        }
        private bool ShowQuestDialog(Quest quest)
        {
            if (quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                UIQuestDialog dialog = UIManager.Instance.Show<UIQuestDialog>();
                dialog.SetQuest(quest);
                dialog.OnClose += OnQuestDialogClose;
                return true;
            }
            if(quest.Info != null && quest.Info.Status != QuestStatus.Complated)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                    MessageBox.Show(quest.Define.DialogIncomplete);
            }
            return true;
        }

        private void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if (result == UIWindow.WindowResult.Yes)
            {
                MessageBox.Show(dlg.quest.Define.DialogAccept);
            }else if (result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(dlg.quest.Define.DialogDeny);
            }
        }
        public void OnQuestAccepted(Quest quest)
        {

        }
    }
}


