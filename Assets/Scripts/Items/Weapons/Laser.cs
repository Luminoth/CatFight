using System;

using CatFight.Data;
using CatFight.Fighters;
using CatFight.Util.ObjectPool;

namespace CatFight.Items.Weapons
{
    [Serializable]
    public sealed class Laser : Weapon
    {
        public Laser(Fighter fighter, WeaponData.WeaponDataEntry weaponData)
            : base(fighter, weaponData)
        {
        }

        protected override void DoFire()
        {
            PooledObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(Data.WeaponData.GetAmmoPool(WeaponType), FighterManager.Instance.AmmoContainer.transform);
            LaserShot laserShot = pooledObject?.GetComponent<LaserShot>();
            if(null == laserShot) {
                return;
            }

            laserShot.Initialize(Fighter, WeaponType, Damage);
        }
    }
}
