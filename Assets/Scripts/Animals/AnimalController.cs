using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalController : MonoBehaviour
{
    public MenuManager menuManager;
    public OceanGridGenerator oceanGridGenerator;

    public int numberOfDolphins=4;
    public int numberOfWhales=1;

    public List<Vector3> allPositions = new List<Vector3>();
    public List<Vector3> randomLocations = new List<Vector3>();

    public GameObject dolphinPrefab;
    public GameObject whalePrefab;
    
    public float minDolphinSpacing = 3f;
    public int maxTriesPerDolphin = 20;
    public float yOffset = 1;
    
    public List<GameObject> animalsSpawned = new List<GameObject>();
    public List<GameObject> dolphinsSpawned = new List<GameObject>();
    public List<GameObject> whalesSpawned = new List<GameObject>();
    
    public event Action AnnounceAnimalsSpawned;
    
    void OnEnable()
    {
        menuManager.AnnounceGameStarted += StartGame;
        oceanGridGenerator.AnnounceOceanGenerated += SetRandomLocations;
    }

    public void StartGame()
    {
        int index = 0;

        for (int i = 0; i < numberOfDolphins; i++)
        {
            dolphinsSpawned.Add(Spawn(dolphinPrefab, randomLocations[index]));
            index++;
        }

        for (int i = 0; i < numberOfWhales; i++)
        {
            whalesSpawned.Add(Spawn(whalePrefab, randomLocations[index]));
            index++;
        }
        
        AnnounceAnimalsSpawned?.Invoke();
    }
    
    GameObject Spawn(GameObject prefab, Vector3 pos)
    {
        Vector3 spawnPos = pos + Vector3.up * yOffset;

        Quaternion rot = Quaternion.Euler(
            0f,
            Random.Range(0f, 360f),
            0f
        );

        GameObject spawnedAnimal = Instantiate(prefab, spawnPos, rot);
        animalsSpawned.Add(spawnedAnimal);
        return  spawnedAnimal;
    }

    public void ResetAnimals()
    {
        List<GameObject> copy = new List<GameObject>(animalsSpawned);

        foreach (GameObject animal in copy)
        {
            animalsSpawned.Remove(animal);
            Destroy(animal);
        }

        animalsSpawned.Clear();
        dolphinsSpawned.Clear();
        whalesSpawned.Clear();
    }


    public void SetRandomLocations()
    {
        allPositions.Clear();
        randomLocations.Clear();

        foreach (OilComponent oil in oceanGridGenerator.allOceanTilesOilComponents)
        {
            allPositions.Add(oil.transform.position);
        }

        if (allPositions.Count == 0) return;
        
        for (int i = 0; i < numberOfDolphins + numberOfWhales; i++)
        {
            bool foundSpot = false;

            for (int attempt = 0; attempt < maxTriesPerDolphin; attempt++)
            {
                Vector3 candidate = allPositions[Random.Range(0, allPositions.Count)];

                bool tooClose = false;
                foreach (Vector3 chosen in randomLocations)
                {
                    if (Vector3.Distance(candidate, chosen) < minDolphinSpacing)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    randomLocations.Add(candidate);
                    foundSpot = true;
                    break;
                }
            }

            if (!foundSpot)
            {
                randomLocations.Add(allPositions[Random.Range(0, allPositions.Count)]);
            }
        }
    }

    void OnDisable()
    {
        menuManager.AnnounceGameStarted -= StartGame;
        oceanGridGenerator.AnnounceOceanGenerated -= SetRandomLocations;
    }
}