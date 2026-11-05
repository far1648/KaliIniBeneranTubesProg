using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //BEHOLD BILION IF ELSE STATEMENT 
    Player player;
    [SerializeField] Image health1;
    [SerializeField] Image health2;
    [SerializeField] Image health3;
    [SerializeField] Image health4;
    [SerializeField] Sprite fullHealth;
    [SerializeField] Sprite emptyHealth;
    int health;
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }

    void Update()
    {
        health = player.HP;
        if (health == 4)
        {
            health1.sprite = fullHealth;
            health2.sprite = fullHealth;
            health3.sprite = fullHealth;
            health4.sprite = fullHealth;
        }
        else if (health == 3)
        {
            health1.sprite = fullHealth;
            health2.sprite = fullHealth;
            health3.sprite = fullHealth;
            health4.sprite = emptyHealth;
        }
        else if (health == 2)
        {
            health1.sprite = fullHealth;
            health2.sprite = fullHealth;
            health3.sprite = emptyHealth;
            health4.sprite = emptyHealth;
        } 
        else if (health == 1)
        {
            health1.sprite = fullHealth;
            health2.sprite = emptyHealth;
            health3.sprite = emptyHealth;
            health4.sprite = emptyHealth;
        }
        else if (health == 0)
        {
            health1.sprite = emptyHealth;
            health2.sprite = emptyHealth;
            health3.sprite = emptyHealth;
            health4.sprite = emptyHealth;
        }
    }
}
