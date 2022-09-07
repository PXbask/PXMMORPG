using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Network;
using SkillBridge.Message;
using Models;
using Manager;
namespace Services
{
    public class MapService : Singleton<MapService>, IDisposable
    {
        public int CurrentMapID { get; set; }
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }
        public void Init() {}
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySync);

        }
        public void EnterMap(int id)
        {
            if (DataManager.Instance.Maps.ContainsKey(id))
            {
                User.Instance.CurrentMapData = DataManager.Instance.Maps[id];
                SceneManager.Instance.LoadScene(DataManager.Instance.Maps[id].Resource);
            }
            else
            {
                Debug.LogErrorFormat("Scene{0} don't Exist", id);
            }
        }
        #region SendMessages
        internal void SendEntitySync(NEntity entity,EntityEvent @event)
        {
            NetMessage message = new NetMessage();
            message.Request =new NetMessageRequest();
            message.Request.mapEntitySync=new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Entity=entity,
                Event=@event
            };
            NetClient.Instance.SendMessage(message);
        }
        internal void SendMapTeleport(int iD)
        {
            Debug.LogFormat("MapTeleportRequest: teleporterId:{0}",iD);
            NetMessage message = new NetMessage();
            message.Request=new NetMessageRequest();
            message.Request.mapTeleport=new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId=iD;
            NetClient.Instance.SendMessage(message);
        }
        #endregion
        #region MessageDistributer Event
        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            if (User.Instance.CurrentCharacter != null)
                Debug.LogFormat("OnMapCharacterEnter:MapID:{0} CharacterID:{1}", User.Instance.CurrentCharacter.mapId, response.mapId);
            foreach (var character in response.Characters)
            {
                if (User.Instance.CurrentCharacter==null || User.Instance.CurrentCharacter.Id == character.Id)
                {
                    User.Instance.CurrentCharacter = character;
                }
                CharacterManager.Instance.AddCharacter(character);
            }
            if (response.mapId != CurrentMapID)
            {
                this.EnterMap(response.mapId);
                CurrentMapID = response.mapId;
            }
        }
        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave:MapID:{0} CharacterID:{1}", User.Instance.CurrentCharacter.mapId, response.characterId);
            if (response.characterId != User.Instance.CurrentCharacter.Id)
                Manager.CharacterManager.Instance.RemoveCharacter(response.characterId);
            else
                Manager.CharacterManager.Instance.Clear();
        }
        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("MapEntityUpdateResponse:Entitys:{0}", response.entitySyncs.Count);
            sb.AppendLine();
            foreach(var entity in response.entitySyncs)
            {
                Manager.EntityManager.Instance.OnEntitySync(entity);
                sb.AppendFormat("    [{0}]evt:{1} entity:{2}", entity.Id, entity.Event, entity.Entity.String());
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }
        #endregion
    }
}
