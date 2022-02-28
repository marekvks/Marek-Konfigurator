using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuning : MonoBehaviour
{
    [System.Serializable]
    public class tire
    {
        public int id;
        public GameObject model;
    }

    [System.Serializable]
    public class rim
    {
        public int id;
        public GameObject model;
    }
    
    [System.Serializable]
    public class spoiler
    {
        public int id;
        public GameObject model;
    }

    public List<tire> tires = new List<tire>();
    public List<rim> rims = new List<rim>();
    public List<spoiler> spoilers = new List<spoiler>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RandomTire();
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RandomRim();
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RandomSpoiler();
        }
    }

    private void RandomTire()
    {
        int randomIndex = Random.Range(1, 4);
        for (int i = 0; i < tires.Count; i++)
        {
            if (tires[i].id == randomIndex)
            {
                tires[i].model.gameObject.SetActive(true);
            } else
            {
                tires[i].model.SetActive(false);
            }
        }
    }
    
    private void RandomRim()
    {
        int randomIndex = Random.Range(1, 4);
        for (int i = 0; i < rims.Count; i++)
        {
            if (rims[i].id == randomIndex)
            {
                rims[i].model.gameObject.SetActive(true);
            } else
            {
                rims[i].model.SetActive(false);
            }
        }
    }
    
    private void RandomSpoiler()
    {
        int randomIndex = Random.Range(1, 8);
        for (int i = 0; i < spoilers.Count; i++)
        {
            if (spoilers[i].id == randomIndex)
            {
                spoilers[i].model.gameObject.SetActive(true);
            } else
            {
                spoilers[i].model.SetActive(false);
            }
        }
    }
}