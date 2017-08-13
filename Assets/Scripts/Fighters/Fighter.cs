using System.Collections.Generic;
using System.Globalization;

using CatFight.Data;
using CatFight.Fighters.Loadouts;
using CatFight.Util;

using UnityEngine;

namespace CatFight.Fighters
{
    [RequireComponent(typeof(PlayMakerFSM))]
    public sealed class Fighter : MonoBehavior
    {
        [SerializeField]
        private TextMesh _healthText;

        [SerializeField]
        [ReadOnly]
        private TeamData.TeamDataEntry _team;

        public TeamData.TeamDataEntry Team => _team;

        [SerializeField]
        [ReadOnly]
        private string _fighterName;

        public string FighterName => _fighterName;

        [SerializeField]
        private TextMesh _nameText;

        [SerializeField]
        private GameObject _missileTargetObject;

        [SerializeField]
        [ReadOnly]
        private Loadout _loadout;

        public Loadout Loadout => _loadout;

        [SerializeField]
        [ReadOnly]
        private FighterStats _stats;

        public FighterStats Stats => _stats;

        [SerializeField]
        private FighterLoadoutSlot[] _loadoutSlots;

        private readonly Dictionary<int, FighterLoadoutSlot> _loadoutSlotMap = new Dictionary<int, FighterLoadoutSlot>();

        [SerializeField]
        [ReadOnly]
        private int _targetingMissiles;

        private PlayMakerFSM _fsm;

#region Unity Lifecycle
        private void Awake()
        {
            _fsm = GetComponent<PlayMakerFSM>();

            _loadout = new Loadout(this);
            _stats = new FighterStats(this);
        }

        private void Update()
        {
            _healthText.text = Stats.CurrentHealth.ToString(CultureInfo.InvariantCulture);

#if UNITY_EDITOR
            if(Input.GetKey(KeyCode.Space)) {
                Stats.FireAllWeapons();
            }

            if(Input.GetKeyDown(KeyCode.X)) {
                Stats.UseSpecial(SpecialData.SpecialType.Missiles);
            }

            if(Input.GetKeyDown(KeyCode.C)) {
                Stats.UseSpecial(SpecialData.SpecialType.Chaff);
            }
#endif
        }
#endregion

        public void Initialize(TeamData.TeamDataEntry team, string fighterName, FighterData fighterData)
        {
            _team = team;

            _fighterName = fighterName;
            _nameText.text = FighterName;

            gameObject.name = _team.Name;
            gameObject.layer = team.Layer;

            _loadout.Initialize();
            Debug.Log(Loadout);

            _loadoutSlotMap.Clear();
            foreach(FighterLoadoutSlot slot in _loadoutSlots) {
                slot.Initialize(_loadout);
                _loadoutSlotMap.Add(slot.SlotId, slot);
            }

            _stats.Initialize(_loadout, _fsm);
            Debug.Log(Stats);
        }

#region Ammo Spawns
        public Transform GetWeaponAmmoSpawnTransform(int slotId)
        {
            FighterLoadoutSlot slot = _loadoutSlotMap.GetOrDefault(slotId);
            WeaponLoadoutSlotItem slotItem = slot?.SlotItem as WeaponLoadoutSlotItem;
            return slotItem?.AmmoTransform ?? transform;
        }

        public Transform GetSpecialAmmoSpawnTransform()
        {
            return transform;
        }
#endregion

#region Missiles
        public void AddMissileTarget()
        {
            ++_targetingMissiles;
            //_missileTargetObject.SetActive(true);
        }

        public void RemoveMissileTarget()
        {
            --_targetingMissiles;
            if(_targetingMissiles <= 0) {
                _targetingMissiles = 0;
                //_missileTargetObject.SetActive(false);
            }
        }
#endregion
    }
}
