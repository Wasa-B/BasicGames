using UnityEngine;


namespace WasabiGame
{
    [System.Serializable]
    public class MovementStatus
    {
        public float speed ;
        public float acc ;
        public float maxSpeed ;
    }

    [System.Serializable]
    public class MovementInfo
    {
        public MovementStatus status;

        public float speed { get => status.speed; set => status.speed = value; }
        public float acc { get => status.acc; set => status.acc = value; }
        public float maxSpeed { get => status.maxSpeed; set => status.maxSpeed = value;}

        public LayerMask blockLayer;
        public bool blockMove = false;
        public bool hitCheck = false;
        public enum BlockType {RayCast, BoxCast}
        public BlockType blockType = BlockType.RayCast;
        
    }



    [CreateAssetMenu(fileName = "Move", menuName = "Command/Move/Move")]
    public class MoveCommand : BehaviourCommand
    {
        Vector2 moveToPos;
        public MovementInfo movementInfo;
        
        Vector2 dir;
        float currentAcc = 0;
        float length = 0;

        BoxCollider2D collider;

        internal event System.Action<Vector2> update;
        internal event System.Action<Vector2> end;
        internal event System.Action<RaycastHit2D> hitAction;
        
        public virtual void MoveTo(Vector2 pos, bool accReset = true)
        {
            moveToPos = pos;
            dir = moveToPos - (Vector2)gameObject.transform.position;
            length = dir.magnitude;
            dir.Normalize();
            if(accReset)
                currentAcc = 0;
            state = State.Running;
        }

        protected override void OnEnd()
        {
            
        }

        protected override void OnStart()
        {
            dir = moveToPos - (Vector2)gameObject.transform.position;
            length = dir.magnitude;
            dir.Normalize();
            currentAcc = 0;
        }

        protected override State OnUpdate()
        {
            var moveAmount = Time.fixedDeltaTime * ((movementInfo.speed + currentAcc)>movementInfo.maxSpeed?movementInfo.maxSpeed: (movementInfo.speed + currentAcc));
            if(collider)
            {
                var hit = GetRaycast(moveAmount);

                if (hit && movementInfo.blockMove)
                {
                    length = 0;
                    moveToPos = hit.point;
                }
            }

            if(length - moveAmount > 0)
            {
                length -= moveAmount;
                gameObject.transform.Translate(dir * moveAmount,Space.World);
            }
            else
            {
                gameObject.transform.position = moveToPos;
                end?.Invoke(gameObject.transform.position);
                return State.Success;
            }
            currentAcc += movementInfo.acc * Time.fixedDeltaTime;
            update?.Invoke(gameObject.transform.position);

            return State.Running;
        }

        public override BehaviourCommand Clone(GameObject gameObject)
        {

            var clone = base.Clone(gameObject) as MoveCommand;
            clone.collider = gameObject.GetComponent<BoxCollider2D>();
            return clone;
        }

        protected RaycastHit2D GetRaycast(float moveAmount)
        {

            var hit = movementInfo.blockType == MovementInfo.BlockType.BoxCast ?
                Physics2D.BoxCast(gameObject.transform.position, collider.size, 0, dir, moveAmount, movementInfo.blockLayer)
                : Physics2D.Raycast(gameObject.transform.position, dir, moveAmount,movementInfo.blockLayer);
                ;
            
            if (hit)
            {
                hitAction?.Invoke(hit);
            }
            
            return hit;
        }
    }
}