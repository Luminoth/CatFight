using System;
using CatFight.Util;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Brains
{
    [Serializable]
    public sealed class Brain : Item
    {
        private const string ResourcePath = "brains";

        [SerializeField]
        [ReadOnly]
        private string _brainType;

        public string BrainType { get { return _brainType; } private set { _brainType = value; } }

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
