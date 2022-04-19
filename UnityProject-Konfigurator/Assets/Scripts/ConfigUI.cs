using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigUI : MonoBehaviour
{
    [Header("Components")]
    public Button Btn;
    public TMP_Text Title;
    public Image Img;

    public void Initialize(string title, Sprite img, Action callback)
    {
        Title.text = title; // Nastavení titlu
        Img.sprite = img; // Nastavení náhledovky
        Btn.onClick.AddListener(() => callback()); // Přidávání funkce (onClick)
        transform.localScale = Vector3.one; // localScale na normální, protože se to vždycky smrskne, když se UI vytvoří
    }
}
