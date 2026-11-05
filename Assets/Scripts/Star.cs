using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] GameObject collectEffect;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
