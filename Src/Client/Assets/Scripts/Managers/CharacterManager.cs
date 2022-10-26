using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.Events;

using Entities;
using SkillBridge.Message;

namespace Manager
{
    class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        /// <summary>
        /// 以EntityId为Key
        /// </summary>
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();


        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;

        public CharacterManager()
        {

        }

        public void Dispose()
        {
        }

        public void Init()
        {

        }

        public void Clear()
        {
            int[] keys=this.Characters.Keys.ToArray();
            foreach(int key in keys)
            {
                this.RemoveCharacter(key);
            }
            this.Characters.Clear();
        }

        public void AddCharacter(Character cha)
        {
            Debug.LogFormat("AddCharacter:{0}:{1} Map:{2} Entity:{3}", cha.ID, cha.Name, cha.Info.mapId, cha.Info.Entity.String());
            this.Characters[cha.entityId] = cha;
            Manager.EntityManager.Instance.AddEntity(cha);
            if (OnCharacterEnter!=null)
            {
                OnCharacterEnter(cha);
            }
        }


        public void RemoveCharacter(int entityId)
        {
            Debug.LogFormat("RemoveCharacter:{0}", entityId);
            if (this.Characters.ContainsKey(entityId))
            {
                Manager.EntityManager.Instance.RemoveEntity(this.Characters[entityId].Info.Entity);
                if (this.OnCharacterLeave != null)
                {
                    this.OnCharacterLeave(Characters[entityId]);
                }
                this.Characters.Remove(entityId);
            }
        }
        public Character GetCharacter(int id)
        {
            Character character = null;
            this.Characters.TryGetValue(id, out character);
            return character;
        }
    }
}
