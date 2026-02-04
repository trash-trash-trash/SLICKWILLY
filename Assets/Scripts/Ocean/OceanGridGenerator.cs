using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class OceanGridGenerator : MonoBehaviour
{
    [Header("Prefabs")] public GameObject waterPrefab;
    public GameObject oilPrefab;
    public GameObject barrierPrefab;

    [Header("Grid Settings")] public int gridWidth = 10;
    public int gridLength = 10;

    [Header("Percentages (must total 100)")] [Range(0, 100)]
    public float oilPercent = 50f;
    [Range(0, 100)] public float waterPercent = 50f;

    [Header("Perlin Noise")] public float perlinScale = 0.2f;
    public Vector2 perlinOffset;

    public float randomOffsetRange = 10000f;

    public List<OilComponent> allOceanTilesOilComponents = new List<OilComponent>();

    public event Action AnnounceOceanGenerated;

#if UNITY_EDITOR
    private void OnValidate()
    {
        oilPercent = Mathf.Max(0, oilPercent);
        waterPercent = Mathf.Max(0, waterPercent);

        float total = oilPercent + waterPercent;
        if (total > 0f)
        {
            oilPercent = (oilPercent / total) * 100f;
            waterPercent = (waterPercent / total) * 100f;
        }
    }
#endif

    private void Start()
    {
        SpawnGrid();
    }


    public void SpawnGrid()
    {
        allOceanTilesOilComponents.Clear();
        
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        // Random perlin offset
        perlinOffset = new Vector2(
            Random.Range(-randomOffsetRange, randomOffsetRange),
            Random.Range(-randomOffsetRange, randomOffsetRange)
        );

        float oilThreshold = oilPercent / 100f;

        // Get prefab size from localScale (assumes cube is uniform in X/Z)
        float prefabWidth = waterPrefab.transform.localScale.x;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridLength; z++)
            {
                float nx = (x * perlinScale) + perlinOffset.x;
                float nz = (z * perlinScale) + perlinOffset.y;

                float noise = Mathf.PerlinNoise(nx, nz);

                GameObject prefabToSpawn = (noise < oilThreshold) ? oilPrefab : waterPrefab;

                Vector3 pos = new Vector3(x * prefabWidth, 0f, z * prefabWidth);
                GameObject tile = Instantiate(prefabToSpawn, pos, Quaternion.identity, transform);
                tile.name = $"{prefabToSpawn.name}_{x}_{z}";

                OceanTile oceanTile = tile.GetComponent<OceanTile>();
                oceanTile.oceanType = (noise < oilThreshold) ? OceanType.Oil : OceanType.Water;

                OilComponent oil = tile.GetComponent<OilComponent>();

                oil.AnnounceCleanOrOily += (oilComp, isClean) => oceanTile.CleanDirty(isClean);

                if
                    (noise < oilThreshold)
                    oil.Dirty();
                else
                    oil.Clean();

                allOceanTilesOilComponents.Add(oil);
                
                bool isEdgeTile =
                    x == 0 || x == gridWidth - 1 ||
                    z == 0 || z == gridLength - 1;

                if (isEdgeTile && barrierPrefab != null)
                {
                    float half = prefabWidth * 0.5f;

                    Vector3 offset = Vector3.zero;
                    Quaternion rot = Quaternion.identity;

                    // push outward depending on which edge we're on
                    if (x == 0)
                        offset = new Vector3(-half, 0f, 0f);          // left wall
                    else if (x == gridWidth - 1)
                        offset = new Vector3(+half, 0f, 0f);          // right wall
                    else if (z == 0)
                    {
                        offset = new Vector3(0f, 0f, -half);          // bottom wall
                        rot = Quaternion.Euler(0f, 90f, 0f);
                    }
                    else if (z == gridLength - 1)
                    {
                        offset = new Vector3(0f, 0f, +half);          // top wall
                        rot = Quaternion.Euler(0f, 90f, 0f);
                    }

                    GameObject barrier = Instantiate(barrierPrefab, pos + offset, rot, transform);
                    barrier.name = $"Barrier_{x}_{z}";
                }

            }
        }

        AnnounceOceanGenerated?.Invoke();
    }
}