using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseImage : MonoBehaviour
{
    public List<RectTransform> rects = new List<RectTransform>();

    public float scaleMultiplier = 1.1f;

    public float duration = 1f;

    private List<Vector3> baseScales = new List<Vector3>();
    private List<Vector3> maxScales = new List<Vector3>();

    void Start()
    {
        baseScales.Clear();
        maxScales.Clear();

        foreach (RectTransform rect in rects)
        {
            baseScales.Add(rect.localScale);
            maxScales.Add(rect.localScale * scaleMultiplier);
        }

        StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        while (true)
        {
            yield return ScaleOverTime(baseScales, maxScales, duration);
            yield return ScaleOverTime(maxScales, baseScales, duration);
        }
    }

    IEnumerator ScaleOverTime(List<Vector3> fromList, List<Vector3> toList, float time)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / time;

            for (int i = 0; i < rects.Count; i++)
            {
                rects[i].localScale = Vector3.Lerp(fromList[i], toList[i], t);
            }

            yield return null;
        }

        for (int i = 0; i < rects.Count; i++)
        {
            rects[i].localScale = toList[i];
        }
    }
}