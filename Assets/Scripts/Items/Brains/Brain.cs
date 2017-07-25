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

        public FsmTemplate LoadBrain()
        {
            return Resources.Load<FsmTemplate>(ResourcePath + "/" + BrainType);
        }
    }
}
