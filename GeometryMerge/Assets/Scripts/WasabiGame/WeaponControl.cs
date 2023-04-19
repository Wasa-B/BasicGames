using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace WasabiGame
{
    public class WeaponControl : MonoBehaviour
    {
        WeaponCommand weapon;
        public float autoReloadTime = .5f;

        public LayerMask findLayer;
        float VisionRadius => weapon.weaponStatus.range;

        internal void SetWeapon(WeaponCommand wapon)
        {
            this.weapon = wapon;
            weapon.ammoUpdate += AmmoUpdate;
        }

        private void Awake()
        {
            
        }
        private void Update()
        {
            FindEnemy();
        }
        private void FixedUpdate()
        {
            weapon.Update();

        }
        internal void WeaponUse()
        {
            weapon.weaponUse = true;
        }

        internal void AmmoUpdate(int ammo, int maxAmmo)
        {
            if(ammo < maxAmmo)
            {
                CancelInvoke(nameof(Reload));
                Invoke(nameof(Reload), autoReloadTime);
            }
        }

        internal void Reload()
        {
            weapon.ReLoadStart();
        }

        void FindEnemy()
        {
            var enemies = Physics2D.OverlapCircleAll(transform.position, VisionRadius, findLayer);
            if (enemies.Length > 0)
            {
                Vector2 dir = Vector2.up;
                float distance = float.MaxValue;
                foreach (var enemy in enemies)
                {
                    if (enemy.transform.position.y < transform.position.y) continue;
                    if(distance > Vector2.Distance(transform.position, enemy.transform.position))
                    {
                        distance = Vector2.Distance(transform.position, enemy.transform.position);
                        dir = enemy.transform.position - transform.position;
                    }
                }
                if (distance != float.MaxValue)
                {
                    weapon.dir = dir;
                    //WeaponUse();
                }
                else
                    weapon.dir = Vector2.up;
            }
            else
            {
                weapon.dir = Vector2.up;
            }

            
            

        }
    }
}