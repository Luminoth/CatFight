namespace CatFight.Items.Weapons
{
    public sealed class MachineGun : Weapon
    {
        public override string WeaponType => WeaponTypeMachineGun;

        public override void SetStrength(int strength)
        {
UnityEngine.Debug.LogError("TODO: set machinegun strength");
        }

        public override void Fire()
        {
UnityEngine.Debug.LogError("TODO: fire machinegun");
        }
    }
}
