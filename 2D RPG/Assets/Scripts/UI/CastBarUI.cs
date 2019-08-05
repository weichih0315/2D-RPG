using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastBarUI : MonoBehaviour {

    private static CastBarUI instance;
    public static CastBarUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CastBarUI>();
            }

            return instance;
        }
    }

    [SerializeField]
    private float fadeTime;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text descripts;

    [SerializeField]
    private Image fillImage;

    [SerializeField]
    private Text castTime;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private Coroutine fadeRoutine, progressRoutine;

    public void PopCastBar(Sprite icon,string descripts,float castTime,Color color)
    {
        this.icon.sprite = icon;
        this.descripts.text = descripts;
        this.fillImage.color = color;

        if (progressRoutine != null)
            StopCoroutine(progressRoutine);
        progressRoutine = StartCoroutine(Progress(castTime));

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeBar());
    }

    private IEnumerator Progress(float castTime)
    {
        float timePassed = Time.deltaTime;

        float rate = 1.0f / castTime;

        float percent = 0f;

        while (percent <= 1f)
        {
            fillImage.fillAmount = Mathf.Lerp(0, 1, percent);
            percent += rate * Time.deltaTime;
            timePassed += Time.deltaTime;
            this.castTime.text = (castTime - timePassed).ToString("F2");

            if (castTime - timePassed < 0)
            {
                this.castTime.text = "0.00";
            }

            yield return null;
        }
        StopCating();
    }

    private IEnumerator FadeBar()
    {
        float rate = 1f / fadeTime;

        float percent = 0f;

        while (percent <= 1f)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, percent);
            percent += rate * Time.deltaTime;
            yield return null;
        }
    }
    public void StopCating()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }

        if (progressRoutine != null)
        {
            StopCoroutine(progressRoutine);
            progressRoutine = null;
        }
    }
}
