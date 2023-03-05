using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SweetRoad
{

    public class ExplosionEffect : MonoBehaviour
    {

        Animator animator;
        internal bool effectEnd = false;
        // Start is called before the first frame update
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                effectEnd = true;
                animator.speed = 0;
            }
        }
    }

}