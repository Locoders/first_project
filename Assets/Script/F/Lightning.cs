using System.Collections;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Transform p1;
    public Transform p2;
    public Transform p3;
    private LineRenderer lineRenderer;
    public int segments = 10;
    public float noiseAmount = 0.2f;
    public float fadeDuration = 1f; // 사라지는 데 걸리는 시간

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = (segments * 2) + 1;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        StartCoroutine(Routine1());
    }

    IEnumerator Routine1()
    {
        while (true)
        {
            LightningEffect();
            yield return new WaitForSeconds(1f);
            StartCoroutine(FadeOut());
        }
    }

    void LightningEffect()
    {
        Vector3[] positions = new Vector3[(segments * 2) + 1];
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 point = Vector3.Lerp(p1.position, p2.position, t);
            
            point.x += Random.Range(-noiseAmount, noiseAmount);
            point.y += Random.Range(-noiseAmount, noiseAmount);
            point.z += Random.Range(-noiseAmount, noiseAmount);
            positions[i] = point;
        }
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 point = Vector3.Lerp(p2.position, p3.position, t);
            
            point.x += Random.Range(-noiseAmount, noiseAmount);
            point.y += Random.Range(-noiseAmount, noiseAmount);
            point.z += Random.Range(-noiseAmount, noiseAmount);
            positions[segments + i] = point;
        }
        lineRenderer.SetPositions(positions);
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Gradient gradient = lineRenderer.colorGradient;
        GradientAlphaKey[] alphaKeys = gradient.alphaKeys;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            for (int i = 0; i < alphaKeys.Length; i++)
            {
                alphaKeys[i].alpha = alpha;
            }

            gradient.SetKeys(gradient.colorKeys, alphaKeys);
            lineRenderer.colorGradient = gradient;

            yield return null;
        }
    }
}
