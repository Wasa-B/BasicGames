using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace WasabiGame
{

    [System.Serializable]
    public class AttackStatus
    {
        [SerializeField]
        protected int damage;
        [SerializeField]
        protected float attackSpeed ;
        [SerializeField]
        protected float size ;
        [SerializeField]
        protected float range ;

        [SerializeField]
        private LayerMask layerMask;

        public int Damage { get => damage;}
        public float AttackSpeed { get => attackSpeed;}
        public float Size { get => size;  }
        public float Range { get => range;}
        public LayerMask LayerMask { get => layerMask; set => layerMask = value; }
    }

    public abstract class Weapon : BehaviourCommand
    {

        public virtual AttackStatus Status => new AttackStatus();
        public ClipStatus weaponSound;

        internal event System.Action attackAction;
        float currentTime;
        bool weaponUse;
        internal Vector2 dir = Vector2.up;
        void ResetTime() => currentTime = Status.AttackSpeed > 0 ? (100 / Status.AttackSpeed) : 2;
        internal void WeaponUse(bool value = true) => weaponUse = value;
        protected override void OnStart()
        {
            ResetTime();
        }
        protected override State OnUpdate()
        {

            currentTime -= Time.fixedDeltaTime;
            if (currentTime <= 0)
            {
                PreAttack();
                if (weaponUse && CheckWeapon())
                {
                    weaponUse = false;
                    ResetTime();
                    
                    Attack(dir);
                    attackAction?.Invoke();
                }
                AfterAttack();
            }
            weaponUse = false;
            return state;
        }
        protected virtual bool CheckWeapon() => true;
        protected override void OnEnd() { }

        protected virtual void PreAttack() { }
        protected abstract void Attack(Vector2 dir);
        protected virtual void AfterAttack() { }

        protected void PlayWeaponSound()
        {
            weaponSound.Play();
        }

        public abstract void SetStatus(AttackStatus attackStatus);
    }


    [CreateAssetMenu(fileName = "Base", menuName = "Command/Weapon/Base")]
    public class WeaponCommand : Weapon
    {
        [SerializeField]
        AttackStatus weaponSatus;
        public override AttackStatus Status => weaponSatus;
        internal AttackStatus status;
        public ProjectileControl projectilePrefab;


        float AttackSpeed => status.AttackSpeed;


        public override BehaviourCommand Clone(GameObject gameObject)
        {
            var clone = base.Clone(gameObject) as WeaponCommand;

            return clone;
        }



        protected override void Attack(Vector2 dir)
        {

        }

        public override void SetStatus(AttackStatus attackStatus)
        {

        }
    }
}