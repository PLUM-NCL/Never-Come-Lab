using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Health }
    public InfoType type;

    Slider mySlider;

    private void Awake()
    {
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Health:
                float curHealth = GameManager.Instance.health;
                float maxHealth = GameManager.Instance.maxHealth;
                //mySlider.value = curHealth / maxHealth;
                mySlider.value = Mathf.Lerp(mySlider.value, curHealth / maxHealth, Time.deltaTime * 10); // 체력이 부드럽게 깎이도록 선형보간 활용
                break;
        }
    }
}
