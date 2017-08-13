using System;

using CatFight.Data;
using CatFight.Fighters;
using CatFight.Util.ObjectPool;

namespace CatFight.Items.Weapons
{
    [Serializable]
    public sealed class MachineGun : Weapon
    {
        public MachineGun(Fighter fighter, WeaponData.WeaponDataEntry weaponData)
            : base(fighter, weaponData)
        {
        }

        protected override void DoFire()
        {
            PooledObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(Data.WeaponData.GetAmmoPool(WeaponType), FighterManager.Instance.AmmoContainer.transform);
            Bullet bullet = pooledObject?.GetComponent<Bullet>();
            if(null == bullet) {
                return;
            }

            bullet.Initialize(Fighter, WeaponType, Damage);
        }
    }
}
