using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitioner : MonoBehaviour
{
    public static SceneTransitioner Instance;
    Image fadeImage;
    float fadeDuration = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            fadeImage = GameObject.FindWithTag("FadeImageObject").GetComponent<Image>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToScene(int sceneId)
    {
        StartCoroutine(TransitionRoutine(sceneId));
    }

    private IEnumerator TransitionRoutine(int sceneId)
    {
        yield return StartCoroutine(FadeToBlack());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        InitializeLighting();
        yield return StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0;
        Color color = new Color(0, 0, 0, 1);
        if (fadeImage != null)
        {
            color = fadeImage.color;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            if (fadeImage) {
                fadeImage.color = color;
            }
            yield return null;
        }
    }

    private IEnumerator FadeFromBlack()
    {
        fadeImage = GameObject.FindWithTag("FadeImageObject").GetComponent<Image>();

        float elapsedTime = 0;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }


    private void InitializeLighting()
    {
        // Reset or reinitialize lighting settings
        RenderSettings.ambientIntensity = 1.0f;
        RenderSettings.reflectionIntensity = 1.0f;
        // Add any additional lighting settings you need to reset here
    }
}
