using System;

using CatFight.Data;
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

        protected override void DoFire()
        {
            PooledObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(WeaponType.GetDescription());
            Bullet bullet = pooledObject?.GetComponent<Bullet>();
            if(null == bullet) {
                return;
            }

UnityEngine.Debug.LogError("TODO: fire machinegun");
            //bullet.Damage = damage;
// TODO: other stuff
        }
    }
}
