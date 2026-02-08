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
    public AnimalController animalController;
    
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
                oceanGenerator.oilPercent = 10f;
                animalController.numberOfDolphins = 2;
                animalController.numberOfWhales = 1;
                animalController.numberOfFrozen = 2;
                animalController.numberOfBuoys = 1;
                break;

            case Difficulty.Medium:
                oceanGenerator.oilPercent = 30f;
                
                animalController.numberOfDolphins = 3;
                animalController.numberOfWhales = 1;
                animalController.numberOfFrozen = 3;
                animalController.numberOfBuoys = 3;
                break;

            case Difficulty.Hard:
                oceanGenerator.oilPercent = 66.6f;
                
                animalController.numberOfDolphins = 5;
                animalController.numberOfWhales = 2;
                animalController.numberOfFrozen = 4;
                animalController.numberOfBuoys = 5;
                break;
        }

        oceanGenerator.waterPercent = 100f - oceanGenerator.oilPercent;
        oceanGenerator.SpawnGrid();
    }
}