using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cancelMoveButton : MonoBehaviour
{
    public Animator animator;

    private void OnMouseUp()
    {
        animator.Play("Close Move Object");
        Destroy(animator.gameObject,0.5f);
    }
}
