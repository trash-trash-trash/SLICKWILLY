using System;
using System.Collections.Generic;
using UnityEngine;

public class OceanTracker : MonoBehaviour
{
    public OceanGridGenerator generator;

    public List<OilComponent> totalTiles = new List<OilComponent>();
    public List<OilComponent> cleanTiles = new List<OilComponent>();
    public List<OilComponent> dirtyTiles = new List<OilComponent>();

    [Range(0f, 100f)] public float percentClean = 0f;
    [SerializeField] private RandomAudioPlayer randomAudioPlayer;

    public event Action<float> AnnouncePercentClean;

    public bool gameStarted = false;

    private void OnEnable()
    {
        generator.AnnounceOceanGenerated += OnOceanGenerated;
    }
    

    public void FlipGameStarted(bool input)
    {
        gameStarted = input;
    }
    
    private void OnOceanGenerated()
    {
        RefreshTracking(generator.allOceanTilesOilComponents);
    }

    public void RefreshTracking(List<OilComponent> found)
    {
        UnsubscribeAll();

        totalTiles.Clear();
        cleanTiles.Clear();
        dirtyTiles.Clear();

        foreach (OilComponent oil in found)
        {
            totalTiles.Add(oil);

            if (oil.IsClean)
                cleanTiles.Add(oil);
            else
                dirtyTiles.Add(oil);

            oil.AnnounceCleanOrOily += OnTileCleanStateChanged;
        }

            RecalculatePercentClean();
    }

    private void UnsubscribeAll()
    {
        for (int i = 0; i < totalTiles.Count; i++)
        {
            totalTiles[i].AnnounceCleanOrOily -= OnTileCleanStateChanged;
        }
    }

    private void OnTileCleanStateChanged(OilComponent tile, bool isCleanNow)
    {
        cleanTiles.Remove(tile);
        dirtyTiles.Remove(tile);

        if (isCleanNow)
        {
            cleanTiles.Add(tile);
            randomAudioPlayer.PlayRandomClip();
        }
        else
            dirtyTiles.Add(tile);

            RecalculatePercentClean();
    }

    private void RecalculatePercentClean()
    {
        percentClean = (cleanTiles.Count / (float)totalTiles.Count) * 100f;

        if (gameStarted)
            AnnouncePercentClean?.Invoke(percentClean);
    }

    public void InstantClean()
    {
        foreach (OilComponent oil in totalTiles)
        {
            oil.Clean();
        }
    }

    private void OnDisable()
    {
        if (generator != null)
            generator.AnnounceOceanGenerated -= OnOceanGenerated;

        UnsubscribeAll();
    }
}