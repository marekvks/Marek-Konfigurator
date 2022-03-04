using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tuning : MonoBehaviour
{
    [System.Serializable]
    public class WheelAccessory
    {
        public string name;
        public string accessoryClass;
        public Sprite image;
        public GameObject[] gameObjects = new GameObject[4];
    }

    public Renderer carRenderer;

    [System.Serializable]
    public class Paint
    {
        public string name;
        public Sprite image;
        public Material material;
    }

    [System.Serializable]
    public class Spoiler
    {
        public string name;
        public Sprite image;
        public GameObject gameObject;
    }

    public List<WheelAccessory> wheelAccessories = new List<WheelAccessory>();
    public List<Paint> paints = new List<Paint>();
    public List<Spoiler> spoilers = new List<Spoiler>();

    private void Awake()
    {
        SetPaint(1);
    }

    public void SetPaint(int index)
    {
        carRenderer.material = paints[index].material;
    }
}