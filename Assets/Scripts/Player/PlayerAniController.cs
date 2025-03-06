using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniController : MonoBehaviour
{
    private Animator animator;
    private static readonly int IsMove = Animator.StringToHash("Moving");
    private static readonly int IsJump = Animator.StringToHash("Jump");
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 dir)
    {
        animator.SetBool(IsMove, dir.magnitude > 0.5f);
    }

    public void Jump()
    {
        animator.SetTrigger(IsJump);
    }
}
