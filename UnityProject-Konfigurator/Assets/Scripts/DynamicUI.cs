using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DynamicUI : MonoBehaviour
{
    [Header("Scripts")]
    public Tuning TuningScript;
    public SaveConfiguration SaveConfig;

    [Header("Prefabs")]
    public GameObject Button;

    [NonSerialized] public int savedConfigCount = 0;

    [Header("UI Parents")]
    public Transform PaintParent;
    public Transform RimParent;
    public Transform TireParent;
    public Transform SpoilerParent;
    public Transform ConfigParent;

    [NonSerialized] public SaveConfiguration.Configuration CurrentConfig = new SaveConfiguration.Configuration();

    private void Awake()
    {
        CurrentConfig = SaveConfig.DefaultConfig; // Načte defaultní config
    }

    private void Start()
    {
        foreach (Tuning.WheelAccessory item in TuningScript.WheelAccessories)
        {
            switch (item.accessoryClass)
            {
                case "Rim":
                    CreateUI(item.accessoryClass, RimParent, item.name, item.gameObjects, item.image); // Vytváření UI pro Rims
                    break;
                case "Tire":
                    CreateUI(item.accessoryClass, TireParent, item.name, item.gameObjects, item.image); // Vytváření UI pro Tires
                    break;
            }
        }

        foreach (Tuning.Paint item in TuningScript.Paints)
        {
            CreateUI("Paint", PaintParent, item.name, null, item.image); // Vytváření UI pro barvy
        }

        foreach (Tuning.Spoiler item in TuningScript.Spoilers)
        {
            CreateUI("Spoiler", SpoilerParent, item.name, null, item.image); // Vytváření UI pro Spoilery
        }

        foreach (SaveConfiguration.Configuration item in SaveConfig.Configurations)
        {
            savedConfigCount++; // Slouží k očíslovaní UI - Uložených konfigurací
            CreateConfigUI(item.paint, item.tireName, item.rimName, item.spoilerName); // Vytváření UI pro uložené konfigurace
        }
    }

    private void CreateUI(string accessoryType, Transform parent, string title, GameObject[] accessory, Sprite image)
    {
        Debug.Log(parent.name);
        GameObject instantiatedButton = Instantiate(Button, parent.transform); // Vytvoří GameObject s parentem ConfigParent a rovnou je uložen do  proměnné instantiatedButton
        instantiatedButton.GetComponent<ConfigUI>().Initialize(title, image, () => SetAccessory(title, accessoryType, instantiatedButton)); // Přes ConfigUI script se na něj nastaví title, náhledovka a funkce - funkci (jen pokud se jedná o void funkci, která nic nevrací) musím nastavovat v anonymní funkci
    }

    public void CreateConfigUI(string paint, string tireName, string rimName, string spoilerName)
    {
        SaveConfiguration.Configuration config = new SaveConfiguration.Configuration()
        {
            paint = paint,
            tireName = tireName,
            rimName = rimName,
            spoilerName = spoilerName,
        };

        GameObject instantiatedButton = Instantiate(Button, ConfigParent); // Vytvoří GameObject s parentem ConfigParent a rovnou je uložen do  proměnné instantiatedButton
        instantiatedButton.GetComponent<ConfigUI>().Initialize($"Config {savedConfigCount}", null, () => SetConfigAccessories(config)); // Zase se přes ConfigUI script nastaví title, náhledovka a funkce - For some reason nemůžu passnout void funkci do System.Action, musím použít anonymní funkci
    }

    private void SetConfigAccessories(SaveConfiguration.Configuration config)
    {
        CurrentConfig = config;

        foreach (Tuning.WheelAccessory item in TuningScript.WheelAccessories)
        {
            foreach (GameObject model in item.gameObjects)
            {
                if (item.name == config.tireName) model.SetActive(true); // SetActive na pneumatiky
                else if (item.name == config.rimName) model.SetActive(true); // SetActive na tires
                else model.SetActive(false); // pokud se jedná o spoiler, tak SetActive(false)
            }
        }

        foreach (Tuning.Paint item in TuningScript.Paints)
        {
            if (item.name == config.paint) TuningScript.SetPaint(TuningScript.Paints.IndexOf(item)); // Nastavování barvy
        }

        foreach (Tuning.Spoiler item in TuningScript.Spoilers)
        {
            if (item.name == config.spoilerName) item.gameObject.SetActive(true); // Nastavování spoileru
            else item.gameObject.SetActive(false);
        }
    }

    public void SetAccessory(string accessoryName, string accessoryClass, GameObject instantiatedButton)
    {
        string parentName = instantiatedButton.transform.parent.name;
        if (parentName != PaintParent.name && parentName != SpoilerParent.name)
        {
            foreach (Tuning.WheelAccessory item in TuningScript.WheelAccessories) // Projede všechny accessories na kola - které jsou v jednom listu
            {
                foreach (GameObject model in item.gameObjects)
                {
                    if (item.accessoryClass != accessoryClass) // pokud se accessory class nerovná tomu, co právě potřebuje, tak přeskočí na další
                        continue;

                    if (item.name == accessoryName)
                    {
                        switch (accessoryClass)
                        {
                            case "Rim":
                                CurrentConfig.rimName = item.name; // Nastavování do current configu - potřebné k savování konfigurace
                                break;
                            case "Tire":
                                CurrentConfig.tireName = item.name; // Nastavování do current configu - potřebné k savování konfigurace
                                break;
                        }
                        model.SetActive(true);
                    }
                    else model.SetActive(false);
                }
            }
        }
        else if (parentName == PaintParent.name)
        {
            foreach (Tuning.Paint item in TuningScript.Paints)
            {
                if (item.name == accessoryName) // Stejná logika pro barvy
                {
                    CurrentConfig.paint = item.name;
                    TuningScript.SetPaint(TuningScript.Paints.IndexOf(item));
                }
            }
        }
        else if (parentName == SpoilerParent.name)
        {
            foreach (Tuning.Spoiler item in TuningScript.Spoilers) // Stejná logika pro křídla
            {
                if (item.name == accessoryName)
                {
                    CurrentConfig.spoilerName = item.name;
                    item.gameObject.SetActive(true);
                } else item.gameObject.SetActive(false);
            }
        }
    }
}