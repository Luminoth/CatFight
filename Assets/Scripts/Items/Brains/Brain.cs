using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Brains
{
    public class Brain : Item
    {
        private const string ResourcePath = "brains";

        public string BrainType { get; }

        public Brain(string type)
        {
            BrainType = type;
        }

        [CanBeNull]
        public FsmTemplate LoadBrain()
        {
            return Resources.Load<FsmTemplate>(ResourcePath + "/" + BrainType);
        }
    }
}
