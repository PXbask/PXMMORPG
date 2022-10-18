﻿using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;

namespace Managers
{
    class StatusManager : Network.IPostResponser
    {
        private Character Owner;
        private List<NStatus> Status { get; set; }
        public bool HasStatus
        {
            get { return Status != null && Status.Count > 0; }
        }
        public StatusManager(Character character)
        {
            this.Owner = character;
            this.Status=new List<NStatus>();
        }
        public void AddStatus(StatusType type,int id,int value,StatusAction action)
        {
            this.Status.Add(new NStatus
            {
                Type = type,
                Id = id,
                Value = value,
                Action = action,
            });
        }
        public void AddGoldChange(int goldDelta)
        {
            if (goldDelta > 0)
                this.AddStatus(StatusType.Money, 0, goldDelta, StatusAction.Add);
            if (goldDelta < 0)
                this.AddStatus(StatusType.Money, 0, goldDelta, StatusAction.Delete);
        }
        public void AddExpChange(int exp)
        {
            this.AddStatus(StatusType.Exp, 0, exp, StatusAction.Add);
        }
        public void AddLevelUp(int level)
        {
            this.AddStatus(StatusType.Level, 0, level, StatusAction.Add);
        }
        public void AddItemChange(int id,int count,StatusAction action)
        {
            this.AddStatus(StatusType.Item, id, count, action);
        }
        public void PostProcess(NetMessageResponse message)
        {
            if (message.statusNotify == null)
                message.statusNotify = new StatusNotify();
            foreach (NStatus status in Status)
            {
                message.statusNotify.Status.Add(status);
            }
            this.Status.Clear();
        }
    }
}