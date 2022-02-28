using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColours : MonoBehaviour
{
    public Renderer renderer;
    Color materialColor;

    private void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            RandomColour();
        }
    }

    private void RandomColour()
    {
        byte R = (byte)Random.Range(0, 255);
        byte G = (byte)Random.Range(0, 256);
        byte B = (byte)Random.Range(0, 256);

        materialColor = new Color32(R, G, B, 255);
        renderer.material.SetColor("_Color", materialColor);
    }
}
