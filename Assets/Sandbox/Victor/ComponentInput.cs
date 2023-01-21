using UnityEngine;

namespace Sandbox.Victor
{
    public class ComponentInput : MonoBehaviour
    {
        private UserActions _userActions;

        public delegate void VectorEvent( Vector2 vector );

        public VectorEvent InputEventMove;
        public VectorEvent InputEventLook;

        private void Start()
        {
            _userActions = new UserActions();
            _userActions.Player.Enable();
        }

        private void Update()
        {
            InputEventMove?.Invoke(_userActions.Player.Move.ReadValue<Vector2>());
            InputEventLook?.Invoke(_userActions.Player.Look.ReadValue<Vector2>());
        }

        private void OnDestroy()
        {
            _userActions.Player.Disable();
            _userActions = null;
        }
    }
}