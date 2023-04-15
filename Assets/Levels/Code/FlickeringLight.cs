using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D light2D;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1f;
    public float flickerSpeed = 10f;

    private float targetIntensity;

    private void Start()
    {

        light2D = this.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        targetIntensity = Random.Range(minIntensity, maxIntensity);

    }

    private void Update()
    {
        float intensity = light2D.intensity;

        if (Mathf.Abs(targetIntensity - intensity) < 0.01f)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }

        intensity = Mathf.Lerp(intensity, targetIntensity, Time.deltaTime * flickerSpeed);

        light2D.intensity = intensity;
    }
}
