using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WDefense
{

    public interface ICharacter
    {
        public event System.Action DeathEvent;
        public int HP();
        public Vector2 Direction();
        public Vector2 Position();
        public GameObject GetGameObject();
    }

    public interface IWeaponsObject
    {
        /// <summary>
        /// target, hit
        /// </summary>
        public event System.Action<RaycastHit2D> HitEvent;
        public event System.Action EndEvent;
        public enum State { Start, Run, End, Remove }
        public void Init(IWeapon weapon);
        public State ObjectUpdate();
        public void Remove();
    }

    public interface IBaseStatus
    {
        public int HP();
    }


    public interface IItem
    {
        public void EffectOnHit(IAttackedObject target, int damage, Vector2 hitPoint);
    }

    public interface IWeapon : IItem
    {
        public Vector2 DetectOrigin();
        public Vector2 Direction();
        public GameObject Target();
        public Vector2 TargetPosition();
        public IWeaponUser Owner();
        public LayerMask TargetLayerMask();

        
        public void Attack(RaycastHit2D hit);
    }

    public interface IRangeWeapon : IWeapon
    {
        public bool VelocityUpdate(ref Vector2 velocity);
        public Vector2 GetVelocity();
    }

    public interface IWeaponUser :ICharacter
    {
        public event System.Action<IAttackedObject, int, Vector2> HitEvent;
        public void Hit(IAttackedObject target, int damage, Vector2 hitPoint);

        public Bounds Bounds();
        public LayerMask EnemyLayerMask();
    }



    public interface IAttackedObject : ICharacter
    {
        public int BeAttecked(int damage);
    }

    public interface IPlayerCharacter : IWeaponUser
    {
        
    }


}