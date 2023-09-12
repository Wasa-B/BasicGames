using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WDefense
{


    public class EnemySpawn : MonoBehaviour
    {
        public event System.Action<int> expUp;
        public GameObject enemy;
        public float interval = 1f;

        float _ctime = 0;

        private void FixedUpdate()
        {
            if (_ctime >= interval)
            {
                var hit = Physics2D.OverlapPoint(transform.position, 1 << enemy.layer);
                if (hit)
                {
                   
                }
                else
                {
                    _ctime -= interval;
                    var eo = WDefenseUtility.Generate(enemy);
                    eo.transform.position = transform.position;
                    eo.GetComponent<IAttackedObject>().DeathEvent += () => {
                        expUp?.Invoke(1);
                        WDefenseUtility.Delete(eo);
                    };
                }
            }
            _ctime += Time.fixedDeltaTime;
        }
    }
}