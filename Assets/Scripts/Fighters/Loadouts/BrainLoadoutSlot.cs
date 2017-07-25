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
        private readonly Dictionary<string, int> _brainTypeVotes = new Dictionary<string, int>();

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

            int currentCount = _brainTypeVotes.GetOrDefault(brainSlot.BrainItem.Type);
            _brainTypeVotes[brainSlot.BrainItem.Type] = currentCount + 1;
        }

        public override void Complete()
        {
            string winnerType = VoteHelper.GetWinner(_brainTypeVotes);
            if(string.IsNullOrWhiteSpace(winnerType)) {
                return;
            }

            Brain = new Brain(winnerType);
        }
    }
}
