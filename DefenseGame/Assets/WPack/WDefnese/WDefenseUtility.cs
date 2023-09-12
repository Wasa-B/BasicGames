using System;
using UnityEngine;


namespace WDefense
{
    public static class WDefenseUtility
    {
        public static Func<GameObject, GameObject> Generate = GameObject.Instantiate;
        public static Action<GameObject> Delete = GameObject.Destroy;

        public static LayerMask blockLayerMask;
    }
}