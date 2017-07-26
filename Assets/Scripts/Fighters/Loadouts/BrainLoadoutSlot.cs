using System;
using System.Collections.Generic;

using CatFight.Data;
using CatFight.Items.Brains;
using CatFight.Players.Schematics;
using CatFight.Util;

using JetBrains.Annotations;

namespace CatFight.Fighters.Loadouts
{
    [Serializable]
    public sealed class BrainLoadoutSlot : LoadoutSlot
    {
        private readonly Dictionary<int, int> _brainTypeVotes = new Dictionary<int, int>();

        [CanBeNull]
        public Brain Brain { get; private set; }

        public BrainLoadoutSlot(SchematicSlotData slotData)
            : base(slotData)
        {
        }

        public override void Process(SchematicSlot schematicSlot)
        {
            BrainSchematicSlot brainSlot = (BrainSchematicSlot)schematicSlot;
            if(null == brainSlot.BrainItem) {
                return;
            }

            int currentCount = _brainTypeVotes.GetOrDefault(brainSlot.BrainItem.Id);
            _brainTypeVotes[brainSlot.BrainItem.Id] = currentCount + 1;
        }

        public override void Complete()
        {
            int winnerType = VoteHelper.GetWinner(_brainTypeVotes);
            BrainData brainData = DataManager.Instance.GameData.GetItem(ItemData.ItemTypeBrain, winnerType) as BrainData;
            if(null == brainData) {
                return;
            }

            Brain = new Brain(brainData.Type);
        }
    }
}
