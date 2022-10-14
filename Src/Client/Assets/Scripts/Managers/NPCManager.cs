using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using System;

namespace Manager
{
	public class NPCManager : Singleton<NPCManager>
	{
		public delegate bool NpcActionHandler(NpcDefine npc);
		private Dictionary<NpcFunction,NpcActionHandler> eventMap=new Dictionary<NpcFunction,NpcActionHandler>();
        public Dictionary<int, Vector3> npcPosition = new Dictionary<int, Vector3>();
		public void RegisterNpcEvent(NpcFunction function,NpcActionHandler action)
        {
			if (!eventMap.ContainsKey(function))
			{
				eventMap[function] = action;
			}
			else
				eventMap[function] += action;
        }
		public NpcDefine GetNpcDefine(int npcID)
        {
			NpcDefine npcDefine = null;
			DataManager.Instance.Npcs.TryGetValue(npcID, out npcDefine);
			return npcDefine;
        }
		public bool Interactive(int npcID)
        {
            if (DataManager.Instance.Npcs.ContainsKey(npcID))
            {
				var npc=DataManager.Instance.Npcs[npcID];
				return Interactive(npc);
            }
            else
            {
				return false;
            }
        }
		public bool Interactive(NpcDefine define)
        {
            if (DoTaskInteractive(define))
            {
                return true;
            }
            else if (define.Type == NpcType.Functional)
            {
                return DoFunctionInteractive(define);
            }
            return false;
        }
        private bool DoTaskInteractive(NpcDefine define)
        {
            var status = QuestManager.Instance.GetQuestStatusByNpc(define.ID);
            if (status.Equals(NpcQuestStatus.None)) return false;
            else return QuestManager.Instance.OpenNpcQuest(define.ID);
        }
        private bool DoFunctionInteractive(NpcDefine define)
        {
            if(define.Type != NpcType.Functional)
                return false;
            if (!eventMap.ContainsKey(define.Function))
                return false;
            return eventMap[define.Function](define);
        }
        public void UpdateNpcPosition(int npc,Vector3 pos)
        {
            this.npcPosition[npc] = pos;
        }
        public Vector3 GetNpcPosition(int npc)
        {
            return this.npcPosition[npc];
        }
    }
}

