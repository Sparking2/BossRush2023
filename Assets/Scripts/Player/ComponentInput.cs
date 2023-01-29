using UnityEngine;

namespace Player
{
    public class ComponentInput : MonoBehaviour
    {
        public delegate void VectorEvent( Vector2 vector );

        public delegate void BoolEvent( float isTriggered );

        private UserActions _userActions;

        public VectorEvent InputEventMove;
        public VectorEvent InputEventLook;
        public BoolEvent InputEventFire;

        private void Start()
        {
            _userActions = new UserActions();
            _userActions.Player.Enable();
        }

        private void Update()
        {
            InputEventMove?.Invoke(_userActions.Player.Move.ReadValue<Vector2>());
            InputEventLook?.Invoke(_userActions.Player.Look.ReadValue<Vector2>());
            InputEventFire?.Invoke(_userActions.Player.Fire.ReadValue<float>());
        }

        private void OnDestroy()
        {
            _userActions.Player.Disable();
            _userActions = null;
        }
    }
}