using System;

namespace CatFight.Data
{
    [Serializable]
    public sealed class ArmorData : Data
    {
        public override string ToString()
        {
            return $"Armor({id}: {name})";
        }
    }
}
