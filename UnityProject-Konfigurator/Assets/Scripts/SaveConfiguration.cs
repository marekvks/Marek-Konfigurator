using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveConfiguration : MonoBehaviour
{
    public DynamicUI dynamicUIScript;

    public class Configuration
    {
        public string tireName;
        public string rimName;
        public string paint;
        public string spoilerName;
    }

    string m_Path;
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

        string json = File.ReadAllText(m_Path);

        configurations = JsonConvert.DeserializeObject<List<Configuration>>(json);
    }

    public void OnClickSave()
    {
        //Debug.Log(dynamicUIScript.currentConfig.paint + " " + dynamicUIScript.currentConfig.tireName + " " + dynamicUIScript.currentConfig.rimName + " " + dynamicUIScript.currentConfig.spoilerName);
        Configuration config = new Configuration
        {
            tireName = dynamicUIScript.currentConfig.tireName,
            paint = dynamicUIScript.currentConfig.paint,
            rimName = dynamicUIScript.currentConfig.rimName,
            spoilerName = dynamicUIScript.currentConfig.spoilerName
        };
        configurations.Add(config);
        SaveToJson(m_Path, configurations);
        dynamicUIScript.savedConfigCount++;
        dynamicUIScript.CreateConfigUI(dynamicUIScript.currentConfig.paint, dynamicUIScript.currentConfig.tireName, dynamicUIScript.currentConfig.rimName, dynamicUIScript.currentConfig.tireName);

        foreach (Configuration conf in configurations)
        {
            Debug.Log(conf.paint + " " + conf.tireName + " " + conf.rimName + " " + conf.spoilerName);
        }
    }
}