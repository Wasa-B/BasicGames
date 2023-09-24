using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WItem
{
    
    public class ItemEffect : ScriptableObject
    {
        public virtual void EffectUse(Item item) { }
    }
}

