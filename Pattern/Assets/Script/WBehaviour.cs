using UnityEngine;

namespace WasabiTool
{
    public class WBehaviour : MonoBehaviour
    {
        InputHandler mHandler;



        public void Jump()
        {

        }
        private void Update()
        {
            var command = mHandler.HandleInput();
            if (command != null)
                command.Execute(this);
        }
    }
}