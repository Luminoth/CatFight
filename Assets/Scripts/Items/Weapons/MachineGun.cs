using System;

using CatFight.Data;
using CatFight.Fighters;
using CatFight.Util;
using CatFight.Util.ObjectPool;

namespace CatFight.Items.Weapons
{
    [Serializable]
    public sealed class MachineGun : Weapon
    {
        public MachineGun(WeaponData.WeaponDataEntry weaponData)
            : base(weaponData)
        {
        }

        public override void SetStrength(int strength)
        {
UnityEngine.Debug.LogError("TODO: set machinegun strength");
        }

        protected override void DoFire(Fighter fighter)
        {
            PooledObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(WeaponType.GetDescription(), true);
            Bullet bullet = pooledObject?.GetComponent<Bullet>();
            if(null == bullet) {
                return;
            }

            bullet.Initialize(fighter, WeaponType, WeaponData.Damage);
        }
    }
}
