using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WItem
{
    
    public class ItemEffect : ScriptableObject
    {
        protected Item ownItem;

        internal virtual void Init(Item owner)
        {

        }
    }
}

