using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    public interface IEntityController
    {
        void PlayAnim(string name);
        void SetStandBy(bool standBy);
        void UpdateDirection();
        void PlayEffect(EffectType type, string name, Entity target, float duration);
        Transform GetTransform();
    }
}
