using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WDefense
{
    public enum DamageType {Normal, Magic, True}
    public enum AdditionalAbility {
        //Viability
        HPRate, HealRate,

        //Damage
        NormalATK, MagicATK, SpecialATK,
        DMG, NormalDMG, MagicDMG, SpecialDMG, SummonDMG,

        //Critical
        CriticalRate, CriticalDMG,

        //AttackBehavior
        CoolDownRate, ShotIntervalRate, ShotCount, ShotErrorAngle,

        //Projectile
        ProjectileCount, ProjectileSPDRate, ProjectileInterAngle, ProjectileErrorAngle,

        //Summon
        SummonHPRate, SummonAttackCooldownRate, SummonMaxCount,

        //Special
        SpecialDurationRate, SpecialCoefficientRate
    }
    [System.Serializable]
    public class GameData
    {
        
    }

    [System.Serializable]
    public struct CharacterData
    {
        public string name;
        public string description;
        public Sprite image;
        public string objectName;
        public string[] initItems;
        public CharacterStatus status;
        public AdditionalStatus additionalStatus;
        public string[] specificityName;
    }

    public struct CharacterStatus
    {
        public int hp;
        public int normalATK;
        public int magicATK;
        public int specialATK;
    }


    public struct ProjectileStatus
    {
        public string name;
        public int speed;
        public float size;
    }
    
    [System.Serializable]
    public struct AdditionalStatus
    {
        [Header("Viability")]
        public float HPRate;
        public float HealRate;

        [Header("Damage")]
        public int ATK;
        public int NormalATK;
        public int MagicATK;
        public int SpecialATK;
        public float DMG;
        public float NormalDMG;
        public float MagicDMG;
        public float SpecialDMG;
        public float SummonDMG;

        [Header("Critical")]
        public float CritcalRate;
        public float CritcalDMG;

        [Header("Attack")]
        public float CoolDownRate;
        public float ShotIntervalRate;
        public int ShotCount;
        public float ShotErrorAngle;

        [Header("Projectile")]
        public int ProjectileCount;
        public float ProjecTileSPDRate;
        public float ProjecTileInterAngle;
        public float ProjecTileErrorAngle;

        [Header("Summon")]
        public float SummonHPRate;
        public float SummonAttackCooldownRate;
        public int SummonMaxCount;

        [Header("Special")]
        public float SpecialDurationRate;
        public float SpecialCoefficientRate;
    }

    public class ItemDataManager
    {
        
        public void LoadData()
        {

        }
    }

    [System.Serializable]
    public class ItemData
    {
        public string name;
        public string description;
        public Sprite image;
        public AdditionalStatus additionalStatus;
    }
    [System.Serializable]
    public class WeaponItemData : ItemData
    {
        public enum WeaponType { Summon, Howitzer, Line }

        public WeaponType type;
        public WeaponStatusData status;
    }

    public struct WeaponStatusData
    {
        public string objectName;
        public string[] tags;
        
        [Header("Damage")]
        public int normalATK;
        public int magicATK;
        public int specialATK;

        [Header("Critical")]
        public float CriticalRate;
        public float CriticalDMG;

        [Header("Attack")]
        public float Cooldown;
        public float FirstDelay;
        public int ShotCount;
        public float ShotInterval;
        public float ShotErrorAngle;
        public bool AimUpdate;

        [Header("Projectile")]
        public float ProjectileSPD;
        public string ProjectileName;
        public int ProjectileCount;
        public float ProjectileSize;
        public float ProjectileErrorAngle;
        public float ProjectileInterAngle;
        public float ProjectileGuidedForce;

        [Header("Summon")]
        public string SummonObjectName;
        public int SummonHP;
        public int SummonMaxCount;
        public float SummonSPD;
        public float SummonAttackCooldown;

        [Header("Special")]
        public string SpecialEffectName;
        public float SpecialDuration;
        public int SpecialCoefficient;
    }

    [System.Serializable]
    public struct ItemUnlockData
    {
        public string conditions;
        public string[] items;
    }

    [System.Serializable]
    public struct SkillData
    {

    }
    [System.Serializable]
    public struct AchievementData
    {

    }
    [System.Serializable]
    public class UserData
    {

    }
}