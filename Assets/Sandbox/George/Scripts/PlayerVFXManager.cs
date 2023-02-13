using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem shootParticles;
    [SerializeField] private ParticleSystem jumpParticles;

    private ComponentInput _input;
    private CharacterController _characterController;
    private void Start()
    {
        if (!TryGetComponent(out _input))
            throw new Exception($"Can't find {_input.GetType().Name} in player");
        if (!TryGetComponent(out _characterController))
            throw new Exception($"Can't find {_characterController.GetType().Name} in player");
        _input.InputEventJump += OnJumpVFX;
    }

    private void OnDestroy()
    {
        _input.InputEventJump -= OnJumpVFX;

    }


    private void OnJumpVFX()
    {
        jumpParticles.Play();
    }

}
