using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_Menu : MonoBehaviour
{
    public static void Retry()
    {
        if (!GameManager.Instance.player.isDie)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void Exit()
    {
        GameManager.Instance.health = GameManager.Instance.maxHealth;
        SceneManager.LoadScene("Main_Demo");
    }

    public static void GoStage2()
    {
        GameManager.Instance.health = GameManager.Instance.maxHealth;
        SceneManager.LoadScene("Stage2");
    }
}
