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
        public void Damage(WeaponStatus weaponStatuses);
    }

    public class ProjectileControl : PoolObject
    {
        public PoolObject hitEffectPrefab;
        public PoolObject endEffectPrefab;

        WeaponStatus weaponStatus;
        protected float Range { get => weaponStatus.range; }
        Vector2 dir;
        GameObject owner;
        internal bool fire = false;
        bool hit = false;
        public MoveCommand moveCommand;


        private void Awake()
        {
            moveCommand = moveCommand.Clone(this.gameObject) as MoveCommand;
            moveCommand.hitAction += Hit;
            moveCommand.end += End;
        }
        private void Start()
        {
            moveCommand.movementInfo.blockLayer = weaponStatus.layerMask;
        }
        public override void PoolOut()
        {
            fire = true;
            base.PoolOut();
        }
        internal void UpdateStatus(WeaponStatus weaponStatus, GameObject owner, Vector2 dir)
        {
            this.weaponStatus = weaponStatus;
            this.owner = owner;
            //transform.position = owner.transform.position;
            moveCommand.movementInfo.speed = weaponStatus.movementInfo.speed;
            moveCommand.movementInfo.maxSpeed = weaponStatus.movementInfo.maxSpeed;
            this.dir = dir.normalized;
            this.hit = false;
        }
        internal void Fire()
        {
            moveCommand.MoveTo((Vector2)transform.position + dir * Range);
        }
        void Hit(RaycastHit2D hit)
        {
            var hiteft = PoolManager.GenerateObject(hitEffectPrefab);
            hiteft.transform.position = hit.point;
            hit.collider.gameObject.GetComponent<IDamagedObject>()?.Damage(weaponStatus);

            this.hit = true;
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