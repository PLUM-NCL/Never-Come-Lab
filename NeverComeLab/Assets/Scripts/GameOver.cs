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

            if (currentScene == "Stage1")
            {
                AudioManager.instance.PlayBgm(AudioManager.Bgm.Stage1); //브금 시작
            }
            else if (currentScene == "Stage2")
            {
                AudioManager.instance.PlayBgm(AudioManager.Bgm.Stage2); //브금 시작
            }

            SceneManager.LoadScene(currentScene);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.health = GameManager.Instance.maxHealth;
            SceneManager.LoadScene("Main_Demo");
        }
    }
}
