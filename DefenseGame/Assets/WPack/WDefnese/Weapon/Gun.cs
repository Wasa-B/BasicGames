using System.Collections;
using UnityEngine;


namespace WDefense
{

    [CreateAssetMenu(fileName = "Gun",menuName = "Weapon/Gun")]
    public class Gun : RangeWeapon
    {
        public float range = 10;
        public float speed = 5;
        public int damage = 5;

        public float angleRange = 5;
        public bool updateDirection = false;

        public int shotsCount = 1;
        public float interval = .1f;
        public float preDelay = .1f;

        protected override int ShotCount() => shotsCount;
        protected override float ShotInterval() => interval;
        protected override bool UpdateDirection() => updateDirection;
        public override float Range => range;
        public override int WeaponATK => damage;
        public override float WeaponRange => range;
        public override float WeaponSPD => speed;
        public override Vector2 GetVelocity() => WUtility.Math2D.AngleRange(Direction(), angleRange);
        public override bool VelocityUpdate(ref Vector2 velocity) => false;
    }
}