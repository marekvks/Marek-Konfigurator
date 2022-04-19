using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveConfiguration : MonoBehaviour
{
    [Header("Scripts")]
    public DynamicUI dynamicUIScript;

    public class Configuration
    {
        public string tireName;
        public string rimName;
        public string paint;
        public string spoilerName;
    }
    
    public Configuration DefaultConfig = new Configuration() // Defaultní konfigurace, použitá v případě, že nemám vytvořený soubor save.json
    {
        tireName = "Stock Tire",
        rimName = "Stock Rims",
        paint = "Charcoal Grey",
        spoilerName = "None"
    };

    string m_Path; // Cesta k souboru - deklarovaná ve funkci Awake
    [NonSerialized] public List<Configuration> Configurations = new List<Configuration>(); // List konfigurací

    private void Awake()
    {
        m_Path = Application.persistentDataPath + "/save.json";
        LoadFromJson(); // V Awake funkci načítám z Jsonu
    }

    private void SaveToJson(string path, List<Configuration> configs)
    {
        string json = JsonConvert.SerializeObject(configs, Formatting.None); // Serializování listu "configs" do json, ten potom do proměnné string (bez formátování)
        File.WriteAllText(path, json); // Přepisování souboru
    }

    private void LoadFromJson()
    {
        if (!File.Exists(m_Path)) // Pokud neexistuje soubor save.json ...
        {
            Configurations.Add(DefaultConfig); // Přidá se do listu "Configurations" defaultní konfigurace
            SaveToJson(m_Path, Configurations); // Poté se spustí funkce, která mi konfiguraci uloží
            return; // funkce skončí
        }

        string json = File.ReadAllText(m_Path); // Přepisování souboru

        Configurations = JsonConvert.DeserializeObject<List<Configuration>>(json); // Deserializování listu uložených konfigurací (Configurations) a z toho se přímo nastavuje list "Configurations"
    }

    public void OnClickSave()
    {
        Configuration config = dynamicUIScript.CurrentConfig; // Braní si currentConfig ze scriptu DynamicUI.cs a ukládání si ho do proměnné "config"
        Configurations.Add(config); // Přidávání proměnné "config" do listu
        SaveToJson(m_Path, Configurations); // Ukládání listu konfigurací
        dynamicUIScript.savedConfigCount++; // Přičítání slouží k očíslovaní UI
        dynamicUIScript.CreateConfigUI(dynamicUIScript.CurrentConfig.paint, dynamicUIScript.CurrentConfig.tireName, dynamicUIScript.CurrentConfig.rimName, dynamicUIScript.CurrentConfig.spoilerName); // Vytvoření UI
    }
}