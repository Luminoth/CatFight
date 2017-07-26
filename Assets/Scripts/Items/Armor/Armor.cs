using System;
using System.Collections.Generic;
using System.Text;

using CatFight.Data;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Items.Armor
{
    [Serializable]
    public sealed class Armor : Item
    {
        private readonly Dictionary<WeaponData.WeaponType, int> _damageReductionPercents = new Dictionary<WeaponData.WeaponType, int>
        {
            { WeaponData.WeaponType.MachineGun, 0 },
            { WeaponData.WeaponType.Laser, 0 }
        };

        public IReadOnlyDictionary<WeaponData.WeaponType, int> DamageReductionPercents => _damageReductionPercents;

        public void IncreaseStrength(WeaponData.WeaponType type, int strength, int damageReductionPercent, int maxReductionPercent)
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

        public float GetDamageReduction(WeaponData.WeaponType type)
        {
            return _damageReductionPercents.GetOrDefault(type) / 100.0f;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Armor:");
            foreach(var kvp in DamageReductionPercents) {
                builder.AppendLine($"{kvp.Key}: {kvp.Value}");
            }
            return builder.ToString();
        }
    }
}
