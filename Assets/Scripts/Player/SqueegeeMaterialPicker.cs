using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SqueegeeMaterialPicker : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown materialDropdown;
    public Image selectedImage;

    [Header("Target")]
    public Renderer targetRenderer;

    [System.Serializable]
    private struct SqueegeeOption
    {
        public string label;
        public Material material;
        public Sprite sprite;
    }

    [SerializeField] private SqueegeeOption[] options;

    void OnEnable()
    {
        materialDropdown.ClearOptions();
        materialDropdown.AddOptions(GetLabels());
        materialDropdown.onValueChanged.AddListener(Apply);
        
        int safeIndex = Mathf.Clamp(materialDropdown.value, 0, options.Length - 1);
        Apply(safeIndex);
    }

    void Apply(int index)
    {
        if (options == null || options.Length == 0 || index < 0 || index >= options.Length)
            return; // prevent out-of-bounds

        if (targetRenderer != null)
            targetRenderer.material = options[index].material;

        if (selectedImage != null)
            selectedImage.sprite = options[index].sprite;

        if (materialDropdown != null && materialDropdown.captionText != null)
            materialDropdown.captionText.text = options[index].label;
    }

    List<string> GetLabels()
    {
        List<string> labels = new List<string>(options.Length);
        foreach (var o in options)
            labels.Add(o.label);
        return labels;
    }

    public void SetRandomSqueegeeMaterial()
    {
        Apply(Random.Range(0, options.Length));
        materialDropdown.value = materialDropdown.value;
    }
}