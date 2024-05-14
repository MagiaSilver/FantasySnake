using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimate : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    public void Hit()
    {
        animator.SetTrigger("Hit");
    }
    public void Victory()
    {
        animator.SetTrigger("Victory");
    }
    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
}
