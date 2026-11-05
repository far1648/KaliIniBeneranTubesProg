using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] GameObject collectEffect;
    [SerializeField] Player player;

    void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        player.Heal();
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
