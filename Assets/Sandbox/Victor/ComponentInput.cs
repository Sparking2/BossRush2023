using System;
using UnityEngine;

namespace Sandbox.Victor
{
    public class ComponentInput : MonoBehaviour
    {
        private UserActions _userActions;

        public delegate void VectorEvent( Vector2 vector );

        public VectorEvent MoveEvent;

        // private Vector2 _moveInput;
        
        private void Start()
        {
            _userActions = new UserActions();
            _userActions.Player.Enable();
            // _moveInput = default;
        }

        private void Update()
        {
            MoveEvent?.Invoke(_userActions.Player.Move.ReadValue<Vector2>());
        }

        private void OnDestroy()
        {
            _userActions.Player.Disable();
            _userActions = null;
        }
    }
}