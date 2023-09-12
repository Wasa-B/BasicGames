using System;
using System.Numerics;
using UnityEngine;
namespace CartHero
{
    [System.Serializable]
    public class PlayerStatus 
    {
        public int hpMax;
        public int HPMax { get => hpMax;set { hpMax = value; hpUpdate?.Invoke(hp,hpMax); } }
        int hp;
        public int HP { get => hp; set { hp = value; if (hp < 0) hp = 0; hpUpdate?.Invoke(hp,hpMax); } }
        public event System.Action<int,int> hpUpdate;

        [SerializeField]
        float expRate = 2;
        [SerializeField]
        int expMax;
        int exp = 0;
        public int ExpMax { get => expMax; set { expMax = value; expUpdate?.Invoke(exp,expMax); } }
        public int Exp { get => exp; set { exp = value; LevelUp(); expUpdate?.Invoke(exp,expMax); } }
        public event System.Action<int, int> expUpdate;

        [SerializeField]
        int levelMax;
        int level;
        public int Level { get => level; set { level = value; levelUpdate?.Invoke(level); } }
        public event System.Action<int> levelUpdate;

        [SerializeField]

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
                
                if (level <= levelMax)
                {
                    Level++;
                    exp = 0;
                    ExpMax = (int)(expMax * expRate);
                }
                else
                    exp = ExpMax;
            }
        }
    }
}