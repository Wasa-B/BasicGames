using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WDefense
{
    [System.Serializable]
    public class ItemStatus : WItem.ItemStatus
    {
        [SerializeField]
        int dmg = 0;
        [SerializeField]
        float spd = 0;
        [SerializeField]
        float range = 0;
        [SerializeField]
        float cooldownSpd = 0;
        [SerializeField]
        float rangeRate = 1;
        [SerializeField]
        float dmgRate = 1;

        public virtual int DMG {get => dmg;}
        public virtual float SPD { get => spd;}
        public virtual float Range => range;
        public virtual float CooldownSpeedRate => cooldownSpd;

        public virtual float RangeRate => rangeRate;
        public virtual float DMGRate => dmgRate;
        
        
    }
    [System.Serializable]
    public class WeaponStatus : WItem.ItemStatus
    {
        [SerializeField]
        int weaponATK = 0;
        [SerializeField]
        float weaponSPD = 0;
        [SerializeField]
        float weaponRange = 0;

        public int WeaponATK => weaponATK;
        public float WeaponSPD => weaponSPD;
        public float WeaponRange => weaponRange;
    }

}