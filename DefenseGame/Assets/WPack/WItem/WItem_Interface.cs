using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WItem
{
    public interface IItem
    {
        public string ItemCode { get; }
        public string Name { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        public IItem Item { get; }

        public GameObject Owner { get; }
        public ItemStatus Status { get; }
    }

    public interface IEquipmentItem : IItem
    {
        public string[] GetOptions();
        public void ItemUpdate();
    }

    public interface IConsumableItem : IItem
    {

    }

    public interface IItemEffect
    {
        public void Init(IItem item);
        public string EffectCode();
        public void EffectUpdate();
    }

    
}