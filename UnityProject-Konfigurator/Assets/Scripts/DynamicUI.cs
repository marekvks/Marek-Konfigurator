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

    public SaveConfiguration saveConfig;
    public int savedConfigCount = 0;

    public List<GameObject> parents = new List<GameObject>();       // 0 - Paint, 1 - Rim, 2 - Tire, 3 - Spoiler

    public SaveConfiguration.Configuration currentConfig = new SaveConfiguration.Configuration();

    private void Start()
    {
        currentConfig.tireName = "Stock Tire";
        currentConfig.rimName = "Stock Rims";
        currentConfig.paint = "Charcoal Grey";
        currentConfig.spoilerName = "None";

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

        foreach (SaveConfiguration.Configuration item in saveConfig.configurations)
        {
            savedConfigCount++;
            CreateConfigUI(item.paint, item.tireName, item.rimName, item.spoilerName);
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

    public void CreateConfigUI(string paint, string tireName, string rimName, string spoilerName)
    {
        SaveConfiguration.Configuration config = new SaveConfiguration.Configuration();
        config.paint = paint;
        config.tireName = tireName;
        config.rimName = rimName;
        config.spoilerName = spoilerName;

        GameObject instantiatedButton = Instantiate(button);
        instantiatedButton.transform.parent = parents[4].transform;
        instantiatedButton.transform.localScale = new Vector3(1, 1, 1);
        this.title = instantiatedButton.GetComponentInChildren<TextMeshProUGUI>();
        this.title.text = "Config" + savedConfigCount;

        instantiatedButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            SetConfigAccessories(config);
        });
    }

    private void SetConfigAccessories(SaveConfiguration.Configuration config)
    {
        currentConfig.paint = config.paint;
        currentConfig.tireName = config.tireName;
        currentConfig.rimName = config.rimName;
        currentConfig.spoilerName = config.spoilerName;
        Debug.Log(config.paint);

        foreach (Tuning.WheelAccessory item in tuningScript.wheelAccessories)
        {
            foreach (GameObject model in item.gameObjects)
            {
                if (item.name == config.tireName) model.SetActive(true);
                else if (item.name == config.rimName) model.SetActive(true);
                else model.SetActive(false);
            }
        }

        foreach (Tuning.Paint item in tuningScript.paints)
        {
            if (item.name == config.paint) tuningScript.SetPaint(tuningScript.paints.IndexOf(item));
        }

        foreach (Tuning.Spoiler item in tuningScript.spoilers)
        {
            if (item.name == config.spoilerName) item.gameObject.SetActive(true);
            else item.gameObject.SetActive(false);
        }
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
                        switch (accessoryClass)
                        {
                            case "Rim":
                                currentConfig.rimName = item.name;
                                break;
                            case "Tire":
                                currentConfig.tireName = item.name;
                                break;
                            default:c
                                break;
                        }
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
                    currentConfig.paint = item.name;
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
                    currentConfig.spoilerName = item.name;
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