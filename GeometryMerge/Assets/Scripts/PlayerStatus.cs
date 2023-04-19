using System;
using System.Numerics;
using UnityEngine;
namespace CartHero
{
    [System.Serializable]
    public class PlayerStatus 
    {
        [System.Serializable]
        public class UpdateData
        {
            [SerializeField]
            int data;
            public int Data { get { return data; } set { data = value; updateAction?.Invoke(data); }}
            public event System.Action<int> updateAction;
            
        }

        public int hpMax;
        public int HPMax { get => hpMax;set { hpMax = value; hpUpdate?.Invoke(hp,hpMax); } }
        int hp;
        public int HP { get => hp; set { hp = value; if (hp < 0) hp = 0; hpUpdate?.Invoke(hp,hpMax); } }
        public System.Action<int,int> hpUpdate;

        [SerializeField]
        float expRate = 2;
        [SerializeField]
        int expMax;
        int exp = 0;
        public int ExpMax { get => expMax; set { expMax = value; expUpdate?.Invoke(exp,expMax); } }
        public int Exp { get => exp; set { exp = value; LevelUp(); expUpdate?.Invoke(exp,expMax); } }
        public System.Action<int, int> expUpdate;

        [SerializeField]
        int levelMax;
        int level;
        public int Level { get => level; set { level = value; levelUpdate?.Invoke(level); } }
        public System.Action<int> levelUpdate;

        public UpdateData TestData;

        public void Initialize()
        {
            Level = 0;
            ExpMax = expMax;
            HPMax = hpMax;
            HP = HPMax;
            
            
        }



        public void ExpUp(int _value)
        {
            Exp += _value;
        }

        void LevelUp()
        {
            if(exp >= ExpMax)
            {
                level++;
                if (level <= levelMax)
                {
                    exp = 0;
                    ExpMax = (int)(expMax * expRate);
                }
                else
                    exp = ExpMax;
            }
        }
    }
}