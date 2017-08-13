using System;

using CatFight.Data;
using CatFight.Fighters;
using CatFight.Util.ObjectPool;

namespace CatFight.Items.Weapons
{
    [Serializable]
    public sealed class MachineGun : Weapon
    {
        public MachineGun(Fighter fighter, int slotId, WeaponData.WeaponDataEntry weaponData)
            : base(fighter, slotId, weaponData)
        {
        }

        protected override void DoFire()
        {
            PooledObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(Data.WeaponData.GetAmmoPool(WeaponType), FighterManager.Instance.AmmoContainer.transform);
            Bullet bullet = pooledObject?.GetComponent<Bullet>();
            bullet?.Initialize(Fighter, SlotId, WeaponType, Damage);
        }
    }
}
