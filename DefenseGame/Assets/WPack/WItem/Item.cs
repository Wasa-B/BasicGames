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
    
    public abstract class EquipmentItem : Item, IEquipmentItem
    {


        public string[] GetOptions()
        {
            return null;
        }

        public void ItemUpdate()
        {
               
        }
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


    [System.Serializable]
    public class ItemStatus
    {
        public virtual ItemStatus Clone() => new ItemStatus();
    }
    

    public class ItemInventory
    {
        public List<IConsumableItem> consumableItems = new List<IConsumableItem>();
        public List<IEquipmentItem> equipmentItems = new List<IEquipmentItem>();
        public void Update()
        {
            for (int i = 0; i < equipmentItems.Count; i++) equipmentItems[i].ItemUpdate();
        }
    }
}

