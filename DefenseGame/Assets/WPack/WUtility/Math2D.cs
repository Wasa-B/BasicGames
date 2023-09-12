using UnityEngine;
namespace WUtility
{
    public static class Math2D
    {
        public static Vector2 VectorRotate(Vector2 orign, float angle) => Quaternion.AngleAxis(angle, Vector3.forward) * orign;
        public static Vector2 AngleRange(Vector2 orign, float angle) => Quaternion.AngleAxis(Random.Range(-angle / 2, angle / 2), Vector3.forward) * orign;

        /// <summary>
        /// Fixed velocity scale.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="velocity"></param>
        /// <param name="gravity"></param>
        /// <param name="indirect"></param>
        /// <returns></returns>
        public static Vector2 ProjectileVelocity(Vector2 origin, Vector2 target, float velocity, float gravity, bool indirect = true)
        {
            var x = target.x - origin.x;
            var y = target.y - origin.y;
            var angle = Mathf.Atan((velocity * velocity + (indirect ? 1 : -1) * Mathf.Sqrt(Mathf.Pow(velocity, 4) - gravity * (gravity * x * x + 2 * y * velocity * velocity))) / (gravity * x));
            return new Vector2(Mathf.Cos(angle) * Mathf.Sign(x), Mathf.Sin(angle) * Mathf.Sign(x)) * velocity;
        }

        public static Vector2 ProjectileVelocity(Vector2 origin, Vector2 target, Vector2 dir, float flightTime)
        {
            var x = target.x - origin.x;

            return dir *(x/(flightTime * dir.x));
        }

        public static Vector2 Deg2Vector(float degree) => new Vector2((float)Mathf.Cos(degree), (float)Mathf.Sin(degree));
        public static float ProjectileDistance(Vector2 origin, float targetY, float velocity, float degree, float gravity)
        {
            var dir = Deg2Vector(degree);
            return (velocity * dir.x * (velocity * dir.y + Mathf.Sqrt(velocity * velocity * dir.y * dir.y + 2 * gravity * (origin.y - targetY)))) / gravity;
        }
        public static float ProjectileDistance(Vector2 origin, float targetY, Vector2 velocity, float gravity)
        {
            var dir = velocity.normalized;
            var v = velocity.magnitude;
            return (v * dir.x * (v * dir.y + Mathf.Sqrt(v * v * dir.y * dir.y + 2 * gravity * (origin.y - targetY)))) / gravity;
        }

        public static float ProjectileMaxDistanceAngle(Vector2 origin, float targetY, float velocity, float gravity)
        {
            var y = origin.y - targetY;
            var v2 = velocity * velocity;
            return Mathf.Acos(Mathf.Sqrt((2*gravity*y + v2) /(2*gravity*y +2*v2)));
        }
        public static float ProjectileMaxDistance(Vector2 origin, float targetY, float velocity, float gravity)
        {
            var degree = ProjectileMaxDistanceAngle(origin, targetY, velocity, gravity);
            return ProjectileDistance(origin,targetY,velocity,degree,gravity);
        }
    }
}