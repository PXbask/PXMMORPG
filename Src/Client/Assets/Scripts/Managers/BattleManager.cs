using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Manager
{
    class BattleManager:Singleton<BattleManager>
    {
        public Creature Target;
        public Vector3 Position;
    }
}
