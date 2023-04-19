using UnityEngine;


namespace WasabiGame
{
    public abstract class BehaviourCommand : Node
    {
        
        protected GameObject gameObject;



        public virtual BehaviourCommand Clone(GameObject gameObject)
        {
            var clone = base.Clone() as BehaviourCommand;
            clone.gameObject = gameObject;
            return clone;
        }


    }
}