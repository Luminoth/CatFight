﻿using System;

using CatFight.Data;
using CatFight.Util;
using CatFight.Util.ObjectPool;

namespace CatFight.Items.Weapons
{
    [Serializable]
    public sealed class MachineGun : Weapon
    {
        public override WeaponData.WeaponType WeaponType => WeaponData.WeaponType.MachineGun;

        public override void SetStrength(int strength)
        {
UnityEngine.Debug.LogError("TODO: set machinegun strength");
        }

        public override void Fire()
        {
            PooledObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(WeaponType.GetDescription());
            Bullet bullet = pooledObject?.GetComponent<Bullet>();
            if(null == bullet) {
                return;
            }

            //bullet.Damage = damage;
// TODO: other stuff
        }
    }
}
