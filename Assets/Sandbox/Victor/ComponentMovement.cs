using System;
using UnityEngine;

namespace Sandbox.Victor
{
    public class ComponentMovement : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        private ComponentInput _input;
        private CharacterController _characterController;

        private Transform _playerTransform;
        private Vector3 _currentMovement;
        private Vector3 _forward;
        private Vector3 _right;

        private void Start()
        {
            if ( !TryGetComponent(out _input) )
                throw new Exception($"Can't find {_input.GetType().Name} in player");
            if ( !TryGetComponent(out _characterController) )
                throw new Exception($"Can't find {_characterController.GetType().Name} in player");

            _playerTransform = transform;
            _input.InputEventMove += HandleMove;
        }

        private void OnDestroy()
        {
            _input.InputEventMove -= HandleMove;
        }

        // MoveDirection Y is bind to W,S and 
        // MoveDirection X is bind to A,D
        private void HandleMove( Vector2 moveDirection )
        {
            _forward = _playerTransform.forward;
            _right = _playerTransform.right;
            _currentMovement = _forward * moveDirection.y + _right * moveDirection.x;
            _characterController.Move(_currentMovement * ( speed * Time.deltaTime ));
        }
    }
}