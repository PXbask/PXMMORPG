using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        /// <summary>
        /// 地图中的角色，以character.Id为Key
        /// </summary>
        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();

        public SpawnManager SpawnManager = new SpawnManager();

        public MonsterManager MonsterManager = new MonsterManager();

        internal Map(MapDefine define)
        {
            this.Define = define;
            this.SpawnManager.Init(this);
            this.MonsterManager.Init(this);
        }

        internal void BroadcastBattleResponse(NetMessageResponse response)
        {
            foreach (var kv in MapCharacters)
            {
                kv.Value.connection.Session.Response.skillCast = response.skillCast;
                kv.Value.connection.SendResponse();
            }
        }

        internal void Update()
        {
            this.SpawnManager.Update();
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);

            character.Info.mapId = this.ID;

            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            conn.Session.Response.mapCharacterEnter.Characters.Add(character.Info);

            foreach (var kv in this.MapCharacters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                if (kv.Value.character != character)
                    this.AddCharacterEnterMap(kv.Value.connection, character.Info);
            }
            foreach (var kv in this.MonsterManager.Monsters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.Info);
            }
            this.MapCharacters[character.Id] = new MapCharacter(conn, character);

            conn.SendResponse();
        }
        internal void CharacterLeave(Character charInfo)
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterid:{1}", this.ID, charInfo.Id);
            foreach(var kv in this.MapCharacters)
            {
                this.SendCharacterLeaveMap(kv.Value.connection, charInfo);
            }
            MapCharacters.Remove(charInfo.Id);
        }
        internal void MonsterEnter(Monster monster)
        {
            Log.InfoFormat("MonsterEnter: Map:{0} monsterId:{1}", this.ID, monster.Id);
            foreach(var kv in this.MapCharacters)
            {
                this.AddCharacterEnterMap(kv.Value.connection, monster.Info);
            }
        }
        #region SendMessages
        void AddCharacterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            if (conn.Session.Response.mapCharacterEnter == null)
            {
                conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                conn.Session.Response.mapCharacterEnter.mapId = this.ID;
            }
            conn.Session.Response.mapCharacterEnter.Characters.Add(character);

            conn.SendResponse();
        }

        private void SendCharacterLeaveMap(NetConnection<NetSession> connection, Character charInfo)
        {
            Log.InfoFormat("SendCharacterLeaveMap To {0}:{1} Map:{2} character:{3}:{4}",
                connection.Session.Character.Id, connection.Session.Character.Info.Name, this.Define.ID, charInfo.Id, charInfo.Info.Name);
            if (connection.Session.Response.mapCharacterLeave == null)
            {
                connection.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            }
            connection.Session.Response.mapCharacterLeave.entityId = charInfo.entityId;
            connection.SendResponse();
        }

        #endregion
        internal void UpdateEntity(NEntitySync sync)
        {
            foreach(var kv in MapCharacters)
            {
                if (kv.Value.character.entityId == sync.Id)
                {
                    kv.Value.character.Speed = sync.Entity.Speed;
                    kv.Value.character.Direction = sync.Entity.Direction;
                    kv.Value.character.Position = sync.Entity.Position;
                    if (sync.Event == EntityEvent.Ride)
                    {
                        kv.Value.character.Ride = sync.Param;
                    }
                }
                else
                {
                    Services.MapService.Instance.SendUpdateEntity(kv.Value.connection, sync);
                }
            }
        }


    }
}
