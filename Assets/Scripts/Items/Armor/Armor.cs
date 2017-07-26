using System;
using System.Collections.Generic;

using CatFight.Util;

using UnityEngine;

namespace CatFight.Items.Armor
{
    [Serializable]
    public sealed class Armor : Item
    {
        public const string ArmorTypeMachineGun = "machinegun";
        public const string ArmorTypeLaser = "laser";

        private readonly Dictionary<string, int> _damageReductionPercents = new Dictionary<string, int>
        {
            { ArmorTypeMachineGun, 0 },
            { ArmorTypeLaser, 0 }
        };

        public void IncreaseStrength(string type, int strength, int damageReductionPercent, int maxReductionPercent)
        {
            if(!_damageReductionPercents.ContainsKey(type)) {
                return;
            }

            // TODO: can this be done with math rather than a for-loop?
            int reduction = _damageReductionPercents[type];
            for(int i=0; i<strength; ++i) {
                reduction = (int)Mathf.Clamp(reduction + (100 - reduction) * (damageReductionPercent / 100.0f), 0, maxReductionPercent);
            }
            _damageReductionPercents[type] = reduction;
        }

        public float GetDamageReduction(string type)
        {
            return _damageReductionPercents.GetOrDefault(type) / 100.0f;
        }
    }
}
