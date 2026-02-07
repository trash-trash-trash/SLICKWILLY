using System.Collections;
using UnityEngine;

public class FrozenAnimal : MonoBehaviour
{
    public DolphinModel dolphin;

    void OnEnable()
    {
        Physics.IgnoreLayerCollision(9,11);
        Shrink();
    }
    
    public void Shrink()
    {
        StartCoroutine(ShrinkCoroutine());
    }

    IEnumerator ShrinkCoroutine()
    {
        float duration = Random.Range(30f, 166f);

        Transform t = gameObject.transform;
        Vector3 startScale = t.localScale;
        Vector3 endScale = startScale * 0.1f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t01 = elapsed / duration;
            t.localScale = Vector3.Lerp(startScale, endScale, t01);

            elapsed += Time.deltaTime;
            yield return null;
        }

        t.localScale = endScale;
        
        dolphin.FlipCanMove(true);

        DestroyImmediate(gameObject);
    }
}