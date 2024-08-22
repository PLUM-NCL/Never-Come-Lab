using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public Image Panel;
    float time = 0f;
    float F_time = 1f;

    private void Awake()
    {
        Panel.color = new Color(Panel.color.r, Panel.color.g, Panel.color.b, 0);
    }

    public void FadeOut()   //Á¡Á¡ ¾îµÎ¿öÁü 
    {
        StartCoroutine(CoFadeOut());
    }

    IEnumerator CoFadeOut()
    {
        yield return new WaitForSeconds(1f);

        Panel.gameObject.SetActive(true);
        time = 0;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        yield return null;
    }

    public void FadeIn()    // Á¡Á¡ ¹à¾ÆÁü 
    {
        StartCoroutine(CoFadeIn());
    }

    IEnumerator CoFadeIn()
    {
        yield return new WaitForSeconds(1f);

        Panel.gameObject.SetActive(true);
        time = 0;
        Color alpha = Panel.color;

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);

        yield return null;
    }
}
