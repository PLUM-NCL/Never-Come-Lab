using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public FadeScript fade;
    private void Awake()
    {
        if (GameManager.Instance.fade == null)
        {
            fade = FindObjectOfType<FadeScript>();
        }
        else if (GameManager.Instance.fade != null)
        {
            fade = GameManager.Instance.fade;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            fade.FadeOut();
            GameManager.Instance.health = GameManager.Instance.maxHealth;
            string currentScene = PlayerPrefs.GetString("CurrentScene", "Unknown Scene");
            SceneManager.LoadScene(currentScene);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main_Demo");
        }
    }
}
