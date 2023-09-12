using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace TYCOON
{
    public class ShopTable : MonoBehaviour
    {
        internal enum State { EMPTY, USED, CLEANING };
        State _state = State.EMPTY;

        internal State state { get { return _state; } }

    }
}