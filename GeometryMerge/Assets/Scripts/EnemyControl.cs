using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WasabiGame;
using System.Linq;


namespace CartHero
{
    [System.Serializable]
    public class EnemyStatus
    {
        public float speed ;
        public int maxHp ;
        public TriggerVarialbe<int> hp;
        public int exp ;
        /// <summary>
        /// hpUpdate(hp, Maxhp)
        /// </summary>

    }



    public class EnemyControl : PoolObject, IDamagedObject
    {


        public EnemyStatus enemyStatus;
        public SpriteMaskControl hitBlinkObject;

        public event System.Action hitActionHdr;

        internal static event System.Action<int> expUp;

        internal static GameObject target;

        Rigidbody2D rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        // Start is called before the first frame update
        void Start()
        {
            enemyStatus.hp.Value = enemyStatus.maxHp;
        }
        private void FixedUpdate()
        {
            rigid.velocity = (target.transform.position - transform.position).normalized*enemyStatus.speed;

            if (transform.position.y < -5)
                EndObject();
        }

        public override void PoolOut()
        {
            base.PoolOut();
            enemyStatus.hp.Value = enemyStatus.maxHp;
        }
        // Update is called once per frame
        void Update()
        {

        }

        public void Damage(AttackStatus weaponStatuses)
        {
            enemyStatus.hp.Value -= weaponStatuses.Damage;
            hitActionHdr?.Invoke();


            if (enemyStatus.hp <= 0)
            {
                expUp?.Invoke(enemyStatus.exp);
                EndObject();
            }

        }
    }
}