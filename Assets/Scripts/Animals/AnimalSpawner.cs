using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalSpawner : MonoBehaviour
{
    public List<Transform> spawnLocLeft;
    public List<Transform> spawnLocRight;

    public float spawnDelay;

    private float delay;
    
    void Start()
    {
        delay = spawnDelay;
    }

    void FixedUpdate()
    {
        spawnDelay -= Time.deltaTime;
        if (spawnDelay <= 0)
        {
            SpawnAnimal();
        }
    }

    void SpawnAnimal()
    {
        GameObject animal = AnimalPooling.SharedInstance.GetPooledObject();
        if (animal != null)
        {
            bool spawnFromLeft = Random.value > 0.5f;

            Transform spawnPoint;
            
            if (spawnFromLeft)
            {
                spawnPoint = spawnLocLeft[Random.Range(0, spawnLocLeft.Count)];

                animal.transform.position = spawnPoint.position;
                animal.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                spawnPoint = spawnLocRight[Random.Range(0, spawnLocRight.Count)];
                animal.transform.position = spawnPoint.position;
                animal.transform.rotation = quaternion.Euler(0,-90,0);
            }
            
            animal.SetActive(true);

            spawnDelay = delay;
        }
    }
}
