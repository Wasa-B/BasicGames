using System.Collections.Generic;
using WasabiGame;
using UnityEngine;

namespace CartHero
{
    [CreateAssetMenu(fileName = "TimeLine", menuName = "GeneratorTimeline/TimeLine")]
    public class EnemyGenerateTimeline : Node
    {
        [System.Serializable]
        public class GenList
        {
            public float genTime;
            public EnemyGenerateBlock enemyGenerateBlock;
        }
        public List<GenList> genList;

        float time = 0;
        int genIndex;


        


        public override Node Clone()
        {
            var clone = base.Clone() as EnemyGenerateTimeline;
            for (int i = 0; i < genList.Count; i++)
                clone.genList[i].enemyGenerateBlock = genList[i].enemyGenerateBlock.Clone() as EnemyGenerateBlock;

            return clone;
        }

        protected override void OnStart()
        {
            genIndex = 0;
            time = 0;
        }

        protected override State OnUpdate()
        {
            time += Time.deltaTime;
            if(time >= genList[genIndex].genTime)
            {
                if(genList[genIndex].enemyGenerateBlock.Update() == State.Success)
                    genIndex++;
            }
            if (genIndex >= genList.Count)
                return State.Success;
            return state;
        }

        protected override void OnEnd()
        {
            
        }
    }
}