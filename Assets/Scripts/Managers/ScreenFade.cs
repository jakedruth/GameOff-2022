using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JDR.Utils;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade instance;
    [SerializeField] private float _fadeTime;
    [SerializeField] private CanvasGroup _backgroundFade;
    [SerializeField] private CanvasGroup _textFade;
    [SerializeField] private Easing.Functions _easingFunction;

    void Awake()
    {
        instance = this;
    }

    [ContextMenu("Fade to black")]
    public void FadeBlack()
    {
        SetFade(FadeValue.BLACK, _fadeTime);
    }

    [ContextMenu("Fade to clear")]
    public void FadeClear()
    {
        SetFade(FadeValue.CLEAR, _fadeTime);
    }

    /// <summary>
    /// Set the screen to fade to the desired setting with the default time
    /// </summary>
    /// <param name="fade">The target fade value</param>
    public void SetFade(FadeValue fade)
    {
        SetFade(fade, _fadeTime);
    }

    /// <summary>
    /// Set the screen to fade to the desired setting with a custom time
    /// </summary>
    /// <param name="fade">The target fade value</param>
    /// <param name="time">The time it will take to fade</param>
    public void SetFade(FadeValue fade, float time)
    {
        StopCoroutine("HandleFade");
        StartCoroutine(HandleFade((float)fade, time));
    }

    private IEnumerator HandleFade(float targetFade, float time)
    {
        float timer = 0;
        float startAlpha = _backgroundFade.alpha;
        EasingFunction function = Easing.EasingFunctionDictionary[_easingFunction];
        while (timer < time)
        {
            timer += Time.deltaTime;

            float p = timer / time;
            float alpha = Mathf.Lerp(startAlpha, targetFade, function(p));
            _backgroundFade.alpha = alpha;

            yield return null;
        }
    }
}

[System.Serializable]
public enum FadeValue : int
{
    CLEAR = 0,
    BLACK = 1,
}
