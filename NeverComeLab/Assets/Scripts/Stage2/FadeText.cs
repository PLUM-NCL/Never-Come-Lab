using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    public Text stageText;         // �ؽ�Ʈ ������Ʈ ����
    public float fadeDuration = 2f; // ���̵���/���̵�ƿ� �ð�
    private CanvasGroup canvasGroup;

    private void Start()
    {
        // CanvasGroup ������Ʈ�� ã�ų�, ������ �߰��մϴ�.
        canvasGroup = stageText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = stageText.gameObject.AddComponent<CanvasGroup>();
        }

        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        // ���̵� ��
        yield return StartCoroutine(Fade(0f, 1f));
        // ��� ���
        yield return new WaitForSeconds(1f);
        // ���̵� �ƿ�
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
