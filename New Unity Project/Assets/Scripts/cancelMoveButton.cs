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
        Debug.Log("Delete Current Move Object");
        MoveObject.isCanSelectToMove = true;
        Destroy(transform.parent.gameObject,0.5f);
    }
}
