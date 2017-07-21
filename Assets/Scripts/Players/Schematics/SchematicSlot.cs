using System.ComponentModel;
using System.Runtime.CompilerServices;

using CatFight.Data;

using JetBrains.Annotations;

using UnityEngine;

namespace CatFight.Players.Schematics
{
    public static class SchematicSlotFactory
    {
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

    public abstract class SchematicSlot : INotifyPropertyChanged
    {
#region Events
        public event PropertyChangedEventHandler PropertyChanged;
#endregion

        public SchematicSlotData SlotData { get; }

        private int _itemId;

// TODO: subclasses need to validate that the item exists
        public int ItemId
        {
            get { return _itemId; }

            set
            {
                _itemId = value;
                OnPropertyChanged();
            }
        }

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
