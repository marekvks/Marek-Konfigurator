using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    public Animator animator;
    public Button settingsBtn;

    public void Open()
    {
        settingsBtn.enabled = false;
        animator.SetTrigger("open");
    }
}
