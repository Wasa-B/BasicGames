using UnityEngine;
namespace WUtility
{
    public static class PhysicsW2D
    {
        public static RaycastHit2D Raycast(GameObject gameObject, Vector2 velocity, LayerMask layerMask)
        {
            return Raycast(gameObject.transform.position, velocity, Time.fixedDeltaTime, layerMask);
        }
        public static RaycastHit2D Raycast(Vector2 origin, Vector2 velocity, LayerMask layerMask)
        {
            return Raycast(origin, velocity, Time.fixedDeltaTime, layerMask);
        }
        public static RaycastHit2D Raycast(Vector2 origin, Vector2 velocity, float timeScale, LayerMask layerMask)
        {
            return Physics2D.Raycast(origin, velocity, velocity.magnitude * timeScale, layerMask);
        }


        public static void Raycast(GameObject gameObject, Vector2 velocity, LayerMask layerMask, System.Action<RaycastHit2D> hitAction, System.Action othersAction = null)
        {
            Raycast(gameObject.transform.position, velocity, Time.fixedDeltaTime, layerMask, hitAction, othersAction);
        }
        public static void Raycast(Vector2 origin, Vector2 velocity, LayerMask layerMask, System.Action<RaycastHit2D> hitAction, System.Action othersAction = null)
        {
            Raycast(origin, velocity, Time.fixedDeltaTime, layerMask, hitAction, othersAction);
        }
        public static void Raycast(Vector2 origin, Vector2 velocity, float timeScale, LayerMask layerMask, System.Action<RaycastHit2D> hitAction, System.Action othersAction = null)
        {
            HitAction(Raycast(origin, velocity, timeScale, layerMask), hitAction, othersAction);
        }

        public static RaycastHit2D BoxCast(BoxCollider2D col, Vector2 velocity, LayerMask layerMask) => BoxCast(col, velocity, Time.fixedDeltaTime, layerMask);
        public static RaycastHit2D BoxCast(BoxCollider2D col, Vector2 velocity, float timeScale, LayerMask layerMask)
        {
            return Physics2D.BoxCast(col.bounds.center, col.bounds.size, col.gameObject.transform.rotation.z, velocity, velocity.magnitude * timeScale, layerMask);
        }
        public static void BoxCast(BoxCollider2D col, Vector2 velocity, LayerMask layerMask, System.Action<RaycastHit2D> hitAction, System.Action othersAction = null)
        {
            BoxCast(col, velocity, Time.fixedDeltaTime, layerMask, hitAction, othersAction);
        }
        public static void BoxCast(BoxCollider2D col, Vector2 velocity, float timeScale, LayerMask layerMask, System.Action<RaycastHit2D> hitAction, System.Action othersAction = null)
        {
            HitAction(BoxCast(col, velocity, timeScale, layerMask), hitAction, othersAction);
        }

        public static void HitAction(RaycastHit2D hit, System.Action<RaycastHit2D> hitAction, System.Action othersAction = null)
        {
            if (hit) hitAction?.Invoke(hit);
            else othersAction?.Invoke();
        }

        public static void Movement(GameObject gameObject, Vector2 velocity, LayerMask layerMask, System.Action<RaycastHit2D> hitAction, System.Action othersAction = null)
        {
            var hit = Raycast(gameObject, velocity, layerMask);
            HitAction(hit, hitAction, othersAction);
            if (hit) gameObject.transform.Translate(velocity.normalized * hit.distance);
            else Translate.Move(gameObject, velocity);
        }
    }

    public static class Translate
    {
        public static void Move(GameObject gameObject, Vector2 velocity) => Move(gameObject, velocity, Time.fixedDeltaTime);
        public static void Move(GameObject gameObject, Vector2 velocity, float timeScale) => gameObject.transform.Translate(velocity * timeScale, Space.World);
    }
}