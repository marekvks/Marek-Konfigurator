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
        //SetAccessory setAccessory = instantiatedButton.GetComponent<SetAccessory>();
        //setAccessory.dynamicUI = this;
        //setAccessory.tuning = tuningScript;
        //setAccessory.accessoryName = title;
        //setAccessory.accessoryClass = accessoryType;

        instantiatedButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            SetAccessory(title, accessoryType, instantiatedButton);
        });
    }

    public void SetAccessory(string accessoryName, string accessoryClass, GameObject instantiatedButton)
    {
        if (instantiatedButton.transform.parent.name != parents[0].name && instantiatedButton.transform.parent.name != parents[3].name)
        {
            foreach (Tuning.WheelAccessory item in tuningScript.wheelAccessories)
            {
                foreach (GameObject model in item.gameObjects)
                {
                    if (item.name == accessoryName && item.accessoryClass == accessoryClass)
                    {
                        model.SetActive(true);
                    }
                    else if (item.name != accessoryName && item.accessoryClass == accessoryClass)             // lépe optimalizovat, zbavit se dvou foreachů
                    {
                        model.SetActive(false);
                    }
                }
            }
        }
        else if (instantiatedButton.transform.parent.name == parents[0].name)
        {
            foreach (Tuning.Paint item in tuningScript.paints)
            {
                if (item.name == accessoryName)
                {
                    Debug.Log(tuningScript.paints.IndexOf(item));
                    tuningScript.SetPaint(tuningScript.paints.IndexOf(item));
                }
            }
        }
        else if (instantiatedButton.transform.parent.name == parents[3].name)
        {
            foreach (Tuning.Spoiler item in tuningScript.spoilers)
            {
                if (item.name == accessoryName)
                {
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
}