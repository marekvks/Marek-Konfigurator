using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator animator;
    public Button settingsBtn;

    public void Open()
    {
        animator.SetTrigger("open");
    }

    public void Close()
    {
        animator.SetTrigger("close");
    }
}
