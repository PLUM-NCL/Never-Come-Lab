using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.fade.FadeIn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.fade.FadeOut();
            GameManager.Instance.health = GameManager.Instance.maxHealth;
            string currentScene = PlayerPrefs.GetString("CurrentScene", "Unknown Scene");
            SceneManager.LoadScene(currentScene);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Stage1");
        }
    }
}
