using System.Collections.Generic;
using UnityEngine;
using WasabiGame;
namespace CartHero
{


    public partial class PlayerCharacter
    {
        [SerializeField]
        class PlayerWeaponStatus : GunStatus
        {

            List<GunStatus> itemsStatus = new List<GunStatus>();

            internal void AddItems(GunStatus item)
            {

                itemsStatus.Add(item);
                ResetStatus();
            }
            internal void RemoveItems(GunStatus gunStatus)
            {
                if (itemsStatus.Contains(gunStatus))
                {
                    itemsStatus.Remove(gunStatus);
                    ResetStatus();
                }
            }

            void ResetStatus()
            {
                damage = 0;
                size = 0;
                attackSpeed = 0;
                range = 0;
                ammo = 0;
                reloadTime = 0;
                angle = 0;
                pierceCount = 0;
                projectileNumber = 0;
                projectileMoveStatus.acc = 0;
                projectileMoveStatus.speed = 0;
                projectileMoveStatus.maxSpeed = 0;

                foreach (GunStatus item in itemsStatus)
                {
                    damage += item.Damage;
                    size += item.Size;
                    attackSpeed += item.AttackSpeed;
                    range += item.Range;
                    ammo += item.Ammo;
                    reloadTime += item.ReloadTime;
                    angle += item.Angle;
                    pierceCount += item.PierceCount;
                    projectileNumber += item.ProjectileNumber;
                    projectileMoveStatus.acc += item.projectileMoveStatus.acc;
                    projectileMoveStatus.speed += item.projectileMoveStatus.speed;
                    projectileMoveStatus.maxSpeed += item.projectileMoveStatus.maxSpeed;
                }
            }

        }
    }
}