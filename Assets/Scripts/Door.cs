using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject pintuBuka;
    [SerializeField] GameObject pintuTutup;
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }

    void Update()
    {
        if (player.starCount == 3)
        {
            pintuBuka.SetActive(false);
            pintuTutup.SetActive(true);
        }
    }
}
