using System;
using System.Collections;

using CatFight.Data;
using CatFight.Fighters;
using CatFight.Util.ObjectPool;

using UnityEngine;

namespace CatFight.Items.Specials
{
    [Serializable]
    public sealed class Missiles : Special
    {
        public Missiles(Fighter fighter, SpecialData.SpecialDataEntry specialData, int totalUses)
            : base(fighter, specialData, totalUses)
        {
        }

        protected override void DoUse()
        {
            Fighter target = FighterManager.Instance.GetFighterNotOnTeam(Fighter.Team.Id);
            if(null != target) {
                Fighter.StartCoroutine(Spawner(target));
            }
        }

        private IEnumerator Spawner(Fighter target)
        {
            for(int i=0; i<SpecialData.SpawnAmount; ++i) {
                PooledObject pooledObject = ObjectPoolManager.Instance.GetPooledObject(Data.SpecialData.GetAmmoPool(SpecialType), FighterManager.Instance.AmmoContainer.transform);
                Missile missile = pooledObject?.GetComponent<Missile>();
                missile?.Initialize(Fighter, target, SpecialData.Type, SpecialData.Damage);

                yield return new WaitForSeconds(SpecialData.SpawnRateSeconds);
            }
        }
    }
}
