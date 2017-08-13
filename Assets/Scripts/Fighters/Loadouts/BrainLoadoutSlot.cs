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

        public BrainLoadoutSlot(Fighter fighter, SchematicSlotData slotData)
            : base(fighter, slotData)
        {
        }

        public override void Process(SchematicSlot schematicSlot)
        {
            BrainSchematicSlot brainSlot = (BrainSchematicSlot)schematicSlot;
            BrainData.BrainDataEntry brainItem = brainSlot.BrainItem;
            if(null == brainItem) {
                return;
            }

            int currentCount = _brainTypeVotes.GetOrDefault(brainItem.Id);
            _brainTypeVotes[brainItem.Id] = currentCount + 1;
        }

        public override void Complete()
        {
            int winnerType = VoteHelper.GetWinner(_brainTypeVotes);
            BrainData.BrainDataEntry brainData = DataManager.Instance.GameData.Brains.Entries.GetOrDefault(winnerType);
            if(null == brainData) {
                return;
            }

            Brain = new Brain(brainData);
        }
    }
}
