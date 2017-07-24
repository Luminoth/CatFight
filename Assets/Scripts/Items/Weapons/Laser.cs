namespace CatFight.Items.Weapons
{
    public sealed class Laser : Weapon
    {
        public override string WeaponType => WeaponTypeLaser;

        public override void SetStrength(int strength)
        {
UnityEngine.Debug.LogError("TODO: set laser strength");
        }

        public override void Fire()
        {
UnityEngine.Debug.LogError("TODO: fire laser");
        }
    }
}
