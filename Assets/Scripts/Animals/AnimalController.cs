using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public MenuManager menuManager;
    public OceanGridGenerator oceanGridGenerator;

    public int numberOfDolphins;

    public List<Vector3> allPositions = new List<Vector3>();
    public List<Vector3> randomLocations = new List<Vector3>();

    public GameObject dolphinPrefab;
    public float minDolphinSpacing = 3f;
    public int maxTriesPerDolphin = 20;
    public float yOffset = 1;

    void Start()
    {
        menuManager.AnnounceGameStarted += StartGame;
        oceanGridGenerator.AnnounceOceanGenerated += SetRandomLocations;
    }

    public void StartGame()
    {
        foreach (Vector3 pos in randomLocations)
        {
            Vector3 spawnPos = pos + Vector3.up * yOffset;

            Quaternion rot = Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f
            );

            Instantiate(dolphinPrefab, spawnPos, rot);
        }
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

        for (int i = 0; i < numberOfDolphins; i++)
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