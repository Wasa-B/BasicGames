using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WItem
{
    public enum Values {Common, Rare, Unique, Legendary, Epic}
    public enum ItemTarget {Self, Enemy, All}

    public abstract class Base_Item : ScriptableObject, IItem
    {
        [SerializeField]
        private string itemCode = "Base_0";
        public virtual string ItemCode => itemCode;
        public virtual string Name => "DefaultName";
        public virtual string Description => "DefaultDescription";
        public virtual Sprite Icon => null;
        public virtual IItem Item => this;
        public GameObject Owner => null;

        public virtual ItemStatus Status { get; protected set; }
        public virtual T GetStatus<T>() where T : ItemStatus
        {
            return (T)Status;
        }
    }

    public class Item : Base_Item
    {
        [SerializeField]
        protected string itemName;
        [SerializeField]
        protected string description;
        [SerializeField]
        protected Sprite icon;
        public override string Name => itemName;
        public override string Description => description;
        public override Sprite Icon => icon;
    }
    
    public abstract class EquipmentItem : Item
    {
        public virtual void ItemUpdate() { }
    }

    public abstract class ConsumableItem : Item
    {
        public virtual void UseItem() { }
    }

    [System.Serializable]
    public class ItemUpgradeTree
    {
        int level = 0;
        public List<UpgradeItem> Upgrades;
    }

    public class UpgradeItem : Item
    {
        public Item baseItem;

    }

    public class WeaponItem : EquipmentItem
    {

    }


    [System.Serializable]
    public class ItemStatus
    {
        public virtual ItemStatus Clone() => new ItemStatus();
    }

    [System.Serializable]
    public class Inventory
    {
        public virtual void AddItem(Item item) { }
        public virtual void RemoveItem(Item item) { }
    }

    [System.Serializable]
    public class EquipmentInventory
    {
        public virtual void ItemEffectUpdate() { }
    }
}

