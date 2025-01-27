using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public Color fadeColor;
    public float holdBlackDuration = 2f; // Duration to hold the black screen
    public GameObject[] arrows; // Reference for the arrow GameObjects

    public InputAction fadeAction;

    public GameObject teslaModel3;  // Reference to the Tesla Model 3
    public float fadeDistanceThreshold = 10f; // Distance to the car to trigger fade

    private Renderer rend;
    private Renderer[] arrowRenderers;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Get all renderers from the arrows for fading control
        arrowRenderers = new Renderer[arrows.Length];
        for (int i = 0; i < arrows.Length; i++)
        {
            arrowRenderers[i] = arrows[i].GetComponent<Renderer>();
            SetArrowAlpha(arrowRenderers[i], 0f); // Start arrows fully transparent
        }

        fadeAction.Enable();
        fadeAction.performed += ctx => StartFadeSequence();

        if (fadeOnStart)
            StartFadeSequence();
    }

    private void OnDestroy()
    {
        fadeAction.performed -= ctx => StartFadeSequence();
        fadeAction.Disable();
    }

    public void StartFadeSequence()
    {
        if (ShouldFade())
        {
            StartCoroutine(FadeSequence());
        }
        else
        {
            // If the Tesla is within the fade distance, skip the fade sequence
            Debug.Log("Tesla is within range. Skipping fade.");
        }
    }

    private bool ShouldFade()
    {
        // Calculate the distance between the player and the Tesla Model 3
        float distanceToTesla = Vector3.Distance(Camera.main.transform.position, teslaModel3.transform.position);

        // Check if the distance exceeds the threshold to trigger fade
        return distanceToTesla > fadeDistanceThreshold;
    }

    private IEnumerator FadeSequence()
    {
        // Fade out to black and fade in arrows simultaneously
        StartCoroutine(FadeOut());
        StartCoroutine(FadeArrowsIn());

        yield return new WaitForSeconds(fadeDuration + holdBlackDuration);

        // Fade in to clear and fade out arrows simultaneously
        StartCoroutine(FadeIn());
        StartCoroutine(FadeArrowsOut());
    }

    private IEnumerator FadeOut()
    {
        yield return FadeRoutine(0, 1); // Fade screen to black
    }

    private IEnumerator FadeIn()
    {
        yield return FadeRoutine(1, 0); // Fade screen back to clear
    }

    private void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            rend.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color finalColor = fadeColor;
        finalColor.a = alphaOut;
        rend.material.SetColor("_BaseColor", finalColor);
    }

    private IEnumerator FadeArrowsIn()
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            foreach (var renderer in arrowRenderers)
            {
                SetArrowAlpha(renderer, alpha);
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeArrowsOut()
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            foreach (var renderer in arrowRenderers)
            {
                SetArrowAlpha(renderer, alpha);
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void SetArrowAlpha(Renderer renderer, float alpha)
    {
        if (renderer != null && renderer.material.HasProperty("_BaseColor"))
        {
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.SetColor("_BaseColor", color);
        }
    }
}
