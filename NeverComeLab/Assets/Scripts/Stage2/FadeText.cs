using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    public Text stageText;         // 텍스트 컴포넌트 참조
    public float fadeDuration = 2f; // 페이드인/페이드아웃 시간
    private CanvasGroup canvasGroup;

    private void Start()
    {
        // CanvasGroup 컴포넌트를 찾거나, 없으면 추가합니다.
        canvasGroup = stageText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = stageText.gameObject.AddComponent<CanvasGroup>();
        }

        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        // 페이드 인
        yield return StartCoroutine(Fade(0f, 1f));
        // 잠시 대기
        yield return new WaitForSeconds(1f);
        // 페이드 아웃
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
