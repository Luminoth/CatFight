using CatFight.Data;
using CatFight.Fighters;
using CatFight.Util;

using UnityEngine;
using UnityEngine.UI;

namespace CatFight.Stages.Arena
{
    public sealed class FighterStatsCard : MonoBehavior
    {
        [SerializeField]
        private Text _fighterNameText;

        [SerializeField]
        private Text _missilesRemainingText;

        [SerializeField]
        private Text _chaffRemainingText;

        private Fighter _fighter;

#region Unity Lifecycle
        private void Update()
        {
            _missilesRemainingText.text = _fighter.Stats.GetSpecialRemaining(SpecialData.SpecialType.Missiles).ToString();
            _chaffRemainingText.text = _fighter.Stats.GetSpecialRemaining(SpecialData.SpecialType.Chaff).ToString();
        }
#endregion

        public void Initialize(Fighter fighter)
        {
            _fighter = fighter;
            _fighterNameText.text = _fighter.FighterName;
        }
    }
}
