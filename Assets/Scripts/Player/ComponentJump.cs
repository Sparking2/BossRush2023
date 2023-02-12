using System;
using UnityEngine;

namespace Player
{
    public class ComponentJump : MonoBehaviour
    {
        private ComponentInput _input;
        private CharacterController _characterController;
        private bool _groundedPlayer;
        private bool _jumpPressed;
        private Vector3 _playerVelocity;
        [SerializeField]
        private float jumpHeight = 5.0f;
        private const float GravityValue = -9.81f;
        [SerializeField]
        private float jumpUpwardsAccelerator = 1.0f;
        [SerializeField]
        private float jumpDownwardsAccelerator = 1.0f;
        
        private void Start()
        {
            if ( !TryGetComponent(out _input) )
                throw new Exception($"Can't find {_input.GetType().Name} in player");
            if ( !TryGetComponent(out _characterController) )
                throw new Exception($"Can't find {_characterController.GetType().Name} in player");

            _input.InputEventJump += OnJump;
        }

        private void Update()
        {
            MovementJump();
        }

        private void MovementJump()
        {
            if ( Physics.Raycast(_characterController.transform.position, Vector3.down, out RaycastHit hit, 0.1f) )
            {
                _groundedPlayer = true;
            }
            else
            {
                _groundedPlayer = false;
            }

            if ( _groundedPlayer )
            {
                _playerVelocity.y = 0.0f;
            }

            if ( _jumpPressed && _groundedPlayer )
            {
                _playerVelocity.y += Mathf.Sqrt(jumpHeight * -jumpUpwardsAccelerator * GravityValue);
                _jumpPressed = false;
            }

            _playerVelocity.y += (GravityValue * jumpDownwardsAccelerator) * Time.deltaTime;
            _characterController.Move(_playerVelocity * Time.deltaTime);
        }

        private void OnJump()
        {
            if ( _characterController.velocity.y == 0 && _groundedPlayer )
                _jumpPressed = true;
        }

        private void OnDestroy()
        {
            _input.InputEventJump -= OnJump;
        }
    }
}