using System;
using System.Collections;

using CatFight.Data;
using CatFight.Fighters;
using CatFight.Util.ObjectPool;

using UnityEngine;

namespace CatFight.Items.Specials
{
    [Serializable]
    public sealed class Chaffs : Special
    {
        public Chaffs(Fighter fighter, SpecialData.SpecialDataEntry specialData, int totalUses)
            : base(fighter, specialData, totalUses)
        {
        }

        protected override void DoUse()
        {
            Fighter.StartCoroutine(Spawner());
        }

        private IEnumerator Spawner()
        {
            for(int i=0; i<SpecialData.SpawnAmount; ++i) {
                PooledObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(Data.SpecialData.GetAmmoPool(SpecialType), FighterManager.Instance.AmmoContainer.transform);
                Chaff chaff = pooledObject?.GetComponent<Chaff>();
                chaff?.Initialize(Fighter, SpecialData.Type, SpecialData.Damage);

                yield return new WaitForSeconds(SpecialData.SpawnRateSeconds);
            }
        }
    }
}
