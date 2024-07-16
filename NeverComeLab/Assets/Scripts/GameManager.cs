using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PoolManager pool;
    public Player player;

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
}
