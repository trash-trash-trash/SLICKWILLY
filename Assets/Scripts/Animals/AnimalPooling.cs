using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AnimalPooling : MonoBehaviour
{
    public static AnimalPooling SharedInstance;

    public List<GameObject> pooledDolphins;
    public List<GameObject> pooledWhales;

    public GameObject dolphinObjToPool;
    public GameObject whaleObjToPool;
    
    public int dolphinAmountToPool;
    public int whaleAmountToPool;


    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        // create the dolphins
        pooledDolphins = new List<GameObject>();
        GameObject dolphinTemp;
        for (int i = 0; i < dolphinAmountToPool; i++)
        {
            dolphinTemp = Instantiate(dolphinObjToPool);
            dolphinTemp.SetActive(false);
            pooledDolphins.Add(dolphinTemp);
        }
        
        // create the whales
        pooledWhales = new List<GameObject>();
        GameObject whaleTemp;
        for (int i = 0; i < whaleAmountToPool; i++)
        {
            whaleTemp = Instantiate(whaleObjToPool);
            whaleTemp.SetActive(false);
            pooledWhales.Add(whaleTemp);
        }
    }

    public GameObject GetPooledObject()
    {
        bool spawnWhale = Random.value > 0.5f;

        if (spawnWhale)
        {
            for (int i = 0; i < whaleAmountToPool; i++)
            {
                if (!pooledWhales[i].activeInHierarchy)
                {
                    return pooledWhales[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < dolphinAmountToPool; i++)
            {
                if (!pooledDolphins[i].activeInHierarchy)
                {
                    return pooledDolphins[i];
                }
            }
        }
        
        return null;
    }

    public void DisableAllPooledObjects()
    {
        for (int i = 0; i < dolphinAmountToPool; i++)
        {
            if (pooledDolphins[i].activeInHierarchy)
            {
                pooledDolphins[i].SetActive(false);
            }
        }
        for (int i = 0; i < whaleAmountToPool; i++)
        {
            if (pooledWhales[i].activeInHierarchy)
            {
                pooledWhales[i].SetActive(false);
            }
        }
    }
}
