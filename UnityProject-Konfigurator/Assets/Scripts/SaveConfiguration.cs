using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveConfiguration : MonoBehaviour
{
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
            defaultConfig.tireName = "Stock Tires";
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
}