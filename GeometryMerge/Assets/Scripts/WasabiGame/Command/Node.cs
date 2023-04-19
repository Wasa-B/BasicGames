using UnityEngine;


namespace WasabiGame
{
    public abstract class Node : ScriptableObject
    {
        public enum State { Running, Success, Failure }
        bool start = false;
        public State state = State.Running;

        public virtual State Update()
        {
            if (start == false)
            {
                state = State.Running;
                start = true;
                OnStart();
            }
            state = OnUpdate();
            if (state == State.Success || state == State.Failure)
            {
                start = false;
                OnEnd();
            }
            return state;
        }
        protected abstract void OnStart();
        protected abstract State OnUpdate();
        protected abstract void OnEnd();
        public virtual Node Clone()
        {
            var clone = Instantiate(this);
            
            return clone;
        }
    }
}