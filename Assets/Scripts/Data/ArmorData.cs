using System;

namespace CatFight.Data
{
    [Serializable]
    public sealed class ArmorData : Data
    {
        public override void Process()
        {
        }

        public override string ToString()
        {
            return $"Armor({Id}: {Name})";
        }
    }
}
