using System.Collections;
using TMPro;
using UnityEngine;

public class ComboText : MonoBehaviour
{
    [Header("References")]
    public Cleaner cleaner;
    public TMP_Text comboTextPrefab; 
    
    [Header("Animation Settings")]
    public float scrollDistance = 250f;
    public float fadeDuration = 5;
    public float scaleMultiplier = 5f;

    void OnEnable()
    {
        cleaner.AnnounceCurrentCombo += OnCurrentComboChanged;
        cleaner.AnnounceBiggestCombo += OnBiggestComboChanged;
    }

    private void OnCurrentComboChanged(int value)
    {
        comboTextPrefab.text = value.ToString("N0");

        // Spawn a flavor combo if divisible by certain values
        if (value != 0)
        {
            if (value % 1000000 == 0)
            {
                SpawnFloatingText(new string[] { "SLICK!!", "FULLY SLICK!!", "ECONOMICAL!!", "SUSTAINABLE!!" });
            }
            else if (value % 100000 == 0)
            {
                SpawnFloatingText(new string[] { "NOICE", "SICK", "RIPPER", "FULLY SICK", "AWESOME", "OARSOME", "KILLER" });
            }
            else if (value % 10000 == 0)
            {
                SpawnFloatingText(new string[] { "ON YA MATE", "CLEAN", "NOT BAD", "KEEP ON KEEPIN ON" });
            }
        }
    }

    private void OnBiggestComboChanged(int value)
    {
        SpawnFloatingText("A NEW RECORD!!");
    }

    private void SpawnFloatingText(string[] options)
    {
        string msg = options[Random.Range(0, options.Length)];
        SpawnFloatingText(msg);
    }

    private void SpawnFloatingText(string message)
    {
        TMP_Text floatingText = Instantiate(comboTextPrefab, comboTextPrefab.transform.parent);
        floatingText.text = message;
        floatingText.rectTransform.anchoredPosition = comboTextPrefab.rectTransform.anchoredPosition;
        floatingText.alpha = 1f;
        floatingText.transform.localScale = Vector3.one;

        StartCoroutine(AnimateFloatingText(floatingText));
    }

    private IEnumerator AnimateFloatingText(TMP_Text text)
    {
        RectTransform rt = text.rectTransform;
        Vector3 startPos = rt.anchoredPosition;
        Vector3 endPos = startPos + Vector3.up * scrollDistance;
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.one * scaleMultiplier;

        float elapsed = 0f;
        Color c = text.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            rt.anchoredPosition = Vector3.Lerp(startPos, endPos, t);
            rt.localScale = Vector3.Lerp(startScale, endScale, t);
            c.a = Mathf.Lerp(1f, 0f, t);
            text.color = c;

            yield return null;
        }

        Destroy(text.gameObject);
    }

    void OnDisable()
    {
        cleaner.AnnounceCurrentCombo -= OnCurrentComboChanged;
        cleaner.AnnounceBiggestCombo -= OnBiggestComboChanged;
    }
}
