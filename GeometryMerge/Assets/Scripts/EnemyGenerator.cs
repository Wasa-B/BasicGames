using System.Collections;
using UnityEngine;

namespace CartHero
{

    public class EnemyGenerator : MonoBehaviour
    {
        public EnemyGenerateTimeline enemyGenerateTimeline;

        void Generate()
        {
            
        }
        // Start is called before the first frame update
        void Start()
        {
            enemyGenerateTimeline = enemyGenerateTimeline.Clone() as EnemyGenerateTimeline;
        }

        // Update is called once per frame
        void Update()
        {
            //if (enemyGenerateTimeline.state == WasabiGame.Node.State.Running)
            //    enemyGenerateTimeline.Update();
            enemyGenerateTimeline.Update();
        }
    }
}