using System.Collections;
using TMPro;
using UnityEngine;

public class UITextEffects : MonoBehaviour
{
    public static UITextEffects Instance { get; private set; }

    float fadeInDuration = .3f;
    float fadeOutDuration = .3f;
    float displayDuration = 1f;

    void Awake()
    {
     if (Instance == null)
        {
            Instance = this;
        }
    }

    public IEnumerator FadeInOut(TMP_Text text)
    {
        yield return FadeTextAlpha(text,0f, 1f, fadeInDuration);

        yield return new WaitForSeconds(displayDuration);

        yield return FadeTextAlpha(text,1f, 0f, fadeOutDuration);
    }

    public IEnumerator FadeTextAlpha(TMP_Text text, float startAlpha, float targetAlpha, float duration)
    {
        float timer = 0f;
        Color startColor = text.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            text.color = Color.Lerp(startColor, targetColor, progress);
            yield return null;
        }

        text.color = targetColor;
    }
}
