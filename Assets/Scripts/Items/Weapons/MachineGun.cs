using System;

using CatFight.Data;
using CatFight.Fighters;
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

        protected override void DoFire(Fighter fighter)
        {
            PooledObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(Data.WeaponData.GetAmmoPool(WeaponType));
            Bullet bullet = pooledObject?.GetComponent<Bullet>();
            if(null == bullet) {
                return;
            }

            bullet.Initialize(fighter, WeaponType, Damage);
        }
    }
}
