using System.Linq;
using TMPro;
using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class DifficultyPicker : MonoBehaviour
{
    public OceanGridGenerator oceanGenerator;

    public TMP_Dropdown dropdown;
    public Difficulty currentDifficulty;

    void Start()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(System.Enum.GetNames(typeof(Difficulty)).ToList());

        dropdown.onValueChanged.AddListener(OnDifficultyChanged);
        
        dropdown.value = (int)Difficulty.Medium;
        OnDifficultyChanged(dropdown.value);
    }

    public void Reroll()
    {
        oceanGenerator.SpawnGrid();
    }

    void OnDifficultyChanged(int index)
    {
        currentDifficulty = (Difficulty)index;

        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                oceanGenerator.oilPercent = 20f;
                break;

            case Difficulty.Medium:
                oceanGenerator.oilPercent = 50f;
                break;

            case Difficulty.Hard:
                oceanGenerator.oilPercent = 80f;
                break;
        }

        oceanGenerator.waterPercent = 100f - oceanGenerator.oilPercent;
        oceanGenerator.SpawnGrid();
    }
}