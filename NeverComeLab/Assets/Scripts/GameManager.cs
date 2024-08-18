using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    private int killedMonsters = 0;

    [Header("# Player Info")]
    public int health;
    public int maxHealth = 100 ;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void IncrementKilledMonsters()
    {
        killedMonsters++;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
