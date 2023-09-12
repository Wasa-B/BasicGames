using UnityEngine;

namespace WasabiGame
{

    [System.Serializable]
    public class GunStatus : AttackStatus
    {
        [SerializeField]
        protected int ammo ;
        [SerializeField]
        protected float reloadTime ;
        [SerializeField]
        protected float angle ;
        [SerializeField]
        protected int projectileNumber ;
        [SerializeField]
        protected int pierceCount;

        [SerializeField]
        public MovementStatus projectileMoveStatus = new MovementStatus();

        public int Ammo { get => ammo;}
        public float ReloadTime { get => reloadTime;}
        public float Angle { get => angle; }
        public int ProjectileNumber { get => projectileNumber;}
        public int PierceCount { get => pierceCount;}
    }
    [CreateAssetMenu(fileName = "Gun", menuName = "Command/Weapon/Gun")]
    public class GunCommand: Weapon
    {
        [SerializeField]
        GunStatus gunStatus;
        public AttackStatus originStatus => gunStatus;

        GunStatus status;

        public override AttackStatus Status => status;
        public ProjectileControl projectilePrefab;
        public PoolObject FireEffect;
        public float fireEffectOffset;


        int lastMaxAmmo;
        int MaxAmmo => status.Ammo;
        int ammo;
        int Ammunition { get { return ammo; } set { ammo = value; ammoUpdate?.Invoke(ammo, MaxAmmo); } }

        float maxReloadTime => gunStatus.ReloadTime;
        float currentReloadTime;
        float reloadTime { get => currentReloadTime; set { currentReloadTime = value; reloadAction?.Invoke(1f - currentReloadTime / maxReloadTime); } }

        internal bool reloadStart = false;
        internal event System.Action<float> reloadAction;
        internal event System.Action<int, int> ammoUpdate;
        void ReLoad()
        {
            Ammunition = MaxAmmo;
            ReloadTimeReset();
        }
        void ReloadTimeReset()
        {
            reloadTime = maxReloadTime;
        }

        protected override void OnStart()
        {
            base.OnStart();
            lastMaxAmmo = MaxAmmo;
            ReLoad();
        }

        public void ReLoadStart()
        {
            ReloadTimeReset();
            reloadStart = true;
        }
        protected override void PreAttack()
        {
            if(lastMaxAmmo < MaxAmmo)
            {
                lastMaxAmmo = MaxAmmo;
                ammoUpdate?.Invoke(ammo, MaxAmmo);
            }
            if (Ammunition > MaxAmmo)
                Ammunition = MaxAmmo;

            
        }
        protected override void AfterAttack()
        {
            if (reloadStart)
            {
                reloadTime -= Time.fixedDeltaTime;
                if(reloadTime <= 0)
                {
                    ReLoad();
                    reloadStart=false;
                }
            }
        }
        protected override bool CheckWeapon()
        {
            if(Ammunition == 0)
            {
                WeaponUse(false);
                return false;
            }
            return true;
        }
        protected override void Attack(Vector2 dir)
        {
            

            reloadStart = false;
            reloadTime = maxReloadTime;
            Ammunition -= 1;

            dir.Normalize();
            var count = status.ProjectileNumber;
            var angle = status.Angle;
            if (count > 1)
            {
                for (int i = 0; i < count; i++)
                {
                    var cAngle = -angle + 2 * angle * i / (count - 1);

                    if (count == 1) cAngle = 0;

                    var ndir = Quaternion.AngleAxis(cAngle, Vector3.forward) * dir;
                    Fire(ndir);
                }
            }
            else
            {
                Fire(dir);
            }

            PlayWeaponSound();
        }

        public virtual void Fire(Vector2 dir)
        {
            var bt = PoolManager.GenerateObject(projectilePrefab);
            bt.UpdateStatus(status, gameObject, dir);
            bt.transform.rotation = Quaternion.FromToRotation(Vector2.up,dir);
            bt.transform.position = gameObject.transform.position + (Vector3)dir *fireEffectOffset;
            bt.Fire();
            GunFire(dir);
        }

        void GunFire(Vector2 dir)
        {
            var gunfireObj = PoolManager.GenerateObject(FireEffect);
            gunfireObj.transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);
            gunfireObj.transform.position = gameObject.transform.position + (Vector3)dir * fireEffectOffset;
        }

        public override void SetStatus(AttackStatus attackStatus)
        {
            status = attackStatus as GunStatus;
        }
    }
}