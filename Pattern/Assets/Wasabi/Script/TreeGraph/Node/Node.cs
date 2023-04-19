using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wasabi.TreeGraph
{

    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Running,
            Failure,
            Success
        }
        [HideInInspector] public State state = State.Running;
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;

        [TextArea] public string description;

        GameObject gameObject;
        public State Update()
        {   
            if(started == false)
            {
                OnStart();
                started = true;
            }
            state = OnUpdate();

            if(state == State.Failure || state == State.Success)
            {
                OnStop();
                started = false;

            }
            return state;
        }

        public virtual Node Clone(GameObject gameObject)
        {
            var node = Instantiate(this);
            node.gameObject = gameObject;
            return node;
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }

    
}
