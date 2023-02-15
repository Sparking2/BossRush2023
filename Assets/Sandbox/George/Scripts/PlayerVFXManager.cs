using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private TrailRenderer[] moveTrails;

    private ComponentInput _input;
    private CharacterController _characterController;
    private ComponentJump _componentJump;
    private void Start()
    {
        if (!TryGetComponent(out _input))
            throw new Exception($"Can't find {_input.GetType().Name} in player");
        if (!TryGetComponent(out _characterController))
            throw new Exception($"Can't find {_characterController.GetType().Name} in player");
        if (!TryGetComponent(out _componentJump))
            throw new Exception($"Can't find {_componentJump.GetType().Name} in player");
        _input.InputEventJump += OnJumpVFX;
    }

    private void OnDestroy()
    {
        _input.InputEventJump -= OnJumpVFX;

    }

    private void Update()
    {
        for (int i = 0; i < moveTrails.Length; i++)
        {
            moveTrails[i].emitting = _componentJump._groundedPlayer;
        }
    }

    private void OnJumpVFX()
    {
        if(jumpParticles) jumpParticles.Play();
    }

}
