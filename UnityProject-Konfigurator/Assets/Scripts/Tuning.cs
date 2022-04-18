using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tuning : MonoBehaviour
{
    public Renderer CarRenderer;

    public List<WheelAccessory> WheelAccessories = new List<WheelAccessory>();
    public List<Paint> Paints = new List<Paint>();
    public List<Spoiler> Spoilers = new List<Spoiler>();


    [System.Serializable]
    public class WheelAccessory
    {
        public string name;
        public string accessoryClass;
        public Sprite image; // Sprite pro náhledovku
        public GameObject[] gameObjects = new GameObject[4]; // 4 kola
    }

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

    private void Awake()
    {
        SetPaint(1);
    }

    public void SetPaint(int index)
    {
        CarRenderer.material = Paints[index].material;
    }
}