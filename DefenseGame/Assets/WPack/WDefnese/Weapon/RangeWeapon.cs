using UnityEngine;


namespace WDefense
{
    public class RangeWeapon : Weapon, IRangeWeapon
    {
        private enum RangeWeaponState { DetectWait, PreDelay, Shot, ShotEnd }
        RangeWeaponState gunState = RangeWeaponState.DetectWait;
        public GameObject projectile;
        public GameObject hitEffect;

        protected Vector2 dir;
        protected Vector2 targetPos;


        public override Vector2 Direction() => dir;
        public override Vector2 TargetPosition() => targetPos;
        protected virtual bool UpdateDirection() => false;
        protected virtual float PreDelay() => .1f;
        protected virtual int ProjectileCount() => 1;
        protected virtual float ShotInterval() => .1f;
        protected virtual int ShotCount() => 1;
        public virtual Vector2 GetVelocity() => dir * WeaponSPD;
        public override Vector2 DetectOrigin() => new Vector2(owner.Position().x, standard.y);

        public virtual bool VelocityUpdate(ref Vector2 velocity) => true;

        protected GameObject target;
        int _count = 0;
        public override void Attack(RaycastHit2D hit)
        {
            if (hitEffect)
            {
                var go = WDefenseUtility.Generate(hitEffect);
                go.transform.position = hit.point;
            }

            base.Attack(hit);
        }

        protected virtual GameObject DetectTarget()
        {
            RaycastHit2D hit = Physics2D.Raycast(DetectOrigin(), Vector2.right, Range, 1 << 7);
            if (hit)
            {
                targetPos = hit.collider.transform.position;
                dir = (hit.point - owner.Position()).normalized;

                return hit.collider.gameObject;
            }
            return null;
        }

        protected override State StartWeapon(ref float _time)
        {
            gunState = RangeWeaponState.DetectWait;
            target = null;
            _count = 0;
            return base.StartWeapon(ref _time);
        }
        protected override State Behaviour(ref float _time)
        {
            switch (gunState)
            {
                case RangeWeaponState.DetectWait:
                    gunState = DetectWait(ref _time);
                    break;
                case RangeWeaponState.PreDelay:
                    gunState = PreDelay(ref _time);
                    break;
                case RangeWeaponState.Shot:
                    gunState = Shot(ref _time);
                    break;
                case RangeWeaponState.ShotEnd:
                    return State.End;
            }
            return State.Behaviour;
        }

        RangeWeaponState DetectWait(ref float _time)
        {
            target = DetectTarget();

            if (target)
            {
                return RangeWeaponState.PreDelay;
            }
            return RangeWeaponState.DetectWait;
        }

        RangeWeaponState PreDelay(ref float _time)
        {

            if (_time >= PreDelay())
            {
                _time = 0;
                return RangeWeaponState.Shot;
            }
            return RangeWeaponState.PreDelay;
        }

        RangeWeaponState Shot(ref float _time)
        {
            if (UpdateDirection())
                DetectTarget();
            if (_count < ShotCount())
            {
                if (_count == 0 || _time >= ShotInterval())
                {
                    _count += 1;
                    for (int i = 0; i < ProjectileCount(); ++i)
                        GenerateObject(projectile);
                    _time = 0;
                }
                return RangeWeaponState.Shot;
            }
            else
            {
                return RangeWeaponState.ShotEnd;
            }
        }
    }
}