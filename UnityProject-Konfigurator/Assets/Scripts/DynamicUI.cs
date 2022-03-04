using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DynamicUI : MonoBehaviour
{
    public Tuning tuningScript;

    public GameObject button;
    public Image[] image;
    TextMeshProUGUI title;

    public GameObject Rim;
    public GameObject Tire;

    public List<GameObject> parents = new List<GameObject>();       // 0 - Paint, 1 - Rim, 2 - Tire, 3 - Spoiler

    private void Awake()
    {
        foreach (Tuning.WheelAccessory item in tuningScript.wheelAccessories)
        {
            switch (item.accessoryClass)
            {
                case "Rim":
                    CreateUI(item.accessoryClass, parents[1], item.name, item.gameObjects, item.image);
                    break;
                case "Tire":
                    CreateUI(item.accessoryClass, parents[2], item.name, item.gameObjects, item.image);
                    break;
                default:
                    break;
            }
        }

        foreach (Tuning.Paint item in tuningScript.paints)
        {
            CreateUI("Paint", parents[0], item.name, null, item.image);
        }

        foreach (Tuning.Spoiler item in tuningScript.spoilers)
        {
            CreateUI("Spoiler", parents[3], item.name, null, item.image);
        }
    }

    private void CreateUI(string accessoryType, GameObject parent, string title, GameObject[] accessory, Sprite image)
    {
        GameObject instantiatedButton = Instantiate(button);
        instantiatedButton.transform.parent = parent.transform;
        instantiatedButton.transform.localScale = new Vector3(1, 1, 1);
        this.image = instantiatedButton.GetComponentsInChildren<Image>();
        this.image[1].sprite = image;
        this.title = instantiatedButton.GetComponentInChildren<TextMeshProUGUI>();
        this.title.text = title;
        SetAccessory setAccessory = instantiatedButton.GetComponent<SetAccessory>();
        setAccessory.dynamicUI = this;
        setAccessory.tuning = tuningScript;
        setAccessory.accessoryName = title;
        setAccessory.accessoryClass = accessoryType;
    }
}