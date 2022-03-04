using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAccessory : MonoBehaviour
{
    public DynamicUI dynamicUI;
    public Tuning tuning;

    public string accessoryName;
    public string accessoryClass;

    private void Awake()
    {
        dynamicUI = GameObject.Find("UI Manager").GetComponent<DynamicUI>();
    }

    public void OnClick()
    {
        if (gameObject.transform.parent.name != dynamicUI.parents[0].name && gameObject.transform.parent.name != dynamicUI.parents[3].name)
        {
            foreach (Tuning.WheelAccessory item in tuning.wheelAccessories)
            {
                if (item.name == accessoryName && item.accessoryClass == accessoryClass)
                {
                    foreach (GameObject model in item.gameObjects)
                    {
                        model.SetActive(true);
                    }
                } else if (item.name != accessoryName && item.accessoryClass == accessoryClass)             // lépe optimalizovat, zbavit se dvou foreachů
                {
                    foreach (GameObject model in item.gameObjects)
                    {
                        model.SetActive(false);
                    }
                }
            }
        } else if (gameObject.transform.parent.name == dynamicUI.parents[0].name)
        {
            foreach (Tuning.Paint item in tuning.paints)
            {
                if (item.name == accessoryName)
                {
                    Debug.Log(tuning.paints.IndexOf(item));
                    tuning.SetPaint(tuning.paints.IndexOf(item));
                }
            }
        } else if (gameObject.transform.parent.name == dynamicUI.parents[3].name)
        {
            foreach (Tuning.Spoiler item in tuning.spoilers)
            {
                Debug.Log("sus");
                if (item.name == accessoryName)
                {
                    item.gameObject.SetActive(true);
                } else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
}