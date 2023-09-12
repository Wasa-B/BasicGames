using UnityEngine;


namespace WDefense
{
    [CreateAssetMenu(fileName = "Howitzer", menuName = "Weapon/Howitzer")]
    public class Howitzer : Gun
    {



        public float gravity = 9.8f;
        public float minRange = 1;
        public bool indirect = true;

        float maxRange = 0;
        float maxAngle;

        public override void Init(IWeaponUser owner)
        {
            base.Init(owner);
            maxRange = WUtility.Math2D.ProjectileMaxDistance(owner.Position(), standard.y, WeaponSPD, gravity);
            maxAngle = WUtility.Math2D.ProjectileMaxDistanceAngle(owner.Position(), standard.y, WeaponSPD, gravity);
            Debug.Log("MaxRange : "+maxRange + ", MaxAngle" + maxAngle);
        }

        public override float Range => maxRange;

        public override Vector2 DetectOrigin()
        {
            var o = base.DetectOrigin();
            return o + Vector2.right * minRange;
        }

        public override Vector2 Direction()
        {
            
            if(Mathf.Abs(targetPos.x - owner.Position().x )> maxRange) return WUtility.Math2D.Deg2Vector(maxAngle);
            return WUtility.Math2D.ProjectileVelocity(owner.Position(), targetPos, WeaponSPD, gravity, indirect).normalized;
        }

        public override Vector2 GetVelocity()
        {
            //return WUtility.Math2D.ProjectileVelocity(owner.Position(), targetPos, Vector2.one.normalized, .5f);
            return WUtility.Math2D.AngleRange(Direction(), angleRange) * WeaponSPD;
        }

        public override bool VelocityUpdate(ref Vector2 velocity)
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
            return true;
        }
    }
}