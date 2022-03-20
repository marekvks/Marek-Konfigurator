using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveConfiguration : MonoBehaviour
{
    public DynamicUI dynamicUIScript;

    string m_Path;

    public class Configuration
    {
        public string tireName;
        public string rimName;
        public string paint;
        public string spoilerName;
    }

        public List<Configuration> configurations = new List<Configuration>();

    private void Awake()
    {
        m_Path = Application.persistentDataPath + "/save.json";
        LoadFromJson();
    }

    private void SaveToJson(string path, List<Configuration> configs)
    {
        string json = JsonConvert.SerializeObject(configs, Formatting.None);
        File.WriteAllText(path, json);
    }

    private void LoadFromJson()
    {
        if (!File.Exists(m_Path))
        {
            // Default configuration
            Configuration defaultConfig = new Configuration();
            defaultConfig.tireName = "Stock Tire";
            defaultConfig.rimName = "Stock Rims";
            defaultConfig.paint = "Charcoal Grey";
            defaultConfig.spoilerName = "None";

            configurations.Add(defaultConfig);
            SaveToJson(m_Path, configurations);
            return;
        }

        string json;

        json = File.ReadAllText(m_Path);

        configurations = JsonConvert.DeserializeObject<List<Configuration>>(json);
    }

    public void OnClickSave()
    {
        Configuration saveCurrentConfig = dynamicUIScript.currentConfig;
        configurations.Add(saveCurrentConfig);
        SaveToJson(m_Path, configurations);
        dynamicUIScript.savedConfigCount++;
        dynamicUIScript.CreateConfigUI(saveCurrentConfig.paint, saveCurrentConfig.tireName, saveCurrentConfig.rimName, saveCurrentConfig.tireName);
    }
}