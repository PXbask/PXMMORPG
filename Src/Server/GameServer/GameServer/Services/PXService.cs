using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    internal class PXService : Singleton<PXService>
    {
        public void Init() { }
        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<PXTestRequest>(this.OnPXTestRequest);
        }
        public void Stop() { }
        public void OnPXTestRequest(NetConnection<NetSession> sender, PXTestRequest request)
        {
            Log.InfoFormat("PXTestRequest: PXAge:{0}", request.Age);
        }
    }
}
