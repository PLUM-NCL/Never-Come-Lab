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
        else if (Input.GetKeyDown(KeyCode.R) && !player.isDie)
            InGame_Menu.Retry();

        else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameObject.Find("Weapon0").GetComponent<Item>().OnClick();
            }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject.Find("Weapon1").GetComponent<Item>().OnClick();
        }
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
