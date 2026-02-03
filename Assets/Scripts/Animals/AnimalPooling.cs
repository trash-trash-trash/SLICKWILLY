using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPooling : MonoBehaviour
{
    public static AnimalPooling SharedInstance;

    public List<GameObject> pooledDolphins;

    public GameObject dolphinObjToPool;

    public int dolphinAmountToPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledDolphins = new List<GameObject>();
        GameObject dolphinTemp;
        for (int i = 0; i < dolphinAmountToPool; i++)
        {
            dolphinTemp = Instantiate(dolphinObjToPool);
            dolphinTemp.SetActive(false);
            pooledDolphins.Add(dolphinTemp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < dolphinAmountToPool; i++)
        {
            if (!pooledDolphins[i].activeInHierarchy)
            {
                return pooledDolphins[i];
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
