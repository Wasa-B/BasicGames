using UnityEngine;


namespace WDefense
{
    public class EffectObject : MonoBehaviour
    {

        public void ReturnObject() => WDefenseUtility.Delete(this.gameObject);
    }
}