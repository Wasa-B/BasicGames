using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wasabi.TreeGraph
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public BehaviourTree tree;
        // Start is called before the first frame update
        void Start()
        {
            tree = tree.Clone(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            tree.Update();
        }
    }
}