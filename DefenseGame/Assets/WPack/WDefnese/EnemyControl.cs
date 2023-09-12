using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WDefense
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EnemyControl : MonoBehaviour, IAttackedObject
    {
        static float stageLevel = 1;
        static internal void SetLevel(float level) => stageLevel = level;

        [System.Serializable]
        public class EnemyStatus
        {
            [SerializeField]
            int hp = 10;
            public float speed = 5;
            public int damage = 5;
            public float range = 2;
            bool death = false;
            public event System.Action deathEvent;
            public int HP
            {
                get => hp;
                set
                {
                    hp = value;
                    if (!death && hp <= 0)
                    {
                        death = true;
                        deathEvent?.Invoke();
                    }
                }
            }
            public EnemyStatus Clone(float level = 1)
            {
                EnemyStatus result = new EnemyStatus();
                result.hp = (int)(hp * level);
                result.speed = speed;
                result.damage = (int)(damage * level);
                result.range = range;
                return result;
            }
        }

        [SerializeField]
        EnemyStatus enemyStatus;


        EnemyStatus status;

        public event Action DeathEvent;
        public LayerMask targetLayer;

        // Start is called before the first frame update

        BoxCollider2D col;
        private void Awake()
        {
            col = GetComponent<BoxCollider2D>();
        }

        void Start()
        {
            status = enemyStatus.Clone(stageLevel);
            status.deathEvent += DeathEnemy;
        }
        private void OnEnable()
        {
            status = enemyStatus.Clone(stageLevel);
            status.deathEvent += DeathEnemy;

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (Range_Calculate(targetLayer) == false)
            {
                Move();
            }
        }

        public bool Range_Calculate(LayerMask layer)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, status.range, layer);
            if (hit)
            {
                return true;
            }
            return false;
        }
        public void Move()
        {
            var hit = Physics2D.Raycast(new Vector2(col.bounds.min.x -.05f,transform.position.y), Vector2.left, status.speed * Time.fixedDeltaTime, 1<<gameObject.layer);
            if (hit) transform.Translate(hit.distance * Vector2.left,Space.World);
            else transform.Translate(status.speed * Time.fixedDeltaTime * Vector2.left, Space.World);
        }


        public int BeAttecked(int damage)
        {
            status.HP -= damage;
            //Debug.Log($"Damage : {damage} , HP : {status.HP}");
            return damage;
        }



        void DeathEnemy()
        {
            DeathEvent?.Invoke();
            DeathEvent = null;
        }

        public virtual Vector2 Position() => transform.position;
        public Vector2 Direction() => Vector2.right;
        public GameObject GetGameObject() => this.gameObject;
        public int HP() => this.status.HP;
    }
}