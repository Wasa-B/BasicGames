using System.Collections.Generic;
using UnityEngine;
using WasabiGame;

namespace CartHero
{
    [CreateAssetMenu(fileName = "Block", menuName = "GeneratorTimeline/Block")]

    public class EnemyGenerateBlock : Node
    {
        [System.Serializable]
        public class EnemyGenerateInfo
        {
            public float subGenTime;
            public Vector2 subPosition;
        }
        public EnemyControl enemyPrefabs;
        public Vector2 position;
        public List<EnemyGenerateInfo> enemyGenerateInfos;

        float time = 0;
        int genIndex = 0;

        public EnemyControl Generate()
        {
            var enemy = PoolManager.GenerateObject<EnemyControl>(enemyPrefabs);

            enemy.transform.position = position + enemyGenerateInfos[genIndex].subPosition;



            return enemy;
        }

        protected override void OnStart()
        {
            genIndex = 0;
            time = enemyGenerateInfos[genIndex].subGenTime;
        }

        protected override State OnUpdate()
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                Generate();
                genIndex++;
                if (genIndex >= enemyGenerateInfos.Count)
                    return State.Success;
                else
                    time = enemyGenerateInfos[genIndex].subGenTime;
            }

            return state;

        }

        protected override void OnEnd()
        {

        }


    }
}