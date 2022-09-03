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
        public int CurrentMapID { get; private set; }
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }
        public void Init() {}
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);

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
        #region MessageDistributer Event
        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:MapID:{0} CharacterID:{1}", User.Instance.CurrentCharacter.mapId, response.mapId);
            foreach (var character in response.Characters)
            {
                if (User.Instance.CurrentCharacter.Id == character.Id)
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
                Manager.CharacterManager.Instance.Characters.Clear();
        }
        #endregion
    }
}
