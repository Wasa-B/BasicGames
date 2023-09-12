using UnityEngine;

using WasabiGame;

namespace CartHero
{
    [System.Serializable]
    public class ItemStatus
    {
        public GunStatus GunStatus;

        [SerializeField]
        int hpMax = 0;
        [SerializeField]
        float moveSpeed = 0;

        public int HpMax { get => hpMax; }
        public float MoveSpeed { get => moveSpeed; }
    }

    [CreateAssetMenu(fileName = "Item", menuName = "Item/Itme")]
    public class PlayerItemBase : ScriptableObject
    {
        public Sprite itemIcon;

        [SerializeField]
        ItemStatus itemStatus;

        public string text;

        public GunStatus weaponStatus => itemStatus.GunStatus;
        public ItemStatus status => itemStatus;
        public PlayerItemBase Clone() => Instantiate(this);
    }

}