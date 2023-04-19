using UnityEngine;

namespace WasabiGame
{

    [System.Serializable]
    public class GunStatus
    {
        public PoolObject FireEffect;
        public float fireEffectOffset;
    }
    [CreateAssetMenu(fileName = "Gun", menuName = "Command/Weapon/Gun")]
    public class GunCommand: WeaponCommand
    {
        public GunStatus gunStatus;
        public override void Attack(Vector2 dir)
        {
            dir.Normalize();
            var count = weaponStatus.projectileNumber;
            var angle = weaponStatus.angle;
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
            bt.UpdateStatus(weaponStatus, gameObject, dir);
            bt.transform.rotation = Quaternion.FromToRotation(Vector2.up,dir);
            bt.transform.position = gameObject.transform.position + (Vector3)dir * gunStatus.fireEffectOffset;
            bt.Fire();
            GunFire(dir);
        }

        void GunFire(Vector2 dir)
        {
            var gunfireObj = PoolManager.GenerateObject(gunStatus.FireEffect);
            gunfireObj.transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);
            gunfireObj.transform.position = gameObject.transform.position + (Vector3)dir * gunStatus.fireEffectOffset;
        }
    }
}