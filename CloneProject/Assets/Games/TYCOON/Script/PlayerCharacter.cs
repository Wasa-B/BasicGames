using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TYCOON
{
    public class PlayerCharacter : MonoBehaviour
    {

        PlayerActions inputActions;

        [SerializeField]
        float moveSpeed = 1;

        private void Awake()
        {
            inputActions = new PlayerActions();


            inputActions.gameplay.use.performed += OnUse;
        }

        private void OnUse(InputAction.CallbackContext obj)
        {
            Debug.Log("Use");
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            var inputDir = inputActions.gameplay.move.ReadValue<Vector2>();
            MoveUpdate(inputDir);
        }

        void MoveUpdate(Vector2 dir)
        {
            var moveAmount = Vector2.right * dir.x *moveSpeed * Time.fixedDeltaTime;

            transform.Translate(moveAmount);

        }

        private void OnEnable()
        {
            inputActions.gameplay.Enable();
        }
        private void OnDisable()
        {
            inputActions.gameplay.Disable();
        }
    }
}