using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.speed = 0f;
        } else
        {
            animator.speed = 1f;
        }
    }
}
