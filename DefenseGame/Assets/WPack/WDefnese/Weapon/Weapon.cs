using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WDefense
{

    public class Weapon : WItem.Item, IWeapon
    {
        static protected Vector2 standard = new Vector2(-7f, -2.5f);
        [SerializeField]
        static float minCooltime = .05f;

        public enum State { Start, Wait, Behaviour, End, Remove }
        protected State state = State.Start;

        public ItemStatus passiveStatus;
        public float coolDown = 1;

        protected IWeaponUser owner;
        protected List<IWeaponsObject> gen_objects = new List<IWeaponsObject>();

        Queue<IWeaponsObject> deleteWait = new Queue<IWeaponsObject>();

        private float _ctime = 0;



        public virtual void Init(IWeaponUser owner)
        {
            this.owner = owner;
            owner.HitEvent += EffectOnHit;
        }

        public void FixedUpdate()
        {
            
            switch (state)
            {
                case State.Start:
                    SetState(StartWeapon(ref _ctime));
                    break;
                case State.Wait:
                    SetState(WaitWeapon(ref _ctime));
                    break;
                case State.Behaviour:
                    SetState(Behaviour(ref _ctime));
                    break;
                case State.End:
                    SetState(End(ref _ctime));
                    break;
                case State.Remove:
                    break;
            }
            _ctime += Time.fixedDeltaTime;
//            _ctime += Time.fixedDeltaTime * owner.CooldownSpeedRate;

            //Gen_Update();
        }

        void SetState(State _state)
        {
            if (this.state != _state)
            {
                this.state = _state;
                _ctime = 0;
            }
        }

        protected virtual State StartWeapon(ref float _time) => State.Wait;
        protected virtual State WaitWeapon(ref float _time)
        {
            if (_time >= coolDown)
            {
                return State.Behaviour;
            }
            return State.Wait;
        }
        protected virtual State Behaviour(ref float _time) => State.Wait;
        protected virtual State End(ref float _time) => State.Start;

        public void RemoveWeapon()
        {
            gen_objects.ForEach(obj => obj.Remove());
            gen_objects.Clear();
        }

        protected virtual void BasicEndAction(IWeaponsObject wo)
        {
            gen_objects.Remove(wo);
        }

        protected GameObject GenerateObject(GameObject prefab, System.Action endAction = null)
        {
            
            var go = WDefenseUtility.Generate(prefab);
            var wo = go.GetComponent<IWeaponsObject>();
            if (wo != null)
            {
                gen_objects.Add(wo);
                wo.Init(this);

                wo.HitEvent += Attack;
                wo.EndEvent += () =>
                {
                    gen_objects.Remove(wo);
                    WDefenseUtility.Delete(go);
                    //GameControl.RemveObject(go);
                };

                if(endAction != null) wo.EndEvent += endAction;
            }
            go.transform.position = owner.Position();
            return go;
        }

        void Gen_Update()
        {
            for (int i = 0; i < gen_objects.Count; i++)
            {
                if (gen_objects[i].ObjectUpdate() == IWeaponsObject.State.End)
                    deleteWait.Enqueue(gen_objects[i]);
            }

            while (deleteWait.Count > 0)
                gen_objects.Remove(deleteWait.Dequeue());
        }
        
        public virtual void EffectOnHit(IAttackedObject target, int damage, Vector2 hitPoint) { }

        public virtual int WeaponATK => 0;
        public virtual float WeaponSPD => 1;
        public virtual float WeaponRange => 0;

        public virtual Vector2 Direction() => Vector2.right;
        public virtual GameObject Target() => null;
        public virtual IWeaponUser Owner() => owner;
        public virtual Vector2 DetectOrigin() => owner.Position();
        public virtual LayerMask TargetLayerMask() => owner.EnemyLayerMask();

        public virtual float CooldownSpeedRate => 1f;
        public virtual float RangeRate => 1f;
        public virtual float ATKRate => 1f;

        public virtual int ATK => 0;
        public virtual float SPD => 0;
        public virtual float Range => 0;



        public virtual void Attack(RaycastHit2D hit)
        {
            var attackedObject = hit.collider.GetComponent<IAttackedObject>();
            if (attackedObject != null)
            {
                var damaged = attackedObject.BeAttecked(WeaponATK);
                owner.Hit(attackedObject, damaged, hit.point);
            }
        }

        public virtual Vector2 TargetPosition() => Vector2.zero;
    }

}