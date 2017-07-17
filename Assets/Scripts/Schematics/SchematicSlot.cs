using System.ComponentModel;
using System.Runtime.CompilerServices;

using CatFight.Data;

using JetBrains.Annotations;

namespace CatFight.Schematics
{
    public static class SchematicSlotFactory
    {
        public static SchematicSlot Create(SchematicSlotData data)
        {
            switch(data.type)
            {
            case SchematicSlotData.SchematicSlotTypeWeapon:
                return new WeaponSchematicSlot();
            case SchematicSlotData.SchematicSlotTypeArmor:
                return new ArmorSchematicSlot();
            case SchematicSlotData.SchematicSlotTypeCore:
                return new CoreSchematicSlot();
            default:
                return null;
            }
        }
    }

    public abstract class SchematicSlot : INotifyPropertyChanged
    {
#region Events
        public event PropertyChangedEventHandler PropertyChanged;
#endregion

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
