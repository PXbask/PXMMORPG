﻿using Common.Data;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battle
{
    class BuffManager
    {
        private Creature Owner;
        private List<Buff> Buffs = new List<Buff>();

        private int idx = 1;
        private int BuffID { get { return this.idx++; } }
        public BuffManager(Creature creature)
        {
            this.Owner = creature;
        }

        internal void AddBuff(BattleContext context, BuffDefine define)
        {
            Buff buff = new Buff(this.BuffID, this.Owner, define, context);
            this.Buffs.Add(buff);
        }

        internal void Update()
        {
            foreach (var buff in this.Buffs)
            {
                if (!buff.isStopped)
                {
                    buff.Update();
                }
            }
            this.Buffs.RemoveAll(buff => buff.isStopped);
        }
    }
}
