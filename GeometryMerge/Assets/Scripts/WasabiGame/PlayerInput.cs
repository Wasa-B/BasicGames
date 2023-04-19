using UnityEngine;


namespace WasabiGame
{
    public class PlayerInput : MonoBehaviour
    {
        internal System.Action input;
        internal System.Action inputStay;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if(Time.timeScale == 0) Time.timeScale = 1;
                else Time.timeScale = 0;
            }
            if (Input.anyKeyDown)
                input?.Invoke();
            if(Input.anyKey)
                inputStay?.Invoke();
        }
    }
}