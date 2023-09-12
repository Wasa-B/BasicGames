using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TYCOON
{
    public class GameManager : MonoBehaviour
    {
        internal GameManager instance;
        PlayData data;



        private void Awake()
        {
            if(instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;
            
            DataLoad();



            DontDestroyOnLoad(gameObject);
        }

        private void DataLoad()
        {
            
        }

    }

}