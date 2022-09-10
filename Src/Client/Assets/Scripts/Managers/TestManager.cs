using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
namespace Manager
{
    public class TestManager : Singleton<TestManager>
    {
        public void Init()
        {
            NPCManager.Instance.RegisterNpcEvent(NpcFunction.InvokeShop, OnNpcInvokeShop);
            NPCManager.Instance.RegisterNpcEvent(NpcFunction.InvokeInstance, OnNpcInvokeInstance);
        }
        private bool OnNpcInvokeShop(NpcDefine npc)
        {
            UITest test=UIManager.Instance.Show<UITest>();
            test.SetInfo(npc.Name, npc.Description);
            return true;
        }
        private bool OnNpcInvokeInstance(NpcDefine npc)
        {
            MessageBox.Show("Click Npc:" + npc.Name);
            return true;
        }
    }
}


