using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator animator;
    public Button settingsBtn;

    public CameraRotation cameraRotation;

    public void Open()
    {
        animator.SetTrigger("open");
        cameraRotation._canMove = false;
    }

    public void Close()
    {
        animator.SetTrigger("close");
        cameraRotation._canMove = true;
    }
}
