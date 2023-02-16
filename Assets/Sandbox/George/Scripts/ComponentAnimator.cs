using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentAnimator : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        animator.SetBool("isActive", true);
    }
}
