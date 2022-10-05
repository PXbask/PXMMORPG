using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using SkillBridge.Message;
using Common;
using Utils;

namespace GameServer.Models
{
    class Team : IPostResponser
    {
        public int Id;
        public Character Leader;
        public List<Character> Members = new List<Character>();
        public double timeStamp;

        public Team(Character leader)
        {
            this.AddMember(leader);
        }

        public void AddMember(Character member)
        {
            if(Members.Count == 0)
            {
                this.Leader = member;
            }
            this.Members.Add(member);
            member.team = this;
            timeStamp = TimeUtil.timestamp;
        }
        public void Leave(Character member)
        {
            Log.InfoFormat("{0}:{1} Leave Team id:{2}", member.Id, member.Info.Name, this.Id);
            this.Members.Remove(member);
            if(member == this.Leader)
            {
                if (this.Members.Count > 0)
                    this.Leader = this.Members[0];
                else
                    this.Leader = null;
            }
            member.team = null;
            timeStamp = TimeUtil.timestamp;
        }
        public void PostProcess(NetMessageResponse response)
        {
            if(response.teamInfo == null)
            {
                response.teamInfo = new TeamInfoResponse();
                response.teamInfo.Team = new NTeamInfo();
                response.teamInfo.Result = Result.Success;
                response.teamInfo.Team.Id = this.Id;
                response.teamInfo.Team.Leader = this.Leader.Id;
                foreach(var member in this.Members)
                {
                    response.teamInfo.Team.Members.Add(member.GetBasicInfo());
                }
            }
        }
    }
}
