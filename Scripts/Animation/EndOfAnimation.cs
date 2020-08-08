using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfAnimation : MonoBehaviour
{
    public Animator animator;

    public void EndAnimation()
    {
        animator.SetBool("rewinding", false);
    }
}
