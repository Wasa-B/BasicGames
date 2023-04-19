using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace WasabiGame
{

    [System.Serializable]
    public class AttackStatus
    {
        [SerializeField]
        int damage = 10;
        [SerializeField]
        float attackSpeed = 100;
        [SerializeField]
        float size = 1;
        [SerializeField]
        int ammunition = 6;
        [SerializeField]
        float reloadTime = .5f;

        [SerializeField]
        float angle = 10;
        [SerializeField]
        int projectileNumber = 1;
        [SerializeField]
        float range = 6;

        [SerializeField]
        MovementStatus movementStatus;

        [SerializeField]
        LayerMask layerMask;

        public virtual int Damage { get => damage; set { damage = value; updateStatus?.Invoke(); }}
        public virtual float AttackSpeed { get => attackSpeed; set { attackSpeed = value; updateStatus?.Invoke(); }}
        public virtual float Size { get => size; set { size = value; updateStatus?.Invoke(); }}
        public virtual int Ammo { get => ammunition; set { ammunition = value; updateStatus?.Invoke(); }}
        public virtual float ReloadTime { get => reloadTime; set { reloadTime = value; updateStatus?.Invoke(); }}
        public virtual int ProjectileNumber { get => projectileNumber; set { projectileNumber = value; updateStatus?.Invoke();} }
        public virtual float Range { get => range; set { range = value; updateStatus?.Invoke(); }}
        public virtual float Angle { get => angle; set { angle = value; updateStatus?.Invoke(); }}
        public virtual LayerMask LayerMask { get => layerMask; set { layerMask = value; updateStatus?.Invoke(); }}
        public event System.Action updateStatus;

        
    }

    [System.Serializable]
    public class WeaponStatus
    {
        
        public AttackStatus status;
        public AttackStatus characterStatus;

        public int damage => status.Damage + characterStatus.Damage;
        public float attackSpeed => status.AttackSpeed + characterStatus.AttackSpeed;
        public float size => status.Size + characterStatus.Size;
        public int ammunition => status.Ammo + characterStatus.Ammo;
        public float reloadTime => status.ReloadTime + characterStatus.ReloadTime;

        public float angle => status.Angle + characterStatus.Angle;
        public int projectileNumber => status.ProjectileNumber + characterStatus.ProjectileNumber;
        public float range => status.Range + characterStatus.Range;
        public MovementInfo movementInfo;
        public LayerMask layerMask => characterStatus.LayerMask;
        public void UpdateStatus()
        {

        }
        
        internal Vector2 dir;
    }

    [CreateAssetMenu(fileName = "Base", menuName = "Command/Weapon/Base")]
    public class WeaponCommand : BehaviourCommand
    {
        public WeaponStatus weaponStatus;
        public ProjectileControl projectilePrefab;
        public ClipStatus weaponSound;

        float AttackSpeed =>  weaponStatus.attackSpeed; 

        float currentTime;
        int lastMaxAmmo;
        int MaxAmmo => weaponStatus.ammunition;
        int ammo;
        int Ammunition { get { return ammo; } set { ammo = value; ammoUpdate?.Invoke(ammo, MaxAmmo); } }


        float maxReloadTime => weaponStatus.reloadTime;
        float currentReloadTime;
        float reloadTime { get => currentReloadTime; set { currentReloadTime = value; reloadAction?.Invoke(1f - currentReloadTime / maxReloadTime); } }

        internal Vector2 dir = Vector2.up;

        internal bool weaponUse = false;
        internal bool reloadStart = false;
        internal event System.Action<float> reloadAction;
        internal event System.Action<int, int> ammoUpdate;
        internal event System.Action attackAction;

        protected override void OnEnd()
        {
        }
        void ReLoad()
        {
            Ammunition = MaxAmmo;
            ReloadTimeReset();
        }
        void ResetTime()
        {
            currentTime = AttackSpeed > 0 ? (100 / AttackSpeed) : 2;
        }

        protected override void OnStart()
        {
            lastMaxAmmo = MaxAmmo;
            ResetTime();
            ReLoad();
        }
        void ReloadTimeReset()
        {
            reloadTime = maxReloadTime;
        }
        public void ReLoadStart()
        {
            ReloadTimeReset();
            reloadStart = true;
        }

        protected override State OnUpdate()
        {
            if (lastMaxAmmo != MaxAmmo)
            {
                lastMaxAmmo = MaxAmmo;
                ammoUpdate?.Invoke(ammo, MaxAmmo);
            }
            if (Ammunition > MaxAmmo)
                Ammunition = MaxAmmo;
            currentTime -= Time.fixedDeltaTime;
            if (currentTime <= 0)
            {
                if (Ammunition == 0) weaponUse = false;

                if (weaponUse)
                {
                    //Cancle Reload
                    reloadStart = false;
                    reloadTime = maxReloadTime;

                    Ammunition -= 1;
                    weaponUse = false;
                    ResetTime();
                    Attack(dir);
                    attackAction?.Invoke();
                }

                if (reloadStart)
                {
                    reloadTime -= Time.fixedDeltaTime;

                    if (reloadTime <= 0)
                    {
                        ReLoad();
                        reloadStart = false;
                    }
                }


            }
            weaponUse = false;
            return state;
        }

        public virtual void Attack(Vector2 dir)
        {

            var bt = PoolManager.GenerateObject(projectilePrefab);
            bt.UpdateStatus(weaponStatus, gameObject, dir);

            bt.Fire();
            PlayWeaponSound();
        }

        public override BehaviourCommand Clone(GameObject gameObject)
        {
            var clone = base.Clone(gameObject) as WeaponCommand;

            return clone;
        }

        protected void PlayWeaponSound()
        {
            weaponSound.Play();
        }
    }
}