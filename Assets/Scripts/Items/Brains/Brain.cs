using System;

using CatFight.Data;
using CatFight.Util;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Items.Brains
{
    [Serializable]
    public sealed class Brain : Item
    {
        private const string ResourcePath = "Brains";

        [SerializeField]
        [ReadOnly]
        private BrainData.BrainType _brainType;

        public BrainData.BrainType BrainType { get { return _brainType; } private set { _brainType = value; } }

        public Brain(BrainData.BrainType type)
        {
            BrainType = type;
        }

// TODO: instead of loading from resources like this
// we can just hook the FSM to the data serialized object
        [CanBeNull]
        public FsmTemplate LoadBrain()
        {
            return Resources.Load<FsmTemplate>(ResourcePath + "/" + BrainType);
        }

        public override string ToString()
        {
            return $"Brain: {BrainType}";
        }
    }
}
