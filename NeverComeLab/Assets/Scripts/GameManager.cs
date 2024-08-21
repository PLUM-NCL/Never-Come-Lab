using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    private int killedMonsters = 0;
    public FadeScript fade;
    public GameObject menuSet;

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
            Instance.player = this.player;
            Instance.pool = this.pool;
            Instance.fade = this.fade;
            Instance.menuSet = this.menuSet;
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && !player.isDie)
        {
            if (menuSet == null)
                return;

            if (menuSet.activeSelf)
                menuSet.SetActive(false);
            else
                menuSet.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R) && !player.isDie)
            Retry();
    }

    public void Retry()
    {
        if (!player.isDie)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Main_Demo");
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
