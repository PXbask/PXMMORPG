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
    internal class HelloWorldService : Singleton<HelloWorldService>
    {
        public void Init()
        {
            
        }
        public void Start()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest);
        }
        public void Stop() { }
        public void OnFirstTestRequest(NetConnection<NetSession> sender,FirstTestRequest request)
        {
            Log.InfoFormat("FirstTestRequest: HelloWorld:{0}", request.Helloworld);
        }
    }
}
