using System;

namespace CatFight.Items.Armor
{
    [Serializable]
    public sealed class AntiMachineGun : Armor
    {
        public override string ArmorType => ArmorTypeMachineGun;
    }
}
