using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Network;
using SkillBridge.Message;
using Models;
using Manager;
using Entities;

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
                SoundManager.Instance.PlayMusic(DataManager.Instance.Maps[id].Music);
            }
            else
            {
                Debug.LogErrorFormat("Scene{0} don't Exist", id);
            }
        }
        #region SendMessages
        internal void SendEntitySync(NEntity entity,EntityEvent @event,int param)
        {
            NetMessage message = new NetMessage();
            message.Request =new NetMessageRequest();
            message.Request.mapEntitySync=new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Entity=entity,
                Event=@event,
                Param=param
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
            if (User.Instance.CurrentCharacterInfo != null)
                Debug.LogFormat("OnMapCharacterEnter:MapID:{0} CharacterID:{1}", User.Instance.CurrentCharacterInfo.mapId, response.mapId);
            foreach (var character in response.Characters)
            {
                if (User.Instance.CurrentCharacterInfo==null || (User.Instance.CurrentCharacterInfo.Id == character.Id && character.Type.Equals(CharacterType.Player)))
                {
                    User.Instance.CurrentCharacterInfo = character;
                    if (User.Instance.CurrentCharacter == null)
                    {
                        User.Instance.CurrentCharacter = new Entities.Character(character);
                    }
                    else
                    {
                        User.Instance.CurrentCharacter.UpdateCharacterInfo(character);
                    }
                    CharacterManager.Instance.AddCharacter(User.Instance.CurrentCharacter);
                    continue;
                }
                CharacterManager.Instance.AddCharacter(new Character(character));
            }
            if (response.mapId != CurrentMapID)
            {
                this.EnterMap(response.mapId);
                CurrentMapID = response.mapId;
            }
        }
        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave:MapID:{0} CharacterID:{1}", User.Instance.CurrentCharacterInfo.mapId, response.entityId);
            if (response.entityId != User.Instance.CurrentCharacterInfo.EntityId)
                Manager.CharacterManager.Instance.RemoveCharacter(response.entityId);
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
