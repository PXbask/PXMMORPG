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
			//return DataManager.Instance.Npcs[npcID];
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
            if (define.Type == NpcType.Task)
            {
                return DoTaskInteractive(define);
            }
            else if (define.Type == NpcType.Functional)
            {
                return DoFunctionInteractive(define);
            }
            else
            {
                return false;
            }
        }
        private bool DoTaskInteractive(NpcDefine define)
        {
            MessageBox.Show("Clicked Npc:" + define.Name, "Npc talk");
            return true;
        }
        private bool DoFunctionInteractive(NpcDefine define)
        {
            if(define.Type != NpcType.Functional)
                return false;
            if (!eventMap.ContainsKey(define.Function))
                return false;
            return eventMap[define.Function](define);
        }
    }
}

