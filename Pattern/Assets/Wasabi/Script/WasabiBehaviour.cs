using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wasabi
{



    public class WasabiBehaviour : MonoBehaviour
    {
        internal bool visible = false;

        private void OnBecameVisible()
        {
            visible = true;
        }
        private void OnBecameInvisible()
        {
            visible = false;
        }

        
    }
}