using System;
using UnityEngine;
using WUtility;

namespace WDefense
{

    public class Projectile : MonoBehaviour, IWeaponsObject
    {
        IWeaponsObject.State state = IWeaponsObject.State.Run;
        IRangeWeapon weapon;
        protected Vector2 velocity;

        public event Action EndEvent;
        public event Action<RaycastHit2D> HitEvent;


        BoxCollider2D col;
        float ctime = 0;
        LayerMask layerMask;
        private void Awake()
        {
            col = GetComponent<BoxCollider2D>();
        }
        public virtual void Init(IWeapon weapon)
        {
            state = IWeaponsObject.State.Run;
            this.weapon = (IRangeWeapon)weapon;
            velocity = this.weapon.GetVelocity();
            layerMask = this.weapon.TargetLayerMask() | WDefenseUtility.blockLayerMask;
            UpdateRotation();
        }
        void UpdateRotation()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, velocity.normalized);
        }
        public virtual IWeaponsObject.State ObjectUpdate()
        {
            return state;
        }

        public void Remove()
        {
            
        }
        protected void NextState()
        {
            if (state < IWeaponsObject.State.Remove) UpdateState(state+1);
        }
        protected virtual void UpdateState(IWeaponsObject.State _state)
        {
            if (_state != state) ctime = 0;
            state = _state;
        }

        public virtual void FixedUpdate()
        {
            switch (state)
            {
                case IWeaponsObject.State.Start:
                    break;
                case IWeaponsObject.State.Run:
                    Run(ref ctime);
                    break;
                case IWeaponsObject.State.End:
                    End(ref ctime);
                    break;
                case IWeaponsObject.State.Remove:
                    break;
            }
        }

        protected virtual void Run(ref float _time)
        {
            ProjectileBehaviour(velocity);
            if (weapon.VelocityUpdate(ref velocity))
                UpdateRotation();
        }

        void HitAction(RaycastHit2D hit)
        {
            transform.position = hit.point;
            HitEvent?.Invoke(hit);
            NextState();
        }
        void NoneHitAction()
        {
            Translate.Move(gameObject, velocity);
            if (gameObject.transform.position.x > 30 || gameObject.transform.position.y < -20)
            {
                NextState();
            }
        }

        protected virtual void ProjectileBehaviour(Vector2 velocity)
        {
            if (col) PhysicsW2D.BoxCast(col, velocity, layerMask, HitAction, NoneHitAction);
            else PhysicsW2D.Raycast(gameObject, velocity, layerMask,HitAction,NoneHitAction);
        }

        void End(ref float _time)
        {
            EndEvent?.Invoke();
            EndEvent = null;
            HitEvent = null;
        }
    }
}