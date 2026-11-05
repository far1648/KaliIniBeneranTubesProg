using UnityEngine;

public class PlayerSceneManqager : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject gameOverPanel;
    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (player.HP <= 0)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
