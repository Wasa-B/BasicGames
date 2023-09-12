using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WDefense
{
    public class CharacterControl : MonoBehaviour, IPlayerCharacter
    {
        public static CharacterControl Instance;
        public List<Weapon> weapons;

        public event Action DeathEvent;
        public event Action<IAttackedObject, int, Vector2> HitEvent;

        public LayerMask enemyLayer;
        public float cooldownSpeed = 1;

        float cooldownSpdRate = 1;

        public LayerMask EnemyLayerMask() => enemyLayer;

        private void Awake()
        {
            Instance = this;
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i] = Instantiate(weapons[i]);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            weapons.ForEach(weapon => weapon.Init(this));
        }

        private void FixedUpdate()
        {
            weapons.ForEach(weapon => weapon.FixedUpdate());
        }

        public int HP() => -1;
        public float CooldownSpeedRate => cooldownSpdRate;
        public float RangeRate => 1f;
        public int ATK => 0;
        public float ATKRate => 1f;
        public float SPD => 1;
        public float Range => 1f;


        public Vector2 Position() => transform.position;
        public Bounds Bounds() => new Bounds();
        public GameObject GetGameObject() => this.gameObject;
        public Vector2 Direction() => Vector2.right;
        public void Hit(IAttackedObject target, int damage, Vector2 hitPoint)
        {
            HitEvent?.Invoke(target, damage,hitPoint);
        }

        public void UpdateStatus()
        {
            var cdSpdR = cooldownSpeed;
            for(var i = 0; i < weapons.Count; i++)
            {
                cdSpdR += 1-weapons[i].CooldownSpeedRate;
            }
            
            cooldownSpdRate = cdSpdR<.2f?.2f:cdSpdR;
        }


        internal void AddWeapon(Weapon nWeapon)
        {
            var weapon = Instantiate(nWeapon);
            weapon.Init(this);
            weapons.Add(weapon);
            UpdateStatus();
        }
    }
}