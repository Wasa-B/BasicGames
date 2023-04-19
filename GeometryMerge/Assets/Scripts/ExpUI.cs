using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CartHero
{

    public class ExpUI : MonoBehaviour
    {

        public Image expBar;
        Vector2 originSize;
        // Start is called before the first frame update

        private void Awake()
        {
            originSize = expBar.rectTransform.sizeDelta;
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ExpUpdate(int exp, int max)
        {
            expBar.rectTransform.sizeDelta = new Vector2(originSize.x * exp / max, originSize.y);
            
        }
    }

}