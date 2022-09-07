using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
namespace GameServer.Services
{
    internal class MapService : Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleport);
        }

        public void Init()
        {
            MapManager.Instance.Init();
        }
        public void SendUpdateEntity(NetConnection<NetSession> connection, NEntitySync sync)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapEntitySync = new MapEntitySyncResponse();
            message.Response.mapEntitySync.entitySyncs.Add(sync);

            byte[] data = PackageHandler.PackMessage(message);
            connection.SendData(data, 0, data.Length);
        }
        private void OnMapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest request)
        {
            Character character=sender.Session.Character;
            NEntitySync sync = request.entitySync;
            Log.InfoFormat("OnMapEntitySync: characterID:{0}:{1} entityID:{2} event:{3} Entity:{4}",
                character.Id, character.Info.Name, sync.Id, sync.Event, sync.Entity.String());
            MapManager.Instance[character.Info.mapId].UpdateEntity(sync);
        }
        private void OnMapTeleport(NetConnection<NetSession> sender, MapTeleportRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnMapTeleport: characterID:{0}:{1} TeleporterID:{3}", character.Id, character.Data, request.teleporterId);
            if (!DataManager.Instance.Teleporters.ContainsKey(character.Id))
            {
                Log.WarningFormat("Source TeleporterID [{0}] not existed", request.teleporterId);
                return;
            }
            TeleporterDefine define = DataManager.Instance.Teleporters[request.teleporterId];
            if (define.LinkTo == 0 || !DataManager.Instance.Teleporters.ContainsKey(request.teleporterId))
            {
                Log.WarningFormat("Source TeleporterID [{0}] linkTo ID [{1}] not existed", request.teleporterId, define.LinkTo);
            }
            TeleporterDefine target = DataManager.Instance.Teleporters[define.LinkTo];

            MapManager.Instance[define.MapID].CharacterLeave(character);
            character.Position = target.Position;
            character.Direction = target.Direction;
            MapManager.Instance[target.MapID].CharacterEnter(sender, character);
        }
    }
}
