using System;

using CatFight.Data;

namespace CatFight.Items.Brains
{
    [Serializable]
    public sealed class Brain : Item
    {
        private readonly BrainData.BrainDataEntry _brainData;

        public FsmTemplate Template => _brainData.Template;

        public Brain(BrainData.BrainDataEntry brainData)
        {
            _brainData = brainData;
        }
    }
}
