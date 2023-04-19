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
        public float speed = 1;
        public int maxHp = 50;
        public TriggerVarialbe<int> hp;
        public int exp = 10;
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

        private void Awake()
        {
        }
        // Start is called before the first frame update
        void Start()
        {
            enemyStatus.hp.Value = enemyStatus.maxHp;
        }
        private void FixedUpdate()
        {
            transform.Translate(Vector2.down * enemyStatus.speed * Time.fixedDeltaTime);
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

        public void Damage(WeaponStatus weaponStatuses)
        {
            enemyStatus.hp.Value -= weaponStatuses.damage;
            hitActionHdr?.Invoke();


            if (enemyStatus.hp <= 0)
            {
                expUp?.Invoke(enemyStatus.exp);
                EndObject();
            }

        }
    }
}