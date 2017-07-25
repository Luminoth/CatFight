using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using CatFight.Data;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Players.Schematics
{
    public static class SchematicSlotFactory
    {
        [CanBeNull]
        public static SchematicSlot Create(SchematicSlotData slotData)
        {
            switch(slotData.Type)
            {
            case SchematicSlotData.SchematicSlotTypeBrain:
                return new BrainSchematicSlot(slotData);
            case SchematicSlotData.SchematicSlotTypeWeapon:
                return new WeaponSchematicSlot(slotData);
            case SchematicSlotData.SchematicSlotTypeArmor:
                return new ArmorSchematicSlot(slotData);
            case SchematicSlotData.SchematicSlotTypeSpecial:
                return new SpecialSchematicSlot(slotData);
            default:
                Debug.LogError($"Invalid schematic slot type {slotData.Type}!");
                return null;
            }
        }
    }

    [Serializable]
    public abstract class SchematicSlot : INotifyPropertyChanged
    {
#region Events
        public event PropertyChangedEventHandler PropertyChanged;
#endregion

        [SerializeField]
        [Util.ReadOnly]
        private SchematicSlotData _slotData;

        public SchematicSlotData SlotData { get { return _slotData; } private set { _slotData = value; } }

        [SerializeField]
        [Util.ReadOnly]
        private int _itemId;

        public int ItemId
        {
            get { return _itemId; }

            set
            {
                _itemId = value;
                Item = DataManager.Instance.GameData.GetItem(SlotData.Type, ItemId);

                OnPropertyChanged();
            }
        }

        [CanBeNull]
        public ItemData Item { get; private set; }

        public bool IsFilled => ItemId > 0;

        protected SchematicSlot(SchematicSlotData slotData)
        {
            SlotData = slotData;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
