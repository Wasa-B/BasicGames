using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WasabiGame
{

    public interface IDamagedObject
    {
        public event System.Action hitActionHdr;
        public virtual void AddEvent(System.Action action)
        {
            hitActionHdr += action;
        }
        public void Damage(AttackStatus weaponStatuses);
    }

    public class ProjectileControl : PoolObject
    {
        public PoolObject hitEffectPrefab;
        public PoolObject endEffectPrefab;

        GunStatus weaponStatus;
        protected float Range { get => weaponStatus.Range; }
        Vector2 dir;
        GameObject owner;
        internal bool fire = false;
        bool hit = false;
        public MoveCommand moveCommand;

        int pierceCount;
        HashSet<int> damagedObject = new HashSet<int>();

        private void Awake()
        {
            moveCommand = moveCommand.Clone(this.gameObject) as MoveCommand;
            moveCommand.hitAction += Hit;
            moveCommand.end += End;
        }
        private void Start()
        {
            moveCommand.movementInfo.blockLayer = weaponStatus.LayerMask;
        }
        public override void PoolOut()
        {
            fire = true;
            base.PoolOut();
        }
        internal void UpdateStatus(GunStatus weaponStatus, GameObject owner, Vector2 dir)
        {
            this.weaponStatus = weaponStatus;
            this.owner = owner;
            //transform.position = owner.transform.position;
            moveCommand.movementInfo.speed = weaponStatus.projectileMoveStatus.speed;
            moveCommand.movementInfo.maxSpeed = weaponStatus.projectileMoveStatus.maxSpeed;
            this.dir = dir.normalized;
            this.hit = false;

            this.pierceCount = weaponStatus.PierceCount;
            moveCommand.movementInfo.blockMove = false;
            damagedObject.Clear();
        }
        internal void Fire()
        {
            moveCommand.MoveTo((Vector2)transform.position + dir * Range);
        }
        void Hit(RaycastHit2D hit)
        {
            var hObj = hit.collider.GetInstanceID();
            if (damagedObject.Contains(hObj))
                return;

            damagedObject.Add(hObj);
            hit.collider.gameObject.GetComponent<IDamagedObject>()?.Damage(weaponStatus);
            pierceCount--;
            var hiteft = PoolManager.GenerateObject(hitEffectPrefab);
            hiteft.transform.position = hit.point;
            

            if(pierceCount <= 0)
            {
                this.hit = true;
                moveCommand.movementInfo.blockMove = true;
            }
        }
        void End(Vector2 vector)
        {
            if(hit == false)
            {
                var endEffect = PoolManager.GenerateObject(endEffectPrefab);
                endEffect.transform.position = vector + dir.normalized * Time.fixedDeltaTime;
                endEffect.transform.rotation = transform.rotation;
            }
        }

        private void FixedUpdate()
        {
            if(moveCommand.Update() == BehaviourCommand.State.Success)
            {
                EndObject();
            }
        }

    }

}